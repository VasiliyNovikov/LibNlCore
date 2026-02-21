namespace NetlinkCore.Protocol.Generic.EthTool;

internal enum EthToolBitsetAttributes : ushort
{
    Unspecified, // ETHTOOL_A_BITSET_UNSPEC
    NoMask,      // ETHTOOL_A_BITSET_NOMASK - flag
    Size,        // ETHTOOL_A_BITSET_SIZE   - u32
    Bits,        // ETHTOOL_A_BITSET_BITS   - nest
    Value,       // ETHTOOL_A_BITSET_VALUE  - binary
    Mask,        // ETHTOOL_A_BITSET_MASK   - binary
}