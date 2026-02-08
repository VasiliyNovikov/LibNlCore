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