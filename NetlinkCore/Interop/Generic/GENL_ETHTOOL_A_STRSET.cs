namespace NetlinkCore.Interop.Generic;

internal enum GENL_ETHTOOL_A_STRSET : ushort
{
    ETHTOOL_A_STRSET_UNSPEC,
    ETHTOOL_A_STRSET_HEADER,     /* nest - _A_HEADER_* */
    ETHTOOL_A_STRSET_STRINGSETS, /* nest - _A_STRINGSETS_* */
    ETHTOOL_A_STRSET_COUNTS_ONLY /* flag */
}