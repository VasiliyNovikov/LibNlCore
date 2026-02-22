using System;

namespace LibNlCore.Protocol;

internal readonly unsafe ref struct NetlinkAttributeCollection<TAttr>(ReadOnlySpan<byte> attributeBytes)
    where TAttr : unmanaged, Enum
{
    private readonly ReadOnlySpan<byte> _attributeBytes = attributeBytes;

    public Enumerator GetEnumerator() => new(_attributeBytes);

    public ref struct Enumerator
    {
        private SpanReader _reader;

        public NetlinkAttribute<TAttr> Current { get; private set; }

        internal Enumerator(ReadOnlySpan<byte> attributeBytes) => _reader = new SpanReader(attributeBytes);

        public bool MoveNext()
        {
            if (_reader.IsEndOfBuffer)
                return false;
            ref readonly var header = ref _reader.Read<NetlinkAttributeHeader<TAttr>>();
            var name = header.Name;
            var nested = header.IsNested;
            var netByteOrder = header.IsNetworkByteOrder;
            Current = new NetlinkAttribute<TAttr>(name, nested, netByteOrder, _reader.Read(header.Length - sizeof(NetlinkAttributeHeader<TAttr>)));
            return true;
        }
    }
}