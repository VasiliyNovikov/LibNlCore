namespace LibNlCore.Protocol;

internal enum NetlinkErrorMessageAttributes : ushort
{
    Unused,  // NLMSGERR_ATTR_UNUSED,
    Message, // NLMSGERR_ATTR_MSG,
    Offset,  // NLMSGERR_ATTR_OFFS,
    Cookie,  // NLMSGERR_ATTR_COOKIE,
    Policy   // NLMSGERR_ATTR_POLICY
}