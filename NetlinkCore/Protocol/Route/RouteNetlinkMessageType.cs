using System.Diagnostics.CodeAnalysis;

namespace NetlinkCore.Protocol.Route;

[SuppressMessage("Style", "IDE0055:Fix formatting")]
public enum RouteNetlinkMessageType
{
    NewLink       = 16, // RTM_NEWLINK
    DeleteLink    = 17, // RTM_DELLINK
    GetLink       = 18, // RTM_GETLINK
    SetLink       = 19, // RTM_SETLINK

    NewAddress    = 20, // RTM_NEWADDR
    DeleteAddress = 21, // RTM_DELADDR
    GetAddress    = 22, // RTM_GETADDR

    NewNsId       = 88, // RTM_NEWNSID
    DeleteNsId    = 89, // RTM_DELNSID
    GetNsId       = 90, // RTM_GETNSID
}