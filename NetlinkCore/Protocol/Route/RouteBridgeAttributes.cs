namespace NetlinkCore.Protocol.Route;

public enum RouteBridgeAttributes : ushort
{
    Unspecified,                  // IFLA_BR_UNSPEC
    ForwardDelay,                 // IFLA_BR_FORWARD_DELAY
    HelloTime,                    // IFLA_BR_HELLO_TIME
    MaxAge,                       // IFLA_BR_MAX_AGE
    AgeingTime,                   // IFLA_BR_AGEING_TIME
    StpState,                     // IFLA_BR_STP_STATE
    Priority,                     // IFLA_BR_PRIORITY
    VlanFiltering,                // IFLA_BR_VLAN_FILTERING
    VlanProtocol,                 // IFLA_BR_VLAN_PROTOCOL
    GroupForwardMask,             // IFLA_BR_GROUP_FWD_MASK
    RootId,                       // IFLA_BR_ROOT_ID
    BridgeId,                     // IFLA_BR_BRIDGE_ID
    RootPort,                     // IFLA_BR_ROOT_PORT
    RootPathCost,                 // IFLA_BR_ROOT_PATH_COST
    TopologyChange,               // IFLA_BR_TOPOLOGY_CHANGE
    TopologyChangeDetected,       // IFLA_BR_TOPOLOGY_CHANGE_DETECTED
    HelloTimer,                   // IFLA_BR_HELLO_TIMER
    TcnTimer,                     // IFLA_BR_TCN_TIMER
    TopologyChangeTimer,          // IFLA_BR_TOPOLOGY_CHANGE_TIMER
    GcTimer,                      // IFLA_BR_GC_TIMER
    GroupAddr,                    // IFLA_BR_GROUP_ADDR
    FdbFlush,                     // IFLA_BR_FDB_FLUSH
    McastRouter,                  // IFLA_BR_MCAST_ROUTER
    McastSnooping,                // IFLA_BR_MCAST_SNOOPING
    McastQueryUseIfAddr,          // IFLA_BR_MCAST_QUERY_USE_IFADDR
    McastQuerier,                 // IFLA_BR_MCAST_QUERIER
    McastHashElasticity,          // IFLA_BR_MCAST_HASH_ELASTICITY
    McastHashMax,                 // IFLA_BR_MCAST_HASH_MAX
    McastLastMemberCount,         // IFLA_BR_MCAST_LAST_MEMBER_CNT
    McastStartupQueryCount,       // IFLA_BR_MCAST_STARTUP_QUERY_CNT
    McastLastMemberInterval,      // IFLA_BR_MCAST_LAST_MEMBER_INTVL
    McastMembershipInterval,      // IFLA_BR_MCAST_MEMBERSHIP_INTVL
    McastQuerierInterval,         // IFLA_BR_MCAST_QUERIER_INTVL
    McastQueryInterval,           // IFLA_BR_MCAST_QUERY_INTVL
    McastQueryResponseInterval,   // IFLA_BR_MCAST_QUERY_RESPONSE_INTVL
    McastStartupQueryInterval,    // IFLA_BR_MCAST_STARTUP_QUERY_INTVL
    NfCallIpTables,               // IFLA_BR_NF_CALL_IPTABLES
    NfCallIp6Tables,              // IFLA_BR_NF_CALL_IP6TABLES
    NfCallArpTables,              // IFLA_BR_NF_CALL_ARPTABLES
    VlanDefaultPvid,              // IFLA_BR_VLAN_DEFAULT_PVID
    Pad,                          // IFLA_BR_PAD
    VlanStatsEnabled,             // IFLA_BR_VLAN_STATS_ENABLED
    McastStatsEnabled,            // IFLA_BR_MCAST_STATS_ENABLED
    McastIgmpVersion,             // IFLA_BR_MCAST_IGMP_VERSION
    McastMldVersion,              // IFLA_BR_MCAST_MLD_VERSION
    VlanStatsPerPort,             // IFLA_BR_VLAN_STATS_PER_PORT
    MultiBoolOpt,                 // IFLA_BR_MULTI_BOOLOPT
    McastQuerierState,            // IFLA_BR_MCAST_QUERIER_STATE
    FdbLearnedCount,              // IFLA_BR_FDB_N_LEARNED
    FdbMaxLearned                 // IFLA_BR_FDB_MAX_LEARNED
}