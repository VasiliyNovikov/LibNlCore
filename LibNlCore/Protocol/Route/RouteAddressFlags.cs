using System;

namespace LibNlCore.Protocol.Route;

// ifa_flags
[Flags]
internal enum RouteAddressFlags : uint
{
    Secondary       = 0x001, // IFA_F_SECONDARY
    NoDad           = 0x002, // IFA_F_NODAD
    Optimistic      = 0x004, // IFA_F_OPTIMISTIC
    DadFailed       = 0x008, // IFA_F_DADFAILED
    HomeAddress     = 0x010, // IFA_F_HOMEADDRESS
    Deprecated      = 0x020, // IFA_F_DEPRECATED
    Tentative       = 0x040, // IFA_F_TENTATIVE
    Permanent       = 0x080, // IFA_F_PERMANENT
    ManageTempAddr  = 0x100, // IFA_F_MANAGETEMPADDR
    NoPrefixRoute   = 0x200, // IFA_F_NOPREFIXROUTE
    McAutoJoin      = 0x400, // IFA_F_MCAUTOJOIN
    StablePrivacy   = 0x800  // IFA_F_STABLE_PRIVACY
}