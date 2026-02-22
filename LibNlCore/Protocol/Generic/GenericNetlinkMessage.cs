using System;
using System.Runtime.CompilerServices;

namespace LibNlCore.Protocol.Generic;

internal readonly ref struct GenericNetlinkMessage<TCmd, TAttr>
    where TCmd : unmanaged, Enum
    where TAttr : unmanaged, Enum
{
    private readonly ref readonly GenericNetlinkMessageHeader _header;

    public TCmd Command => Unsafe.BitCast<byte, TCmd>(_header.Cmd);
    public NetlinkAttributeCollection<TAttr> Attributes { get; }

    public GenericNetlinkMessage(in GenericNetlinkMessageHeader header, NetlinkAttributeCollection<TAttr> attributes)
    {
        _header = ref header;
        Attributes = attributes;
    }
}