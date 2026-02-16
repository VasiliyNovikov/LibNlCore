using System;

namespace NetlinkCore.Protocol;

internal readonly ref struct NetlinkMessage(ushort type, ReadOnlySpan<byte> payload)
{
    public ushort Type => type;
    public ReadOnlySpan<byte> Payload { get; } = payload;
}

internal readonly ref struct NetlinkMessage<THeader, TAttr>
    where THeader : unmanaged
    where TAttr : unmanaged, Enum
{
    private readonly ref readonly THeader _header;

    public ushort Type { get; }
    public ref readonly THeader Header => ref _header;
    public NetlinkAttributeCollection<TAttr> Attributes { get; }

    public NetlinkMessage(ushort type, in THeader header, NetlinkAttributeCollection<TAttr> attributes)
    {
        _header = ref header;
        Type = type;
        Attributes = attributes;
    }
}