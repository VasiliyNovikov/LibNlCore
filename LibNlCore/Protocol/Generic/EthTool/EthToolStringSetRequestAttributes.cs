namespace LibNlCore.Protocol.Generic.EthTool;

internal enum EthToolStringSetRequestAttributes : ushort
{
    Unspecified, // ETHTOOL_A_STRSET_UNSPEC
    Header,      // ETHTOOL_A_STRSET_HEADER      - nest EthToolHeaderAttributes
    StringSets,  // ETHTOOL_A_STRSET_STRINGSETS  - nest EthToolStringSetsAttributes
    CountsOnly   // ETHTOOL_A_STRSET_COUNTS_ONLY - flag
}