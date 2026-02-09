namespace NetlinkCore.Interop.Generic;

internal enum GENL_ETHTOOL_A_BITSET : ushort
{
    ETHTOOL_A_BITSET_UNSPEC,
    ETHTOOL_A_BITSET_NOMASK, /* flag */
    ETHTOOL_A_BITSET_SIZE,   /* u32 */
    ETHTOOL_A_BITSET_BITS,   /* nest - _A_BITSET_BITS_* */
    ETHTOOL_A_BITSET_VALUE,  /* binary */
    ETHTOOL_A_BITSET_MASK,   /* binary */
}