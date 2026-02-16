using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace NetlinkCore.Protocol;

internal readonly unsafe ref struct NetlinkAttributeWriter<TAttr>(SpanWriter writer)
    where TAttr : unmanaged, Enum
{
    private readonly SpanWriter _writer = writer;

    public Span<byte> PrepareWrite(TAttr name, int length)
    {
        ref var header = ref _writer.Skip<NetlinkAttributeHeader<TAttr>>();
        header.Name = name;
        header.IsNested = false;
        header.IsNetworkByteOrder = false;
        header.Length = (ushort)(length + Unsafe.SizeOf<NetlinkAttributeHeader<TAttr>>());
        return _writer.Skip(length);
    }

    public void WriteFlag(TAttr name) => PrepareWrite(name, 0);

    public void Write<T>(TAttr name, in T value) where T : unmanaged => MemoryMarshal.Write(PrepareWrite(name, sizeof(T)), value);

    public void Write(TAttr name, ReadOnlySpan<byte> value) => value.CopyTo(PrepareWrite(name, value.Length));

    public void Write(TAttr name, string value)
    {
        var buffer = PrepareWrite(name, Encoding.UTF8.GetByteCount(value) + 1);
        Encoding.UTF8.GetBytes(value, buffer);
        buffer[^1] = 0;
    }

    public NestedScope<TNestedAttr> WriteNested<TNestedAttr>(TAttr name)
        where TNestedAttr : unmanaged, Enum
    {
        ref var header = ref _writer.Skip<NetlinkAttributeHeader<TAttr>>();
        header.Name = name;
        header.IsNested = true;
        header.IsNetworkByteOrder = false;
        return new NestedScope<TNestedAttr>(_writer, ref header.Length);
    }

    public NestedScope<TNestedAttr, TNestedHeader> WriteNested<TNestedAttr, TNestedHeader>(TAttr name)
        where TNestedAttr : unmanaged, Enum
        where TNestedHeader : unmanaged
    {
        ref var header = ref _writer.Skip<NetlinkAttributeHeader<TAttr>>();
        header.Name = name;
        header.IsNested = true;
        header.IsNetworkByteOrder = false;
        ref var nestedHeader = ref _writer.Skip<TNestedHeader>();
        return new NestedScope<TNestedAttr, TNestedHeader>(_writer, ref header.Length, ref nestedHeader);
    }

    public readonly ref struct NestedScope<TNestedAttr> : IDisposable
        where TNestedAttr : unmanaged, Enum
    {
        private readonly uint _writerStart;
        private readonly ref readonly uint _writerLength;
        private readonly ref ushort _length;

        public NetlinkAttributeWriter<TNestedAttr> Writer { get; }

        internal NestedScope(SpanWriter writer, ref ushort length)
        {
            _writerStart = writer.Length;
            _writerLength = ref writer.Length;
            _length = ref length;
            Writer = new NetlinkAttributeWriter<TNestedAttr>(writer);
        }

        public void Dispose() => _length = (ushort)(_writerLength - _writerStart + sizeof(NetlinkAttributeHeader<TAttr>));
    }

    public readonly ref struct NestedScope<TNestedAttr, TNestedHeader> : IDisposable
        where TNestedAttr : unmanaged, Enum
        where TNestedHeader : unmanaged
    {
        private readonly uint _writerStart;
        private readonly ref readonly uint _writerLength;
        private readonly ref ushort _length;
        private readonly ref TNestedHeader _header;

        public ref TNestedHeader Header => ref _header;

        public NetlinkAttributeWriter<TNestedAttr> Writer { get; }

        internal NestedScope(SpanWriter writer, ref ushort length, ref TNestedHeader header)
        {
            _writerStart = writer.Length;
            _writerLength = ref writer.Length;
            _length = ref length;
            _header = ref header;
            Writer = new NetlinkAttributeWriter<TNestedAttr>(writer);
        }

        public void Dispose() => _length = (ushort)(_writerLength - _writerStart + sizeof(NetlinkAttributeHeader<TAttr>) + sizeof(TNestedHeader));
    }
}