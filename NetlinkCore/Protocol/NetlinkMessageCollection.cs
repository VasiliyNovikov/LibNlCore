using System;

using NetlinkCore.Interop;

namespace NetlinkCore.Protocol;

internal readonly unsafe ref struct NetlinkMessageCollection(ReadOnlySpan<byte> messageBytes)
{
    private readonly ReadOnlySpan<byte> _messageBytes = messageBytes;

    public Enumerator GetEnumerator() => new(_messageBytes);

    public ref struct Enumerator
    {
        private SpanReader _reader;

        public NetlinkMessage Current { get; private set; }

        internal Enumerator(ReadOnlySpan<byte> messageBytes) => _reader = new SpanReader(messageBytes);

        public bool MoveNext()
        {
            if (_reader.IsEndOfBuffer)
                return false;

            ref readonly var header = ref _reader.Read<nlmsghdr>();
            var rawType = (NetlinkMessageType)header.nlmsg_type;
            var type = rawType & NetlinkMessageType.Mask;
            var subtype = (int)(rawType & ~NetlinkMessageType.Mask);
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