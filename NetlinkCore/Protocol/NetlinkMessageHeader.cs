using System.Runtime.InteropServices;

namespace NetlinkCore.Protocol;

// struct nlmsghdr
[StructLayout(LayoutKind.Sequential)]
internal struct NetlinkMessageHeader
{
    public uint Length;               // nlmsg_len   - Length of message including headers
    public ushort Type;               // nlmsg_type  - Netlink Family (subsystem) ID
    public NetlinkMessageFlags Flags; // nlmsg_flags - Flags - request or dump
    public uint SequenceNumber;       // nlmsg_seq   - Sequence number
    public uint PortId;               // nlmsg_pid   - Port ID, set to 0
}