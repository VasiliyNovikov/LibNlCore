using System;

namespace NetlinkCore.Generic;

[Flags]
public enum LinkFeatureStatus
{
    None = 0,
    Supported = 1,
    Wanted    = 2,
    Active    = 4,
    NoChange  = 8
}