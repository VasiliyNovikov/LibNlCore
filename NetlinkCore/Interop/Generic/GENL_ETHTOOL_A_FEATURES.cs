namespace NetlinkCore.Interop.Generic;

internal enum GENL_ETHTOOL_A_FEATURES : ushort 
{
    ETHTOOL_A_FEATURES_UNSPEC,
    ETHTOOL_A_FEATURES_HEADER,   /* nest - _A_HEADER_* */
    ETHTOOL_A_FEATURES_HW,       /* bitset */
    ETHTOOL_A_FEATURES_WANTED,   /* bitset */
    ETHTOOL_A_FEATURES_ACTIVE,   /* bitset */
    ETHTOOL_A_FEATURES_NOCHANGE, /* bitset */
}