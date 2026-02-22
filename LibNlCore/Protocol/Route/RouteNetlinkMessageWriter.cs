using System;
using System.Runtime.CompilerServices;

namespace LibNlCore.Protocol.Route;

internal readonly ref struct RouteNetlinkMessageWriter<THeader, TAttr>(Span<byte> buffer)
    where THeader : unmanaged
    where TAttr : unmanaged, Enum
{
    private readonly NetlinkMessageWriter<THeader, TAttr> _writer = new(buffer);

    public NetlinkMessageWriter<THeader, TAttr> Writer => _writer;

    public ref THeader Header => ref _writer.Header;
    public ref RouteNetlinkMessageType Type => ref Unsafe.As<ushort, RouteNetlinkMessageType>(ref _writer.Type);
    public ref NetlinkMessageFlags Flags => ref _writer.Flags;
    public ref uint PortId => ref _writer.PortId;

    public NetlinkAttributeWriter<TAttr> Attributes => _writer.Attributes;
}