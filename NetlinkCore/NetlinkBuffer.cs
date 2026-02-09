using System;
using System.Buffers;

namespace NetlinkCore;

internal readonly struct NetlinkBuffer : IDisposable
{
    private readonly byte[] _buffer;

    public NetlinkBuffer(NetlinkBufferSize size)
    {
        _buffer = ArrayPool<byte>.Shared.Rent((int)size);
        Array.Clear(_buffer);
    }

    public void Dispose() => ArrayPool<byte>.Shared.Return(_buffer);

    public static implicit operator Span<byte>(NetlinkBuffer buffer) => buffer._buffer;
}