namespace NetlinkCore.Protocol.Generic.EthTool;

// enum ethtool_stringset
internal enum EthToolStringSet
{
    Test = 0,            // ETH_SS_TEST
    Stats,               // ETH_SS_STATS
    PrivFlags,           // ETH_SS_PRIV_FLAGS
    NTupleFilters,       // ETH_SS_NTUPLE_FILTERS
    Features,            // ETH_SS_FEATURES
    RssHashFuncs,        // ETH_SS_RSS_HASH_FUNCS
    Tunables,            // ETH_SS_TUNABLES
    PhyStats,            // ETH_SS_PHY_STATS
    PhyTunables,         // ETH_SS_PHY_TUNABLES
    LinkModes,           // ETH_SS_LINK_MODES
    MsgClasses,          // ETH_SS_MSG_CLASSES
    WolModes,            // ETH_SS_WOL_MODES
    SofTimestamping,     // ETH_SS_SOF_TIMESTAMPING
    TsTxTypes,           // ETH_SS_TS_TX_TYPES
    TsRxFilters,         // ETH_SS_TS_RX_FILTERS
    UdpTunnelTypes,      // ETH_SS_UDP_TUNNEL_TYPES
    StatsStd,            // ETH_SS_STATS_STD
    StatsEthPhy,         // ETH_SS_STATS_ETH_PHY
    StatsEthMac,         // ETH_SS_STATS_ETH_MAC
    StatsEthCtrl,        // ETH_SS_STATS_ETH_CTRL
    StatsRmon            // ETH_SS_STATS_RMON
}