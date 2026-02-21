namespace NetlinkCore.Protocol.Generic;

public enum GenericControlAttributes : ushort
{
    Unspecified,      // CTRL_ATTR_UNSPEC
    FamilyId,         // CTRL_ATTR_FAMILY_ID
    FamilyName,       // CTRL_ATTR_FAMILY_NAME
    Version,          // CTRL_ATTR_VERSION
    HeaderSize,       // CTRL_ATTR_HDRSIZE
    MaxAttr,          // CTRL_ATTR_MAXATTR
    Ops,              // CTRL_ATTR_OPS
    MulticastGroups,  // CTRL_ATTR_MCAST_GROUPS
    Policy,           // CTRL_ATTR_POLICY
    OpPolicy,         // CTRL_ATTR_OP_POLICY
    Op                // CTRL_ATTR_OP
}