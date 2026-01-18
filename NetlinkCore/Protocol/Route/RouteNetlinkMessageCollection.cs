using System;
using System.Runtime.CompilerServices;

namespace NetlinkCore.Protocol.Route;

internal readonly ref struct RouteNetlinkMessageCollection<THeader, TAttr>(ReadOnlySpan<byte> messageBytes)
    where THeader : unmanaged
    where TAttr : unmanaged, Enum
{
    private readonly NetlinkMessageCollection _collection = new(messageBytes);

    public Enumerator GetEnumerator() => new(_collection);

    public ref struct Enumerator
    {
        private NetlinkMessageCollection.Enumerator _enumerator;

        public RouteNetlinkMessage<THeader, TAttr> Current { get; private set; }

        internal Enumerator(NetlinkMessageCollection collection) => _enumerator = collection.GetEnumerator();

        public bool MoveNext()
        {
            if (!_enumerator.MoveNext())
                return false;
            var current = _enumerator.Current;
            var reader = new SpanReader(current.Payload);
            ref readonly var header = ref reader.Read<THeader>();
            var attributes = new NetlinkAttributeCollection<TAttr>(reader.ReadToEnd());
            Current = new RouteNetlinkMessage<THeader, TAttr>(current.Flags, Unsafe.BitCast<int, RouteNetlinkMessageType>(current.SubType), in header, attributes);
            return true;
        }
    }
}