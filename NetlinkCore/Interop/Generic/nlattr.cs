using System.Runtime.InteropServices;

namespace NetlinkCore.Interop.Generic;

[StructLayout(LayoutKind.Sequential)]
internal struct nlattr
{
    public ushort nla_len;
    public ushort nla_type;
}