using System;

using NetlinkCore.Interop;

namespace NetlinkCore.Protocol;

internal readonly unsafe ref struct NetlinkMessageCollection(NetlinkSocket socket, Span<byte> buffer)
{
    private readonly Span<byte> _buffer = buffer;

    public Enumerator GetEnumerator() => new(socket, _buffer);

    public ref struct Enumerator(NetlinkSocket socket, Span<byte> buffer)
    {
        private readonly Span<byte> _buffer = buffer;
        private SpanReader _reader = default;

        public NetlinkMessage Current { get; private set; }

        public bool MoveNext()
        {
            if (_reader.IsEndOfBuffer)
                _reader = new SpanReader(_buffer[..socket.Receive(_buffer)]);

            ref readonly var header = ref _reader.Read<nlmsghdr>();
            var rawType = (NetlinkMessageType)header.nlmsg_type;
            var type = rawType & NetlinkMessageType.Mask;
            var subtype = (int)rawType;
            var flags = (NetlinkMessageFlags)header.nlmsg_flags;
            var payload = _reader.Read((int)header.nlmsg_len - sizeof(nlmsghdr));

            if (type == NetlinkMessageType.Error)
            {
                var errorReader = new SpanReader(payload);
                var error = errorReader.Read<nlmsgerr>().error;
                if (error == 0)
                    return false;
                var errorAttrs = new NetlinkAttributeCollection<NLMSGERR_ATTRS>(errorReader.ReadToEnd());
                string? message = null;
                foreach (var errorAttr in errorAttrs)
                {
                    if (errorAttr.Name != NLMSGERR_ATTRS.NLMSGERR_ATTR_MSG)
                        continue;
                    message = errorAttr.AsString();
                    break;
                }
                throw new NetlinkException(error, message);
            }

            if (type == NetlinkMessageType.Done)
                return false;

            Current = new(type, subtype, flags, payload);
            return true;
        }
    }
}

internal readonly ref struct NetlinkMessageCollection<THeader, TAttr>(NetlinkSocket socket, Span<byte> buffer)
    where THeader : unmanaged
    where TAttr : unmanaged, Enum
{
    private readonly NetlinkMessageCollection _collection = new(socket, buffer);

    public Enumerator GetEnumerator() => new(_collection);

    public ref struct Enumerator
    {
        private NetlinkMessageCollection.Enumerator _enumerator;

        public NetlinkMessage<THeader, TAttr> Current { get; private set; }

        internal Enumerator(NetlinkMessageCollection collection) => _enumerator = collection.GetEnumerator();

        public bool MoveNext()
        {
            if (!_enumerator.MoveNext())
                return false;
            var current = _enumerator.Current;
            var reader = new SpanReader(current.Payload);
            ref readonly var header = ref reader.Read<THeader>();
            var attributes = new NetlinkAttributeCollection<TAttr>(reader.ReadToEnd());
            Current = new NetlinkMessage<THeader, TAttr>(current.Flags, current.SubType, in header, attributes);
            return true;
        }
    }
}