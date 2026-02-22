namespace LibNlCore.Protocol.Generic.EthTool;

internal enum EthToolStringSetAttributes : ushort
{
    Unspecified, // ETHTOOL_A_STRINGSET_UNSPEC
    Id,          // ETHTOOL_A_STRINGSET_ID      - u32
    Count,       // ETHTOOL_A_STRINGSET_COUNT   - u32
    Strings      // ETHTOOL_A_STRINGSET_STRINGS - nest EthToolStringsAttributes
}