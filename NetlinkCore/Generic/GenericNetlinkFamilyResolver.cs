using System.Collections.Generic;
using System.Threading;

namespace NetlinkCore.Generic;

public static class GenericNetlinkFamilyResolver
{
    private static readonly Lock Lock = new();
    private static readonly ControlNetlinkSocket Socket = new();
    private static readonly Dictionary<string, GenericNetlinkFamily> Cache = [];

    public static GenericNetlinkFamily Resolve(string name)
    {
        lock (Lock)
        {
            if (!Cache.TryGetValue(name, out var family))
                Cache[name] = family = Socket.GetFamily(name);
            return family;
        }
    }
}