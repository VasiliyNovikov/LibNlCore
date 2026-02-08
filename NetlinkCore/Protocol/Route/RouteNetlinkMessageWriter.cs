using System;
using System.Runtime.CompilerServices;

namespace NetlinkCore.Protocol.Route;

internal readonly ref struct RouteNetlinkMessageWriter<THeader, TAttr>(Span<byte> buffer)
    where THeader : unmanaged
    where TAttr : unmanaged, Enum
{
    private readonly NetlinkMessageWriter<THeader, TAttr> _writer = new(buffer);

    public NetlinkMessageWriter<THeader, TAttr> Writer => _writer;

    public ref THeader Header => ref _writer.Header;

    public RouteNetlinkMessageType Type
    {
        get => Unsafe.BitCast<int, RouteNetlinkMessageType>(_writer.SubType);
        set => _writer.SubType = Unsafe.BitCast<RouteNetlinkMessageType, int>(value);
    }

    public NetlinkMessageFlags Flags
    {
        get => _writer.Flags;
        set => _writer.Flags = value;
    }

    public ref uint PortId => ref _writer.PortId;

    public NetlinkAttributeWriter<TAttr> Attributes => _writer.Attributes;
}