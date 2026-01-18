using System;
using System.Runtime.CompilerServices;

namespace NetlinkCore.Protocol.Route;

internal readonly ref struct RouteNetlinkMessageWriter<THeader, TAttr>
    where THeader : unmanaged
    where TAttr : unmanaged, Enum
{
    private readonly NetlinkMessageWriter _writer;
    private readonly ref THeader _header;

    public ref THeader Header => ref _header;

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

    public NetlinkAttributeWriter<TAttr> Attributes { get; }

    public ReadOnlySpan<byte> Written => _writer.PayloadWriter.Written;

    public RouteNetlinkMessageWriter(Span<byte> buffer)
    {
        _writer = new NetlinkMessageWriter(buffer);
        var payloadWriter = _writer.PayloadWriter;
        _header = ref payloadWriter.Skip<THeader>();
        Attributes = new NetlinkAttributeWriter<TAttr>(payloadWriter);
    }
}