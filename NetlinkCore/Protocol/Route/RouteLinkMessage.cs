using System.Runtime.InteropServices;

using LinuxCore;

namespace NetlinkCore.Protocol.Route;

// struct ifinfomsg
[StructLayout(LayoutKind.Sequential)]
internal struct RouteLinkMessage
{
    private byte RawFamily;        // ifi_family
    private readonly byte Pad;    // __ifi_pad
    public ushort Type;            // ifi_type   - ARPHRD_*
    public int Index;              // ifi_index
    public NetDeviceFlags Flags;   // ifi_flags  - IFF_* flags
    public NetDeviceFlags Change;  // ifi_change - IFF_* change mask

    public LinuxAddressFamily Family
    {
        readonly get => (LinuxAddressFamily)RawFamily;
        set => RawFamily = (byte)value;
    }
}