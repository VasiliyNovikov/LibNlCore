using System.Runtime.InteropServices;

namespace LibNlCore.Protocol;

[StructLayout(LayoutKind.Sequential)]
internal struct NetlinkSocketAddress
{
    public ushort Family;        // nl_family - AF_NETLINK
    private readonly ushort Pad; // nl_pad    - zero
    public uint PortId;          // nl_pid    - port ID
    public uint Groups;          // nl_groups - multicast groups mask
}