using System;
using System.Runtime.CompilerServices;

using NetlinkCore.Interop.Route;

namespace NetlinkCore.Protocol;

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
            ref readonly var header = ref _reader.Read<rtattr>();
            Current = new NetlinkAttribute<TAttr>(Unsafe.BitCast<ushort, TAttr>(header.rta_type), _reader.Read(header.rta_len - sizeof(rtattr)));
            return true;
        }
    }
}