namespace NetlinkCore.Protocol.Route;

public enum VethInfoAttributes : ushort
{
    Unspecified, // VETH_INFO_UNSPEC
    Peer,        // VETH_INFO_PEER - nest RouteLinkAttributes
}