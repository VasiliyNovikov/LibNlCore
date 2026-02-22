namespace LibNlCore.Protocol.Route;

internal enum RouteLinkAttributes : ushort
{
    Unspecified,                                // IFLA_UNSPEC
    Address,                                    // IFLA_ADDRESS
    Broadcast,                                  // IFLA_BROADCAST
    Name,                                       // IFLA_IFNAME
    Mtu,                                        // IFLA_MTU
    Link,                                       // IFLA_LINK
    QueueDiscipline,                            // IFLA_QDISC
    Stats,                                      // IFLA_STATS
    Cost,                                       // IFLA_COST
    Priority,                                   // IFLA_PRIORITY
    Master,                                     // IFLA_MASTER
    Wireless,                                   // IFLA_WIRELESS
    ProtocolInfo,                               // IFLA_PROTINFO
    TxQueueLength,                              // IFLA_TXQLEN
    Map,                                        // IFLA_MAP
    Weight,                                     // IFLA_WEIGHT
    OperationalState,                           // IFLA_OPERSTATE
    LinkMode,                                   // IFLA_LINKMODE
    LinkInfo,                                   // IFLA_LINKINFO
    NetNsPid,                                   // IFLA_NET_NS_PID
    Alias,                                      // IFLA_IFALIAS
    NumVf,                                      // IFLA_NUM_VF
    VfInfoList,                                 // IFLA_VFINFO_LIST
    Stats64,                                    // IFLA_STATS64
    VfPorts,                                    // IFLA_VF_PORTS
    PortSelf,                                   // IFLA_PORT_SELF
    AfSpec,                                     // IFLA_AF_SPEC
    Group,                                      // IFLA_GROUP
    NetNsFd,                                    // IFLA_NET_NS_FD
    ExtMask,                                    // IFLA_EXT_MASK
    Promiscuity,                                // IFLA_PROMISCUITY
    NumTxQueues,                                // IFLA_NUM_TX_QUEUES
    NumRxQueues,                                // IFLA_NUM_RX_QUEUES
    Carrier,                                    // IFLA_CARRIER
    PhysPortId,                                 // IFLA_PHYS_PORT_ID
    CarrierChanges,                             // IFLA_CARRIER_CHANGES
    PhysSwitchId,                               // IFLA_PHYS_SWITCH_ID
    LinkNetNsId,                                // IFLA_LINK_NETNSID
    PhysPortName,                               // IFLA_PHYS_PORT_NAME
    ProtoDown,                                  // IFLA_PROTO_DOWN
    GsoMaxSegs,                                 // IFLA_GSO_MAX_SEGS
    GsoMaxSize,                                 // IFLA_GSO_MAX_SIZE
    Pad,                                        // IFLA_PAD
    Xdp,                                        // IFLA_XDP
    Event,                                      // IFLA_EVENT
    NewNetNsId,                                 // IFLA_NEW_NETNSID
    TargetNetNsId,                              // IFLA_IF_NETNSID / IFLA_TARGET_NETNSID
    CarrierUpCount,                             // IFLA_CARRIER_UP_COUNT
    CarrierDownCount,                           // IFLA_CARRIER_DOWN_COUNT
    NewIndex,                                   // IFLA_NEW_IFINDEX
    MinMtu,                                     // IFLA_MIN_MTU
    MaxMtu,                                     // IFLA_MAX_MTU
    PropList,                                   // IFLA_PROP_LIST
    AltName,                                    // IFLA_ALT_IFNAME
    PermAddress,                                // IFLA_PERM_ADDRESS
    ProtoDownReason,                            // IFLA_PROTO_DOWN_REASON
    ParentDevName,                              // IFLA_PARENT_DEV_NAME
    ParentDevBusName,                           // IFLA_PARENT_DEV_BUS_NAME
}