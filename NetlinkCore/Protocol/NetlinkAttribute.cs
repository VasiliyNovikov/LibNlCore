using System;

namespace NetlinkCore.Protocol;

public readonly ref struct NetlinkAttribute<TName>(TName name, ReadOnlySpan<byte> data) where TName : unmanaged, Enum
{
    public TName Name => name;
    public ReadOnlySpan<byte> Data { get; } = data;
}