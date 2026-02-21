using System.Runtime.InteropServices;

namespace NetlinkCore.Protocol.Generic;

// struct genlmsghdr
[StructLayout(LayoutKind.Sequential)]
internal struct GenericNetlinkMessageHeader
{
    public byte Cmd;      // cmd
    public byte Version;  // version
    private ushort Reserved;
}