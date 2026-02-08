using System;

using NetlinkCore.Interop.Generic;

namespace NetlinkCore.Protocol.Generic;

internal readonly ref struct GenericNetlinkMessage<TAttr>
    where TAttr : unmanaged, Enum
{
    private readonly ref readonly genlmsghdr _header;

    public NetlinkMessageFlags Flags { get; }
    public int FamilyId { get; }
    public ref readonly genlmsghdr Header => ref _header;
    public NetlinkAttributeCollection<TAttr> Attributes { get; }

    public GenericNetlinkMessage(NetlinkMessageFlags flags, int familyId, in genlmsghdr header, NetlinkAttributeCollection<TAttr> attributes)
    {
        _header = ref header;
        Flags = flags;
        FamilyId = familyId;
        Attributes = attributes;
    }
}