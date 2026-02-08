using System;

using NetlinkCore.Interop.Generic;

namespace NetlinkCore.Protocol.Generic;

internal readonly ref struct GenericNetlinkMessageWriter<TAttr>(Span<byte> buffer)
    where TAttr : unmanaged, Enum
{
    private readonly NetlinkMessageWriter<genlmsghdr, TAttr> _writer = new(buffer);

    public NetlinkMessageWriter Writer => _writer.Writer;

    public ref genlmsghdr Header => ref _writer.Header;

    public int FamilyId
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

    public NetlinkAttributeWriter<TAttr> Attributes => _writer.Attributes;
}