namespace NetlinkCore.Protocol.Generic;

public enum GenericControlCommand : byte
{
    Unspecified,          // CTRL_CMD_UNSPEC
    NewFamily,            // CTRL_CMD_NEWFAMILY
    DeleteFamily,         // CTRL_CMD_DELFAMILY
    GetFamily,            // CTRL_CMD_GETFAMILY
    NewOps,               // CTRL_CMD_NEWOPS
    DeleteOps,            // CTRL_CMD_DELOPS
    GetOps,               // CTRL_CMD_GETOPS
    NewMcastGroup,        // CTRL_CMD_NEWMCAST_GRP
    DeleteMulticastGroup, // CTRL_CMD_DELMCAST_GRP
    GetMulticastGroup,    // CTRL_CMD_GETMCAST_GRP (unused)
    GetPolicy,            // CTRL_CMD_GETPOLICY
}