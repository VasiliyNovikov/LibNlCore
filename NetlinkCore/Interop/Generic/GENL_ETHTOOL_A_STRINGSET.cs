namespace NetlinkCore.Interop.Generic;

internal enum GENL_ETHTOOL_A_STRINGSET : ushort
{
    ETHTOOL_A_STRINGSET_UNSPEC,
    ETHTOOL_A_STRINGSET_ID,     /* u32 */
    ETHTOOL_A_STRINGSET_COUNT,  /* u32 */
    ETHTOOL_A_STRINGSET_STRINGS /* nest - _A_STRINGS_* */
}