using System.Runtime.InteropServices;

namespace NetlinkCore.Interop.Route;

[StructLayout(LayoutKind.Sequential)]
internal struct rtgenmsg
{
    public ushort rtgen_family;
}