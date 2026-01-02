using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetlinkCore.Protocol;

internal unsafe ref struct SpanWriter
{
    private readonly Span<byte> _buffer;
    private readonly ref uint _externalLength;
    private int _position = 0;

    public readonly ReadOnlySpan<byte> Written => _buffer[.._position];

    public SpanWriter(Span<byte> buffer, ref uint externalLength)
    {
        _buffer = buffer;
        _externalLength = ref externalLength;
    }

    public SpanWriter(Span<byte> buffer) : this(buffer, ref Unsafe.NullRef<uint>())
    {
    }

    public Span<byte> Skip(int length)
    {
        var slice = _buffer.Slice(_position, length);
        _position += Alignment.Align(length);
        if (!Unsafe.IsNullRef(ref _externalLength))
            _externalLength += (uint)length;
        return slice;
    }

    public ref T Skip<T>() where T : unmanaged => ref Unsafe.As<byte, T>(ref MemoryMarshal.GetReference(Skip(sizeof(T))));

    public void Write(ReadOnlySpan<byte> data) => data.CopyTo(Skip(data.Length));

    public void Write<T>(in T value) where T : unmanaged => Skip<T>() = value;
}