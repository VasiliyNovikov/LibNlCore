using System;

namespace NetlinkCore.Protocol;

internal readonly ref struct NetlinkMessage(NetlinkMessageFlags flags, ReadOnlySpan<byte> payload)
{
    public NetlinkMessageFlags Flags { get; } = flags;
    public ReadOnlySpan<byte> Payload { get; } = payload;
}