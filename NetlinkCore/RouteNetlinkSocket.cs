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
        var writer = GetWriter<ifinfomsg, IFLA_ATTRS>(buffer);
        writer.Type = RouteNetlinkMessageType.GetLink;
        writer.Flags = NetlinkMessageFlags.Request;
        writer.Attributes.Write(IFLA_ATTRS.IFLA_IFNAME, name);
        foreach (var message in Get(buffer, writer))
            if (message.Type == RouteNetlinkMessageType.NewLink)
                return ParseLink(message);
        throw new InvalidOperationException($"Link with name '{name}' not found");
    }

    public Link GetLink(int index)
    {
        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Small);
        var writer = GetWriter<ifinfomsg, IFLA_ATTRS>(buffer);
        writer.Type = RouteNetlinkMessageType.GetLink;
        writer.Flags = NetlinkMessageFlags.Request;
        writer.Header.ifi_index = index;
        foreach (var message in Get(buffer, writer))
            if (message.Type == RouteNetlinkMessageType.NewLink)
                return ParseLink(message);
        throw new InvalidOperationException($"Link with index '{index}' not found");
    }

    public Link[] GetLinks()
    {
        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Large);
        var writer = GetWriter<ifinfomsg, IFLA_ATTRS>(buffer);
        writer.Type = RouteNetlinkMessageType.GetLink;
        writer.Flags = NetlinkMessageFlags.Request | NetlinkMessageFlags.Dump;
        var links = new List<Link>();
        foreach (var message in Get(buffer, writer))
            if (message.Type == RouteNetlinkMessageType.NewLink)
                links.Add(ParseLink(message));
        return [.. links];
    }

    public void UpdateLink(Link origLink, Link link)
    {
        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Small);
        var writer = GetWriter<ifinfomsg, IFLA_ATTRS>(buffer);
        writer.Type = RouteNetlinkMessageType.SetLink;
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
        var writer = GetWriter<ifinfomsg, IFLA_ATTRS>(buffer);
        writer.Type = RouteNetlinkMessageType.DeleteLink;
        writer.Flags = NetlinkMessageFlags.Request | NetlinkMessageFlags.Ack;
        writer.Attributes.Write(IFLA_ATTRS.IFLA_IFNAME, name);
        Post(buffer, writer);
    }

    public void DeleteLink(int index)
    {
        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Small);
        var writer = GetWriter<ifinfomsg, IFLA_ATTRS>(buffer);
        writer.Type = RouteNetlinkMessageType.DeleteLink;
        writer.Flags = NetlinkMessageFlags.Request | NetlinkMessageFlags.Ack;
        writer.Header.ifi_index = index;
        Post(buffer, writer);
    }

    public void CreateVEth(string name, string peerName, int? rxQueueCount = null, int? txQueueCount = null)
    {
        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Small);
        var writer = BeginCreateLink(buffer, name, rxQueueCount, txQueueCount);
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

    public void CreateBridge(string name, int? rxQueueCount = null, int? txQueueCount = null)
    {
        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Small);
        var writer = BeginCreateLink(buffer, name, rxQueueCount, txQueueCount);
        using (var infoAttrs = writer.Attributes.WriteNested<IFLA_INFO_ATTRS>(IFLA_ATTRS.IFLA_LINKINFO))
            infoAttrs.Writer.Write(IFLA_INFO_ATTRS.IFLA_INFO_KIND, "bridge");
        Post(buffer, writer);
    }

    public void MoveTo(int index, NetNs ns)
    {
        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Small);
        var writer = GetWriter<ifinfomsg, IFLA_ATTRS>(buffer);
        writer.Type = RouteNetlinkMessageType.NewLink;
        writer.Flags = NetlinkMessageFlags.Request | NetlinkMessageFlags.Ack;
        writer.Header.ifi_index = index;
        writer.Attributes.Write(IFLA_ATTRS.IFLA_NET_NS_FD, ns.Descriptor);
        Post(buffer, writer);
    }

    private RouteNetlinkMessageWriter<ifinfomsg, IFLA_ATTRS> BeginCreateLink(Span<byte> buffer, string name, int? rxQueueCount, int? txQueueCount)
    {
        var writer = GetWriter<ifinfomsg, IFLA_ATTRS>(buffer);
        writer.Type = RouteNetlinkMessageType.NewLink;
        writer.Flags = NetlinkMessageFlags.Request | NetlinkMessageFlags.Create | NetlinkMessageFlags.Exclusive | NetlinkMessageFlags.Ack;
        writer.Attributes.Write(IFLA_ATTRS.IFLA_IFNAME, name);
        if (rxQueueCount is not null)
            writer.Attributes.Write(IFLA_ATTRS.IFLA_NUM_RX_QUEUES, rxQueueCount.Value);
        if (txQueueCount is not null)
            writer.Attributes.Write(IFLA_ATTRS.IFLA_NUM_TX_QUEUES, txQueueCount.Value);
        return writer;
    }

    private static Link ParseLink(RouteNetlinkMessage<ifinfomsg, IFLA_ATTRS> message)
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

    #endregion

    private RouteNetlinkMessageWriter<THeader, TAttr> GetWriter<THeader, TAttr>(Span<byte> buffer)
        where THeader : unmanaged
        where TAttr : unmanaged, Enum
    {
        return new RouteNetlinkMessageWriter<THeader, TAttr>(buffer)
        {
            PortId = PortId,
            Header = default
        };
    }

    private RouteNetlinkMessageCollection<THeader, TAttr> Get<THeader, TAttr>(Span<byte> buffer, RouteNetlinkMessageWriter<THeader, TAttr> message)
        where THeader : unmanaged
        where TAttr : unmanaged, Enum
    {
        return new(base.Get(buffer, message.Writer));
    }

    private void Post<THeader, TAttr>(Span<byte> buffer, RouteNetlinkMessageWriter<THeader, TAttr> message)
        where THeader : unmanaged
        where TAttr : unmanaged, Enum
    {
        base.Post(buffer, message.Writer);
    }
}