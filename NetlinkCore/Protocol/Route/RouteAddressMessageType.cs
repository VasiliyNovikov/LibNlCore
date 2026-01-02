using NetlinkCore.Interop.Route;

namespace NetlinkCore.Protocol.Route;

public enum RouteAddressMessageType
{
    NewAddress = ifaddrmsg_type.RTM_NEWADDR,
    DeleteAddress = ifaddrmsg_type.RTM_DELADDR,
    GetAddress = ifaddrmsg_type.RTM_GETADDR
}