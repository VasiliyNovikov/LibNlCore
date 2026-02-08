using System;

namespace NetlinkCore.Protocol;

internal readonly ref struct NetlinkMessage(NetlinkMessageType type, int subType, NetlinkMessageFlags flags, ReadOnlySpan<byte> payload)
{
    public NetlinkMessageType Type => type;
    public int SubType => subType;
    public NetlinkMessageFlags Flags => flags;
    public ReadOnlySpan<byte> Payload { get; } = payload;
}

internal readonly ref struct NetlinkMessage<THeader, TAttr>
    where THeader : unmanaged
    where TAttr : unmanaged, Enum
{
    private readonly ref readonly THeader _header;

    public NetlinkMessageFlags Flags { get; }
    public int SubType { get; }
    public ref readonly THeader Header => ref _header;
    public NetlinkAttributeCollection<TAttr> Attributes { get; }

    public NetlinkMessage(NetlinkMessageFlags flags, int subType, in THeader header, NetlinkAttributeCollection<TAttr> attributes)
    {
        _header = ref header;
        Flags = flags;
        SubType = subType;
        Attributes = attributes;
    }
}