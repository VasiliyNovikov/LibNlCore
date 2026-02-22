namespace LibNlCore.Protocol.Route;

internal enum RouteLinkInfoAttributes : ushort
{
    Unspecified, // IFLA_INFO_UNSPEC
    Kind,        // IFLA_INFO_KIND
    Data,        // IFLA_INFO_DATA
    XStats,      // IFLA_INFO_XSTATS
    SlaveKind,   // IFLA_INFO_SLAVE_KIND
    SlaveData,   // IFLA_INFO_SLAVE_DATA
}