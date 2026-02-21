namespace NetlinkCore.Protocol.Generic.EthTool;

internal enum EthToolStringAttributes : ushort
{
    Unspecified, // ETHTOOL_A_STRING_UNSPEC
    Index,       // ETHTOOL_A_STRING_INDEX - u32
    Value        // ETHTOOL_A_STRING_VALUE - string
}