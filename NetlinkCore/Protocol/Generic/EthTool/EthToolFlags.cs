using System;

namespace NetlinkCore.Protocol.Generic.EthTool;

[Flags]
internal enum EthToolFlags
{
    CompactBitsets = 1 << 0, // ETHTOOL_FLAG_COMPACT_BITSETS
    OmitReply      = 1 << 1, // ETHTOOL_FLAG_OMIT_REPLY
    Stats          = 1 << 2  // ETHTOOL_FLAG_STATS
}