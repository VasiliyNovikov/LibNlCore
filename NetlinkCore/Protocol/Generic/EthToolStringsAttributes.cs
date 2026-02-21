namespace NetlinkCore.Protocol.Generic;

internal enum EthToolStringsAttributes : ushort
{
    Unspecified, // ETHTOOL_A_STRINGS_UNSPEC
    String       // ETHTOOL_A_STRINGS_STRING - nest EthToolStringAttributes
}