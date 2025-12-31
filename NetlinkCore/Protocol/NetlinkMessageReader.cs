using NetlinkCore.Interop;

namespace NetlinkCore.Protocol;

internal unsafe ref struct NetlinkMessageReader(SpanReader reader)
{
    private SpanReader _reader = reader;

    public readonly bool IsEndOfBuffer => _reader.IsEndOfBuffer;

    public NetlinkMessage Read()
    {
        ref readonly var header = ref _reader.Read<nlmsghdr>();
        return new NetlinkMessage((NetlinkMessageFlags)header.nlmsg_flags, _reader.Read((int)header.nlmsg_len - sizeof(nlmsghdr)));
    }
}