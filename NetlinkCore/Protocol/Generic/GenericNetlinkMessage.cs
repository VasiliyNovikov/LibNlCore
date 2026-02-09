using System;
using System.Runtime.CompilerServices;

using NetlinkCore.Interop.Generic;

namespace NetlinkCore.Protocol.Generic;

internal readonly ref struct GenericNetlinkMessage<TCmd, TAttr>
    where TCmd : unmanaged, Enum
    where TAttr : unmanaged, Enum
{
    private readonly ref readonly genlmsghdr _header;

    public NetlinkMessageFlags Flags { get; }
    public int FamilyId { get; }
    public TCmd Command => Unsafe.BitCast<byte, TCmd>(_header.cmd);
    public byte Version => _header.version;
    public NetlinkAttributeCollection<TAttr> Attributes { get; }

    public GenericNetlinkMessage(NetlinkMessageFlags flags, int familyId, in genlmsghdr header, NetlinkAttributeCollection<TAttr> attributes)
    {
        _header = ref header;
        Flags = flags;
        FamilyId = familyId;
        Attributes = attributes;
    }
}