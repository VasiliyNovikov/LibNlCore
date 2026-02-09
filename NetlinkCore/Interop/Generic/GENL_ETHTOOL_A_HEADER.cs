namespace NetlinkCore.Interop.Generic;

internal enum GENL_ETHTOOL_A_HEADER : ushort
{
    ETHTOOL_A_HEADER_UNSPEC,
    ETHTOOL_A_HEADER_DEV_INDEX, /* u32 */
    ETHTOOL_A_HEADER_DEV_NAME,  /* string */
    ETHTOOL_A_HEADER_FLAGS,     /* u32 - ETHTOOL_FLAG_* */
}