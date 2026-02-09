using System.Collections.Generic;

namespace NetlinkCore.Generic;

public class LinkFeatures
{
    public Dictionary<string, bool> Supported { get; } = [];
    public Dictionary<string, bool> Wanted { get; } = [];
    public Dictionary<string, bool> Active { get; } = [];
    public Dictionary<string, bool> NoChange { get; } = [];
}