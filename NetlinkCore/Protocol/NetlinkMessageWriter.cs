using System;

using NetlinkCore.Interop;

namespace NetlinkCore.Protocol;

internal readonly ref struct NetlinkMessageWriter
{
    private readonly ref nlmsghdr _header;

    public int SubType
    {
        get => (int)((NetlinkMessageType)_header.nlmsg_type & ~NetlinkMessageType.Mask);
        set => _header.nlmsg_type = (ushort)((NetlinkMessageType)value | ((NetlinkMessageType)_header.nlmsg_type & NetlinkMessageType.Mask));
    }

    public NetlinkMessageFlags Flags
    {
        get => (NetlinkMessageFlags)_header.nlmsg_flags;
        set => _header.nlmsg_flags = (ushort)value;
    }

    public ref uint PortId => ref _header.nlmsg_pid;

    public SpanWriter PayloadWriter { get; }

    public NetlinkMessageWriter(Span<byte> buffer)
    {
        var tmpWriter = new SpanWriter(buffer);
        _header = ref tmpWriter.Skip<nlmsghdr>();
        _header.nlmsg_len = 0;
        _header.nlmsg_seq = 0;
        _header.nlmsg_type = 0;
        _header.nlmsg_flags = 0;
        _header.nlmsg_pid = 0;
        PayloadWriter = new SpanWriter(buffer, ref _header.nlmsg_len);
        PayloadWriter.Skip<nlmsghdr>();
    }
}