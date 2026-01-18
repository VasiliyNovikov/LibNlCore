using System;

namespace NetlinkCore.Protocol.Route;

internal readonly ref struct RouteNetlinkMessage<THeader, TAttr>
    where THeader : unmanaged
    where TAttr : unmanaged, Enum
{
    private readonly ref readonly THeader _header;

    public NetlinkMessageFlags Flags { get; }
    public RouteNetlinkMessageType Type { get; }
    public ref readonly THeader Header => ref _header;
    public NetlinkAttributeCollection<TAttr> Attributes { get; }

    public RouteNetlinkMessage(NetlinkMessageFlags flags, RouteNetlinkMessageType type, in THeader header, NetlinkAttributeCollection<TAttr> attributes)
    {
        _header = ref header;
        Flags = flags;
        Type = type;
        Attributes = attributes;
    }
}