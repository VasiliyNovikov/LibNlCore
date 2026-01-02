using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using NetlinkCore.Interop.Route;
using NetlinkCore.Protocol.Route;

using NetworkingPrimitivesCore;

namespace NetlinkCore;

public sealed class RouteNetlinkSocket() : NetlinkSocket(NetlinkFamily.Route)
{
    public Link GetLink(string name)
    {
        GetBuffer(out var buffer);
        var messageWriter = new RouteNetlinkMessageWriter<ifinfomsg, ifinfomsg_type, IFLA_ATTRS>(buffer)
        {
            Type = ifinfomsg_type.RTM_GETLINK,
            Flags = NetlinkMessageFlags.Request,
            PortId = PortId,
            Header = default
        };
        messageWriter.Attributes.Write(IFLA_ATTRS.IFLA_IFNAME, name);
        Send(messageWriter.Written);
        var receivedLength = Receive(buffer);
        var received = (ReadOnlySpan<byte>)buffer[..receivedLength];
        foreach (var message in new RouteNetlinkMessageCollection<ifinfomsg, ifinfomsg_type, IFLA_ATTRS>(received))
            if (message.Type == ifinfomsg_type.RTM_NEWLINK)
                return ParseLink(message);
        throw new InvalidOperationException($"Link with name '{name}' not found.");
    }

    public Link[] GetLinks()
    {
        GetBuffer(out var buffer);
        var messageWriter = new RouteNetlinkMessageWriter<ifinfomsg, ifinfomsg_type, IFLA_ATTRS>(buffer)
        {
            Type = ifinfomsg_type.RTM_GETLINK,
            Flags = NetlinkMessageFlags.Request | NetlinkMessageFlags.Dump,
            PortId = PortId,
            Header = default
        };
        Send(messageWriter.Written);
        var receivedLength = Receive(buffer);
        var received = (ReadOnlySpan<byte>)buffer[..receivedLength];
        var links = new List<Link>();
        foreach (var message in new RouteNetlinkMessageCollection<ifinfomsg, ifinfomsg_type, IFLA_ATTRS>(received))
            if (message.Type == ifinfomsg_type.RTM_NEWLINK)
                links.Add(ParseLink(message));
        return [.. links];
    }

    public void DeleteLink(string name)
    {
        GetBuffer(out var buffer);
        var messageWriter = new RouteNetlinkMessageWriter<ifinfomsg, ifinfomsg_type, IFLA_ATTRS>(buffer)
        {
            Type = ifinfomsg_type.RTM_DELLINK,
            Flags = NetlinkMessageFlags.Request | NetlinkMessageFlags.Ack,
            PortId = PortId,
            Header = default
        };
        messageWriter.Attributes.Write(IFLA_ATTRS.IFLA_IFNAME, name);
        Send(messageWriter.Written);
        var receivedLength = Receive(buffer);
        var received = (ReadOnlySpan<byte>)buffer[..receivedLength];
        foreach (var _ in new RouteNetlinkMessageCollection<ifinfomsg, ifinfomsg_type, IFLA_ATTRS>(received)) ;
    }

    public void CreateVEth(string name, string peerName)
    {
        GetBuffer(out var buffer);
        var messageWriter = new RouteNetlinkMessageWriter<ifinfomsg, ifinfomsg_type, IFLA_ATTRS>(buffer)
        {
            Type = ifinfomsg_type.RTM_NEWLINK,
            Flags = NetlinkMessageFlags.Request | NetlinkMessageFlags.Create | NetlinkMessageFlags.Exclusive | NetlinkMessageFlags.Ack,
            PortId = PortId,
            Header = default
        };
        messageWriter.Attributes.Write(IFLA_ATTRS.IFLA_IFNAME, name);
        using (var infoAttrs = messageWriter.Attributes.WriteNested<IFLA_INFO_ATTRS>(IFLA_ATTRS.IFLA_LINKINFO))
        {
            infoAttrs.Writer.Write(IFLA_INFO_ATTRS.IFLA_INFO_KIND, "veth");
            using var vethAttrs = infoAttrs.Writer.WriteNested<VETH_INFO_ATTRS>(IFLA_INFO_ATTRS.IFLA_INFO_DATA);
            using var peerAttrs = vethAttrs.Writer.WriteNested<IFLA_ATTRS, ifinfomsg>(VETH_INFO_ATTRS.VETH_INFO_PEER);
            peerAttrs.Header = default;
            peerAttrs.Writer.Write(IFLA_ATTRS.IFLA_IFNAME, peerName);
        }
        Send(messageWriter.Written);
        var receivedLength = Receive(buffer);
        var received = (ReadOnlySpan<byte>)buffer[..receivedLength];
        foreach (var _ in new RouteNetlinkMessageCollection<ifinfomsg, ifinfomsg_type, IFLA_ATTRS>(received)) ;
    }

    private static Link ParseLink(RouteNetlinkMessage<ifinfomsg, ifinfomsg_type, IFLA_ATTRS> message)
    {
        var ifIndex = message.Header.ifi_index;
        string? name = null;
        MACAddress? macAddress = null;
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
            }
        }
        return name is null
            ? throw new InvalidOperationException($"Link with index '{ifIndex}' is missing a name attribute.")
            : new Link(ifIndex, name, macAddress);
    }

    private static void GetBuffer(out NetlinkBuffer buffer) => Unsafe.SkipInit(out buffer);
}