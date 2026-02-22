namespace LibNlCore.Protocol.Generic.EthTool;

public enum EthToolCommand : byte
{
    None,               // ETHTOOL_MSG_NONE
    StringSetGet,       // ETHTOOL_MSG_STRSET_GET
    LinkInfoGet,        // ETHTOOL_MSG_LINKINFO_GET
    LinkInfoSet,        // ETHTOOL_MSG_LINKINFO_SET
    LinkModesGet,       // ETHTOOL_MSG_LINKMODES_GET
    LinkModesSet,       // ETHTOOL_MSG_LINKMODES_SET
    LinkStateGet,       // ETHTOOL_MSG_LINKSTATE_GET
    DebugGet,           // ETHTOOL_MSG_DEBUG_GET
    DebugSet,           // ETHTOOL_MSG_DEBUG_SET
    WolGet,             // ETHTOOL_MSG_WOL_GET
    WolSet,             // ETHTOOL_MSG_WOL_SET
    FeaturesGet,        // ETHTOOL_MSG_FEATURES_GET
    FeaturesSet,        // ETHTOOL_MSG_FEATURES_SET
    PrivFlagsGet,       // ETHTOOL_MSG_PRIVFLAGS_GET
    PrivFlagsSet,       // ETHTOOL_MSG_PRIVFLAGS_SET
    RingsGet,           // ETHTOOL_MSG_RINGS_GET
    RingsSet,           // ETHTOOL_MSG_RINGS_SET
    ChannelsGet,        // ETHTOOL_MSG_CHANNELS_GET
    ChannelsSet,        // ETHTOOL_MSG_CHANNELS_SET
    CoalesceGet,        // ETHTOOL_MSG_COALESCE_GET
    CoalesceSet,        // ETHTOOL_MSG_COALESCE_SET
    PauseGet,           // ETHTOOL_MSG_PAUSE_GET
    PauseSet,           // ETHTOOL_MSG_PAUSE_SET
    EeeGet,             // ETHTOOL_MSG_EEE_GET
    EeeSet,             // ETHTOOL_MSG_EEE_SET
    TsInfoGet,          // ETHTOOL_MSG_TSINFO_GET
    CableTestAct,       // ETHTOOL_MSG_CABLE_TEST_ACT
    CableTestTdrAct,    // ETHTOOL_MSG_CABLE_TEST_TDR_ACT
    TunnelInfoGet,      // ETHTOOL_MSG_TUNNEL_INFO_GET
    FecGet,             // ETHTOOL_MSG_FEC_GET
    FecSet,             // ETHTOOL_MSG_FEC_SET
    ModuleEepromGet,    // ETHTOOL_MSG_MODULE_EEPROM_GET
    StatsGet,           // ETHTOOL_MSG_STATS_GET
    PhcVclocksGet,      // ETHTOOL_MSG_PHC_VCLOCKS_GET
}