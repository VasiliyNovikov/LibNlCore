using System;
using System.Runtime.CompilerServices;

using NetlinkCore.Interop.Generic;

namespace NetlinkCore.Protocol.Generic;

internal readonly ref struct GenericNetlinkMessage<TCmd, TAttr>
    where TCmd : unmanaged, Enum
    where TAttr : unmanaged, Enum
{
    private readonly ref readonly genlmsghdr _header;

    public TCmd Command => Unsafe.BitCast<byte, TCmd>(_header.cmd);
    public NetlinkAttributeCollection<TAttr> Attributes { get; }

    public GenericNetlinkMessage(in genlmsghdr header, NetlinkAttributeCollection<TAttr> attributes)
    {
        _header = ref header;
        Attributes = attributes;
    }
}