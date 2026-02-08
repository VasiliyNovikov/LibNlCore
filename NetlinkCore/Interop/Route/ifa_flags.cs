using System;

namespace NetlinkCore.Interop.Route;

[Flags]
internal enum ifa_flags : uint
{
    IFA_F_SECONDARY       = 0x001,
    IFA_F_NODAD           = 0x002,
    IFA_F_OPTIMISTIC      = 0x004,
    IFA_F_DADFAILED       = 0x008,
    IFA_F_HOMEADDRESS     = 0x010,
    IFA_F_DEPRECATED      = 0x020,
    IFA_F_TENTATIVE       = 0x040,
    IFA_F_PERMANENT       = 0x080,
    IFA_F_MANAGETEMPADDR  = 0x100,
    IFA_F_NOPREFIXROUTE   = 0x200,
    IFA_F_MCAUTOJOIN      = 0x400,
    IFA_F_STABLE_PRIVACY  = 0x800
}