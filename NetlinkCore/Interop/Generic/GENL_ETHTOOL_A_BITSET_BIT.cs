namespace NetlinkCore.Interop.Generic;

internal enum GENL_ETHTOOL_A_BITSET_BIT : ushort
{
    ETHTOOL_A_BITSET_BIT_UNSPEC,
    ETHTOOL_A_BITSET_BIT_INDEX,  /* u32 */
    ETHTOOL_A_BITSET_BIT_NAME,   /* string */
    ETHTOOL_A_BITSET_BIT_VALUE,  /* flag */
}