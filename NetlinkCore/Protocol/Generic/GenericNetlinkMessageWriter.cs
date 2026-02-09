using System;
using System.Runtime.CompilerServices;

using NetlinkCore.Interop.Generic;

namespace NetlinkCore.Protocol.Generic;

internal readonly ref struct GenericNetlinkMessageWriter<TCmd, TAttr>(Span<byte> buffer)
    where TCmd : unmanaged, Enum
    where TAttr : unmanaged, Enum
{
    private readonly NetlinkMessageWriter<genlmsghdr, TAttr> _writer = new(buffer);

    public NetlinkMessageWriter<genlmsghdr, TAttr> Writer => _writer;
    
    internal ref genlmsghdr Header => ref _writer.Header;
    public ref TCmd Command => ref Unsafe.As<byte, TCmd>(ref _writer.Header.cmd);
    public ref byte Version => ref _writer.Header.version;

    public ushort FamilyId
    {
        get => (ushort)_writer.SubType;
        set => _writer.SubType = value;
    }

    public NetlinkMessageFlags Flags
    {
        get => _writer.Flags;
        set => _writer.Flags = value;
    }

    public ref uint PortId => ref _writer.PortId;

    public NetlinkAttributeWriter<TAttr> Attributes => _writer.Attributes;
}