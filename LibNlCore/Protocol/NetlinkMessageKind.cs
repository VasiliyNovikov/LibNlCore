using System;

namespace LibNlCore.Protocol;

[Flags]
public enum NetlinkMessageKind : ushort
{
    None,
    NoOp    = 0x1, // NLMSG_NOOP    - Nothing
    Error   = 0x2, // NLMSG_ERROR   - Error
    Done    = 0x3, // NLMSG_DONE    - End of a dump
    Overrun = 0x4, // NLMSG_OVERRUN - Data lost

    Mask    = 0x7
}