using System;

using NetlinkCore.Interop.Generic;

namespace NetlinkCore.Protocol.Generic;

internal readonly ref struct GenericNetlinkMessageCollection<TCmd, TAttr>(NetlinkMessageCollection<genlmsghdr, TAttr> collection)
    where TCmd : unmanaged, Enum
    where TAttr : unmanaged, Enum
{
    private readonly NetlinkMessageCollection<genlmsghdr, TAttr> _collection = collection;

    public Enumerator GetEnumerator() => new(_collection);

    public ref struct Enumerator
    {
        private NetlinkMessageCollection<genlmsghdr, TAttr>.Enumerator _enumerator;

        public GenericNetlinkMessage<TCmd, TAttr> Current { get; private set; }

        internal Enumerator(NetlinkMessageCollection<genlmsghdr, TAttr> collection) => _enumerator = collection.GetEnumerator();

        public bool MoveNext()
        {
            if (!_enumerator.MoveNext())
                return false;
            var current = _enumerator.Current;
            Current = new GenericNetlinkMessage<TCmd, TAttr>(in current.Header, current.Attributes);
            return true;
        }
    }
}