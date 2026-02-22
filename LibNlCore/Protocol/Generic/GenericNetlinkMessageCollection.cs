using System;

namespace LibNlCore.Protocol.Generic;

internal readonly ref struct GenericNetlinkMessageCollection<TCmd, TAttr>(NetlinkMessageCollection<GenericNetlinkMessageHeader, TAttr> collection)
    where TCmd : unmanaged, Enum
    where TAttr : unmanaged, Enum
{
    private readonly NetlinkMessageCollection<GenericNetlinkMessageHeader, TAttr> _collection = collection;

    public Enumerator GetEnumerator() => new(_collection);

    public ref struct Enumerator
    {
        private NetlinkMessageCollection<GenericNetlinkMessageHeader, TAttr>.Enumerator _enumerator;

        public GenericNetlinkMessage<TCmd, TAttr> Current { get; private set; }

        internal Enumerator(NetlinkMessageCollection<GenericNetlinkMessageHeader, TAttr> collection) => _enumerator = collection.GetEnumerator();

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