using System;
using System.Runtime.CompilerServices;

namespace NetlinkCore.Protocol.Route;

internal readonly ref struct RouteNetlinkMessageCollection<THeader, TAttr>(NetlinkMessageCollection<THeader, TAttr> collection)
    where THeader : unmanaged
    where TAttr : unmanaged, Enum
{
    private readonly NetlinkMessageCollection<THeader, TAttr> _collection = collection;

    public Enumerator GetEnumerator() => new(_collection);

    public ref struct Enumerator
    {
        private NetlinkMessageCollection<THeader, TAttr>.Enumerator _enumerator;

        public RouteNetlinkMessage<THeader, TAttr> Current { get; private set; }

        internal Enumerator(NetlinkMessageCollection<THeader, TAttr> collection) => _enumerator = collection.GetEnumerator();

        public bool MoveNext()
        {
            if (!_enumerator.MoveNext())
                return false;
            var current = _enumerator.Current;
            Current = new RouteNetlinkMessage<THeader, TAttr>(current.Flags, Unsafe.BitCast<int, RouteNetlinkMessageType>(current.SubType), in current.Header, current.Attributes);
            return true;
        }
    }
}