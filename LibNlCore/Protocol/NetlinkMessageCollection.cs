using System;

namespace LibNlCore.Protocol;

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

            ref readonly var header = ref _reader.Read<NetlinkMessageHeader>();
            var kind = header.Kind;
            var type = header.Type;
            var payload = _reader.Read((int)header.Length - sizeof(NetlinkMessageHeader));

            if (kind == NetlinkMessageKind.Error)
            {
                var errorReader = new SpanReader(payload);
                var error = errorReader.Read<NetlinkErrorMessageHeader>().Error;
                if (error == 0)
                    return false;
                var errorAttrs = new NetlinkAttributeCollection<NetlinkErrorMessageAttributes>(errorReader.ReadToEnd());
                string? message = null;
                foreach (var errorAttr in errorAttrs)
                    if (errorAttr.Name == NetlinkErrorMessageAttributes.Message)
                    {
                        message = errorAttr.AsString();
                        break;
                    }
                throw new NetlinkException(error, message);
            }

            if (kind == NetlinkMessageKind.Done)
                return false;

            Current = new(type, payload);
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
            Current = new NetlinkMessage<THeader, TAttr>(current.Type, in header, attributes);
            return true;
        }
    }
}