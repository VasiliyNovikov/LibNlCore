namespace NetlinkCore.Protocol.Generic;

internal enum EthToolHeaderAttributes : ushort
{
    Unspecified, // ETHTOOL_A_HEADER_UNSPEC
    LinkIndex,   // ETHTOOL_A_HEADER_DEV_INDEX - u32
    LinkName,    // ETHTOOL_A_HEADER_DEV_NAME  - string
    Flags,       // ETHTOOL_A_HEADER_FLAGS     - u32, EthToolFlags
}