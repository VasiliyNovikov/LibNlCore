using System.Runtime.InteropServices;

using LinuxCore;

namespace NetlinkCore.Interop.Route;

// struct ifinfomsg
[StructLayout(LayoutKind.Sequential)]
internal struct RouteLinkMessage
{
    private byte RawFamily; // ifi_family
    public byte __pad;      // __ifi_pad
    public ushort ifi_type; // ifi_type   - ARPHRD_*
    public int ifi_index;   // ifi_index
    public net_device_flags ifi_flags;  // ifi_flags  - IFF_* flags
    public net_device_flags ifi_change; // ifi_change - IFF_* change mask

    public LinuxAddressFamily Family
    {
        readonly get => (LinuxAddressFamily)RawFamily;
        set => RawFamily = (byte)value;
    }
}