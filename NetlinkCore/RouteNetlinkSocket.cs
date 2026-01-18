using System;
using System.Collections.Generic;

using NetlinkCore.Interop.Route;
using NetlinkCore.Protocol.Route;

using NetNsCore;

using NetworkingPrimitivesCore;

namespace NetlinkCore;

public sealed class RouteNetlinkSocket() : NetlinkSocket(NetlinkFamily.Route)
{
    #region Links

    public Link GetLink(string name)
    {
        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Small);
        var writer = GetWriter<ifinfomsg, ifinfomsg_type, IFLA_ATTRS>(buffer);
        writer.Type = ifinfomsg_type.RTM_GETLINK;
        writer.Flags = NetlinkMessageFlags.Request;
        writer.Attributes.Write(IFLA_ATTRS.IFLA_IFNAME, name);
        foreach (var message in Get(buffer, writer))
            if (message.Type == ifinfomsg_type.RTM_NEWLINK)
                return ParseLink(message);
        throw new InvalidOperationException($"Link with name '{name}' not found.");
    }

    public Link[] GetLinks()
    {
        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Large);
        var writer = GetWriter<ifinfomsg, ifinfomsg_type, IFLA_ATTRS>(buffer);
        writer.Type = ifinfomsg_type.RTM_GETLINK;
        writer.Flags = NetlinkMessageFlags.Request | NetlinkMessageFlags.Dump;
        var links = new List<Link>();
        foreach (var message in Get(buffer, writer))
            if (message.Type == ifinfomsg_type.RTM_NEWLINK)
                links.Add(ParseLink(message));
        return [.. links];
    }

    public void UpdateLink(Link origLink, Link link)
    {
        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Small);
        var writer = GetWriter<ifinfomsg, ifinfomsg_type, IFLA_ATTRS>(buffer);
        writer.Type = ifinfomsg_type.RTM_SETLINK;
        writer.Flags = NetlinkMessageFlags.Request | NetlinkMessageFlags.Ack;
        writer.Header.ifi_index = origLink.Index;
        if (origLink.Up != link.Up)
        {
            writer.Header.ifi_flags = link.Up ? net_device_flags.IFF_UP : 0;
            writer.Header.ifi_change = net_device_flags.IFF_UP;
        }
        if (origLink.Name != link.Name)
            writer.Attributes.Write(IFLA_ATTRS.IFLA_IFNAME, link.Name);
        if (origLink.MacAddress != link.MacAddress && link.MacAddress is { } macAddress)
            writer.Attributes.Write(IFLA_ATTRS.IFLA_ADDRESS, macAddress);
        if (origLink.MasterIndex != link.MasterIndex)
            writer.Attributes.Write(IFLA_ATTRS.IFLA_MASTER, link.MasterIndex ?? 0);
        Post(buffer, writer);
    }

    public void DeleteLink(string name)
    {
        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Small);
        var writer = GetWriter<ifinfomsg, ifinfomsg_type, IFLA_ATTRS>(buffer);
        writer.Type = ifinfomsg_type.RTM_DELLINK;
        writer.Flags = NetlinkMessageFlags.Request | NetlinkMessageFlags.Ack;
        writer.Attributes.Write(IFLA_ATTRS.IFLA_IFNAME, name);
        Post(buffer, writer);
    }

    public void CreateVEth(string name, string peerName, int? rxQueueCount = null, int? txQueueCount = null)
    {
        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Small);
        var writer = GetWriter<ifinfomsg, ifinfomsg_type, IFLA_ATTRS>(buffer);
        writer.Type = ifinfomsg_type.RTM_NEWLINK;
        writer.Flags = NetlinkMessageFlags.Request | NetlinkMessageFlags.Create | NetlinkMessageFlags.Exclusive | NetlinkMessageFlags.Ack;
        writer.Attributes.Write(IFLA_ATTRS.IFLA_IFNAME, name);
        if (rxQueueCount is not null)
            writer.Attributes.Write(IFLA_ATTRS.IFLA_NUM_RX_QUEUES, rxQueueCount.Value);
        if (txQueueCount is not null)
            writer.Attributes.Write(IFLA_ATTRS.IFLA_NUM_TX_QUEUES, txQueueCount.Value);
        using (var infoAttrs = writer.Attributes.WriteNested<IFLA_INFO_ATTRS>(IFLA_ATTRS.IFLA_LINKINFO))
        {
            infoAttrs.Writer.Write(IFLA_INFO_ATTRS.IFLA_INFO_KIND, "veth");
            using var vethAttrs = infoAttrs.Writer.WriteNested<VETH_INFO_ATTRS>(IFLA_INFO_ATTRS.IFLA_INFO_DATA);
            using var peerAttrs = vethAttrs.Writer.WriteNested<IFLA_ATTRS, ifinfomsg>(VETH_INFO_ATTRS.VETH_INFO_PEER);
            peerAttrs.Header = default;
            peerAttrs.Writer.Write(IFLA_ATTRS.IFLA_IFNAME, peerName);
            if (rxQueueCount is not null)
                peerAttrs.Writer.Write(IFLA_ATTRS.IFLA_NUM_RX_QUEUES, rxQueueCount.Value);
            if (txQueueCount is not null)
                peerAttrs.Writer.Write(IFLA_ATTRS.IFLA_NUM_TX_QUEUES, txQueueCount.Value);
        }
        Post(buffer, writer);
    }

    public void CreateBridge(string name)
    {
        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Small);
        var writer = GetWriter<ifinfomsg, ifinfomsg_type, IFLA_ATTRS>(buffer);
        writer.Type = ifinfomsg_type.RTM_NEWLINK;
        writer.Flags = NetlinkMessageFlags.Request | NetlinkMessageFlags.Create | NetlinkMessageFlags.Exclusive | NetlinkMessageFlags.Ack;
        writer.PortId = PortId;
        writer.Attributes.Write(IFLA_ATTRS.IFLA_IFNAME, name);
        using (var infoAttrs = writer.Attributes.WriteNested<IFLA_INFO_ATTRS>(IFLA_ATTRS.IFLA_LINKINFO))
            infoAttrs.Writer.Write(IFLA_INFO_ATTRS.IFLA_INFO_KIND, "bridge");
        Post(buffer, writer);
    }

    private static Link ParseLink(RouteNetlinkMessage<ifinfomsg, ifinfomsg_type, IFLA_ATTRS> message)
    {
        var ifIndex = message.Header.ifi_index;
        var up = (message.Header.ifi_flags & net_device_flags.IFF_UP) != 0;
        string? name = null;
        MACAddress? macAddress = null;
        int? masterIndex = null;
        var rxQueueCount = 0;
        var txQueueCount = 0;
        foreach (var attribute in message.Attributes)
        {
            switch (attribute.Name)
            {
                case IFLA_ATTRS.IFLA_IFNAME:
                    name = attribute.AsString();
                    break;
                case IFLA_ATTRS.IFLA_ADDRESS:
                    macAddress = attribute.AsValue<MACAddress>();
                    break;
                case IFLA_ATTRS.IFLA_MASTER:
                    masterIndex = attribute.AsValue<int>();
                    break;
                case IFLA_ATTRS.IFLA_NUM_RX_QUEUES:
                    rxQueueCount = attribute.AsValue<int>();
                    break;
                case IFLA_ATTRS.IFLA_NUM_TX_QUEUES:
                    txQueueCount = attribute.AsValue<int>();
                    break;
            }
        }
        return name is null
            ? throw new InvalidOperationException($"Link with index '{ifIndex}' is missing a name attribute.")
            : new Link(ifIndex, name, up, macAddress, masterIndex, rxQueueCount, txQueueCount);
    }

    private RouteNetlinkMessageWriter<THeader, TMsgType, TAttr> GetWriter<THeader, TMsgType, TAttr>(Span<byte> buffer)
        where THeader : unmanaged
        where TMsgType : unmanaged, Enum
        where TAttr : unmanaged, Enum
    {
        return new RouteNetlinkMessageWriter<THeader, TMsgType, TAttr>(buffer)
        {
            PortId = PortId,
            Header = default
        };
    }

    private RouteNetlinkMessageCollection<THeader, TMsgType, TAttr> Get<THeader, TMsgType, TAttr>(Span<byte> buffer, RouteNetlinkMessageWriter<THeader, TMsgType, TAttr> message)
        where THeader : unmanaged
        where TMsgType : unmanaged, Enum
        where TAttr : unmanaged, Enum
    {
        Send(message.Written);
        var receivedLength = Receive(buffer);
        var received = (ReadOnlySpan<byte>)buffer[..receivedLength];
        return new RouteNetlinkMessageCollection<THeader, TMsgType, TAttr>(received);
    }

    private void Post<THeader, TMsgType, TAttr>(Span<byte> buffer, RouteNetlinkMessageWriter<THeader, TMsgType, TAttr> message)
        where THeader : unmanaged
        where TMsgType : unmanaged, Enum
        where TAttr : unmanaged, Enum
    {
        foreach (var _ in Get(buffer, message)) ;
    }

    #endregion

    #region Generic Messages

    public void CreateNetNsId(NetNs ns)
    {
        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Small);
        var writer = GetWriter<rtgenmsg, rtgenmsg_type, NETNSA_ATTRS>(buffer);
        writer.Type = rtgenmsg_type.RTM_NEWNSID;
        writer.Flags = NetlinkMessageFlags.Request | NetlinkMessageFlags.Create | NetlinkMessageFlags.Exclusive | NetlinkMessageFlags.Ack;
        writer.Attributes.Write(NETNSA_ATTRS.NETNSA_FD, ns.Descriptor);
        writer.Attributes.Write(NETNSA_ATTRS.NETNSA_NSID, -1);
        Post(buffer, writer);
    }

    public int? GetNetNsId(NetNs ns)
    {
        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Small);
        var writer = GetWriter<rtgenmsg, rtgenmsg_type, NETNSA_ATTRS>(buffer);
        writer.Type = rtgenmsg_type.RTM_GETNSID;
        writer.Flags = NetlinkMessageFlags.Request;
        writer.Attributes.Write(NETNSA_ATTRS.NETNSA_FD, ns.Descriptor);
        foreach (var message in Get(buffer, writer))
            if (message.Type == rtgenmsg_type.RTM_NEWNSID)
                foreach (var attribute in message.Attributes)
                    if (attribute.Name == NETNSA_ATTRS.NETNSA_NSID)
                    {
                        var value = attribute.AsValue<int>();
                        return value >= 0 ? value : null;
                    }
        throw new InvalidOperationException("NetNsId not found");
    }

    #endregion
}