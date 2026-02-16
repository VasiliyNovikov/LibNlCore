namespace NetlinkCore.Protocol;

internal enum NetlinkAttributeFlags : ushort
{
    Nested           = 1 << 15, // NLA_F_NESTED        - Attribute payload contains nested attributes
    NetworkByteOrder = 1 << 14, // NLA_F_NET_BYTEORDER - Attribute payload is in network byte order

    FlagsMask = Nested | NetworkByteOrder,
    TypeMask = unchecked((ushort)~FlagsMask) // NLA_TYPE_MASK
}