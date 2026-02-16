using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetlinkCore.Protocol;

internal readonly ref struct NetlinkMessageWriter
{
    private readonly ref NetlinkMessageHeader _header;

    public int SubType
    {
        get => (int)((NetlinkMessageType)_header.Type & ~NetlinkMessageType.Mask);
        set => _header.Type = (ushort)((NetlinkMessageType)value | ((NetlinkMessageType)_header.Type & NetlinkMessageType.Mask));
    }

    public NetlinkMessageFlags Flags
    {
        get => _header.Flags;
        set => _header.Flags = value;
    }

    public ref uint PortId => ref _header.PortId;

    public SpanWriter PayloadWriter { get; }

    public ReadOnlySpan<byte> Written => PayloadWriter.Written;

    public NetlinkMessageWriter(Span<byte> buffer)
    {
        _header = ref Unsafe.As<byte, NetlinkMessageHeader>(ref MemoryMarshal.GetReference(buffer));
        _header = default;
        PayloadWriter = new SpanWriter(buffer, ref _header.Length);
        PayloadWriter.Skip<NetlinkMessageHeader>();
    }
}

internal readonly ref struct NetlinkMessageWriter<THeader, TAttr>
    where THeader : unmanaged
    where TAttr : unmanaged, Enum
{
    private readonly ref THeader _header;
    private readonly NetlinkMessageWriter _writer;

    public NetlinkMessageWriter Writer => _writer;

    public ref THeader Header => ref _header;

    public int SubType
    {
        get => _writer.SubType;
        set => _writer.SubType = value;
    }

    public NetlinkMessageFlags Flags
    {
        get => _writer.Flags;
        set => _writer.Flags = value;
    }

    public ref uint PortId => ref _writer.PortId;

    public NetlinkAttributeWriter<TAttr> Attributes { get; }

    public NetlinkMessageWriter(Span<byte> buffer)
    {
        _writer = new NetlinkMessageWriter(buffer);
        var payloadWriter = _writer.PayloadWriter;
        _header = ref payloadWriter.Skip<THeader>();
        Attributes = new NetlinkAttributeWriter<TAttr>(payloadWriter);
    }
}