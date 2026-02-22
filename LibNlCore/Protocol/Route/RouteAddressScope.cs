namespace LibNlCore.Protocol.Route;

// enum rt_scope_t
internal enum RouteAddressScope : byte
{
    Universe =   0, // RT_SCOPE_UNIVERSE
                    // User defined values
    Site     = 200, // RT_SCOPE_SITE
    Link     = 253, // RT_SCOPE_LINK
    Host     = 254, // RT_SCOPE_HOST
    NoWhere  = 255  // RT_SCOPE_NOWHERE
}