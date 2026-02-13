namespace NetlinkCore.Interop.Generic;

internal enum GENL_ETHTOOL_A_STRING : ushort
{
    ETHTOOL_A_STRING_UNSPEC,
    ETHTOOL_A_STRING_INDEX, /* u32 */
    ETHTOOL_A_STRING_VALUE  /* string */
}