namespace NetlinkCore.Protocol.Route;

internal enum RouteAddressAttributes : ushort
{
    Unspecified,    // IFA_UNSPEC
    Address,        // IFA_ADDRESS        - binary
    Local,          // IFA_LOCAL          - binary
    Label,          // IFA_LABEL          - string
    Broadcast,      // IFA_BROADCAST      - binary
    Anycast,        // IFA_ANYCAST        - binary
    CacheInfo,      // IFA_CACHEINFO      - struct
    Multicast,      // IFA_MULTICAST      - binary
    Flags,          // IFA_FLAGS          - u32
    RoutePriority,  // IFA_RT_PRIORITY    - u32, priority/metric for prefix route
    TargetNetNsId,  // IFA_TARGET_NETNSID - u32, target netns id
    Proto           // IFA_PROTO          - u8, address protocol
}