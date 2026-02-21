using System.Runtime.InteropServices;

using LinuxCore;

namespace NetlinkCore.Protocol.Route;

// struct ifaddrmsg
[StructLayout(LayoutKind.Sequential)]
internal struct RouteAddressMessage
{
    private byte RawFamily;         // ifa_family
    public byte PrefixLength;       // ifa_prefixlen
    private byte RawFlags;          // ifa_flags
    public RouteAddressScope Scope; // ifa_scope
    public uint LinkIndex;          // ifa_index

    public LinuxAddressFamily Family
    {
        readonly get => (LinuxAddressFamily)RawFamily;
        set => RawFamily = (byte)value;
    }

    public RouteAddressFlags Flags
    {
        readonly get => (RouteAddressFlags)RawFlags;
        set => RawFlags = (byte)value;
    }
}