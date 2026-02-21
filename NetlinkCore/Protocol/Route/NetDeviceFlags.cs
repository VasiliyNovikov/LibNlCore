using System;

namespace NetlinkCore.Protocol.Route;

[Flags]
internal enum NetDeviceFlags : uint
{
    Up          = 1 << 0,  // IFF_UP
    Broadcast   = 1 << 1,  // IFF_BROADCAST
    Debug       = 1 << 2,  // IFF_DEBUG
    Loopback    = 1 << 3,  // IFF_LOOPBACK
    PointToPoint = 1 << 4, // IFF_POINTOPOINT
    NoTrailers  = 1 << 5,  // IFF_NOTRAILERS
    Running     = 1 << 6,  // IFF_RUNNING
    NoArp       = 1 << 7,  // IFF_NOARP
    Promisc     = 1 << 8,  // IFF_PROMISC
    AllMulti    = 1 << 9,  // IFF_ALLMULTI
    Master      = 1 << 10, // IFF_MASTER
    Slave       = 1 << 11, // IFF_SLAVE
    Multicast   = 1 << 12, // IFF_MULTICAST
    PortSel     = 1 << 13, // IFF_PORTSEL
    AutoMedia   = 1 << 14, // IFF_AUTOMEDIA
    Dynamic     = 1 << 15, // IFF_DYNAMIC
    LowerUp     = 1 << 16, // IFF_LOWER_UP
    Dormant     = 1 << 17, // IFF_DORMANT
    Echo        = 1 << 18  // IFF_ECHO
}