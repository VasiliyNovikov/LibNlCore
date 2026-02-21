namespace NetlinkCore.Protocol.Generic;

internal enum EthToolStringSetsAttributes : ushort
{
    Unspecified, // ETHTOOL_A_STRINGSETS_UNSPEC
    StringSet    // ETHTOOL_A_STRINGSETS_STRINGSET - nest EthToolStringSetAttributes
}