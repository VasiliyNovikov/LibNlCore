using System;
using System.Runtime.CompilerServices;

namespace LibNlCore.Protocol.Generic;

internal readonly ref struct GenericNetlinkMessageWriter<TCmd, TAttr>(Span<byte> buffer)
    where TCmd : unmanaged, Enum
    where TAttr : unmanaged, Enum
{
    private readonly NetlinkMessageWriter<GenericNetlinkMessageHeader, TAttr> _writer = new(buffer);

    public NetlinkMessageWriter<GenericNetlinkMessageHeader, TAttr> Writer => _writer;

    internal ref GenericNetlinkMessageHeader Header => ref _writer.Header;
    public ref TCmd Command => ref Unsafe.As<byte, TCmd>(ref _writer.Header.Cmd);
    public ref byte Version => ref _writer.Header.Version;
    public ref ushort FamilyId => ref _writer.Type;
    public ref NetlinkMessageFlags Flags => ref _writer.Flags;
    public ref uint PortId => ref _writer.PortId;

    public NetlinkAttributeWriter<TAttr> Attributes => _writer.Attributes;
}