using System;
using System.Runtime.CompilerServices;

namespace NetlinkCore.Protocol.Route;

internal readonly ref struct RouteNetlinkMessageWriter<THeader, TMsgType, TAttr> where THeader : unmanaged
    where TMsgType : unmanaged, Enum
    where TAttr : unmanaged, Enum
{
    private readonly NetlinkMessageWriter _writer;
    private readonly ref THeader _header;

    public ref THeader Header => ref _header;

    public TMsgType Type
    {
        get => Unsafe.BitCast<int, TMsgType>(_writer.SubType);
        set => _writer.SubType = Unsafe.BitCast<TMsgType, int>(value);
    }

    public NetlinkMessageFlags Flags
    {
        get => _writer.Flags;
        set => _writer.Flags = value;
    }

    public ref uint PortId => ref _writer.PortId;

    public RouteNetlinkMessageWriter(Span<byte> buffer)
    {
        _writer = new NetlinkMessageWriter(buffer);
        _header = ref _writer.PayloadWriter.Skip<THeader>();
    }
}