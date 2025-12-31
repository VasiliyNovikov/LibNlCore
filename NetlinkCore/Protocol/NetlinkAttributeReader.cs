using System;
using System.Runtime.CompilerServices;

using NetlinkCore.Interop.Route;

namespace NetlinkCore.Protocol;

internal unsafe ref struct NetlinkAttributeReader(SpanReader reader)
{
    private SpanReader _reader = reader;

    public readonly bool IsEndOfBuffer => _reader.IsEndOfBuffer;

    public NetlinkAttribute<TName> Read<TName>() where TName : unmanaged, Enum
    {
        ref readonly var header = ref _reader.Read<rtattr>();
        return new NetlinkAttribute<TName>(Unsafe.BitCast<ushort, TName>(header.rta_type), _reader.Read(header.rta_len - sizeof(rtattr)));
    }
}