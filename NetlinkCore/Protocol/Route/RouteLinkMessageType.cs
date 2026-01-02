using NetlinkCore.Interop.Route;

namespace NetlinkCore.Protocol.Route;

public enum RouteLinkMessageType
{
    NewLink = ifinfomsg_type.RTM_NEWLINK,
    DeleteLink = ifinfomsg_type.RTM_DELLINK,
    GetLink = ifinfomsg_type.RTM_GETLINK,
    SetLink = ifinfomsg_type.RTM_SETLINK
}