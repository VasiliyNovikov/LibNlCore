using System.Runtime.InteropServices;

namespace NetlinkCore.Interop.Generic;

[StructLayout(LayoutKind.Sequential)]
internal struct genlmsghdr
{
    public byte cmd;
    public byte version;
    private ushort reserved;
}