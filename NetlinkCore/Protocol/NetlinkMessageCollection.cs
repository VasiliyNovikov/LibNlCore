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
            Current = new((NetlinkMessageFlags)header.nlmsg_flags, _reader.Read((int)header.nlmsg_len - sizeof(nlmsghdr)));
            return true;
        }
    }
}