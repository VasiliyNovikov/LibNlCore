using System.Runtime.InteropServices;

namespace NetlinkCore.Interop.Route;

[StructLayout(LayoutKind.Sequential)]
internal struct ifaddrmsg
{
    public byte ifa_family;
    public byte ifa_prefixlen;   /* The prefix length */
    public byte ifa_flags;       /* Flags             */
    public rt_scope_t ifa_scope; /* Address scope     */
    public uint ifa_index;       /* Link index        */
}