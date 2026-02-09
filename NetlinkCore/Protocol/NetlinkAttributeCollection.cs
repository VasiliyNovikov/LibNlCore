using System;
using System.Runtime.CompilerServices;

using NetlinkCore.Interop;

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
            ref readonly var header = ref _reader.Read<nlattr>();
            var rawType = header.nla_type;
            var name = Unsafe.BitCast<ushort, TAttr>((ushort)(rawType & Constants.NLA_F_TYPE_MASK));
            var nested = (rawType & Constants.NLA_F_NESTED) != 0;
            var netByteOrder = (rawType & Constants.NLA_F_NET_BYTEORDER) != 0;
            Current = new NetlinkAttribute<TAttr>(name, nested, netByteOrder, _reader.Read(header.nla_len - sizeof(nlattr)));
            return true;
        }
    }
}