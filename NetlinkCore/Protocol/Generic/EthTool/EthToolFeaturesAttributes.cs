namespace NetlinkCore.Protocol.Generic.EthTool;

internal enum EthToolFeaturesAttributes : ushort
{
    Unspecified, // ETHTOOL_A_FEATURES_UNSPEC
    Header,      // ETHTOOL_A_FEATURES_HEADER   - nest EthToolHeaderAttributes
    Supported,   // ETHTOOL_A_FEATURES_HW       - bitset
    Wanted,      // ETHTOOL_A_FEATURES_WANTED   - bitset
    Active,      // ETHTOOL_A_FEATURES_ACTIVE   - bitset
    NoChange,    // ETHTOOL_A_FEATURES_NOCHANGE - bitset
}