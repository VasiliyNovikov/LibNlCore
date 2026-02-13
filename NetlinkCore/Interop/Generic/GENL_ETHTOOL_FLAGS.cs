using System;

namespace NetlinkCore.Interop.Generic;

[Flags]
internal enum GENL_ETHTOOL_FLAGS
{
    ETHTOOL_FLAG_COMPACT_BITSETS = 1 << 0, /* use compact bitsets in reply */
    ETHTOOL_FLAG_OMIT_REPLY	     = 1 << 1, /* provide optional reply for SET or ACT requests */
    ETHTOOL_FLAG_STATS           = 1 << 2  /* request statistics, if supported by the driver */
}