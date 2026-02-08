using System;

using NetlinkCore.Interop.Generic;
using NetlinkCore.Protocol.Generic;

namespace NetlinkCore.Generic;

public sealed class GenericNetlinkSocket() : NetlinkSocket(NetlinkFamily.Generic)
{
    public (ushort Id, uint Version) GetFamily(string familyName)
    {
        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Small);
        var writer = GetWriter<GENL_CTRL_ATTR>(buffer);
        writer.FamilyId = Constants.GENL_ID_CTRL;
        writer.Flags = NetlinkMessageFlags.Request;
        writer.Header.version = Constants.GENL_ID_CTRL_VERSION;
        writer.Header.cmd = (byte)GENL_CTRL_CMD.CTRL_CMD_GETFAMILY;
        writer.Attributes.Write(GENL_CTRL_ATTR.CTRL_ATTR_FAMILY_NAME, familyName);
        foreach (var message in Get(buffer, writer))
            if (message.Header.cmd == (byte)GENL_CTRL_CMD.CTRL_CMD_NEWFAMILY)
            {
                ushort id = 0;
                uint version = 0;
                foreach (var attribute in message.Attributes)
                {
                    switch (attribute.Name)
                    {
                        case GENL_CTRL_ATTR.CTRL_ATTR_FAMILY_ID:
                            id = attribute.AsValue<ushort>();
                            break;
                        case GENL_CTRL_ATTR.CTRL_ATTR_VERSION:
                            version = attribute.AsValue<uint>();
                            break;
                    }
                }
                return id == 0
                    ? throw new InvalidOperationException("Family ID not found in response")
                    : (id, version);
            }
        throw new InvalidOperationException($"Family {familyName} not found");
    }

    private GenericNetlinkMessageWriter<TAttr> GetWriter<TAttr>(Span<byte> buffer)
        where TAttr : unmanaged, Enum
    {
        return new GenericNetlinkMessageWriter<TAttr>(buffer)
        {
            PortId = PortId,
            Header = default
        };
    }

    private GenericNetlinkMessageCollection<TAttr> Get<TAttr>(Span<byte> buffer, GenericNetlinkMessageWriter<TAttr> message)
        where TAttr : unmanaged, Enum
    {
        return new(base.Get(buffer, message.Writer));
    }

    private void Post<TAttr>(Span<byte> buffer, GenericNetlinkMessageWriter<TAttr> message)
        where TAttr : unmanaged, Enum
    {
        base.Post(buffer, message.Writer);
    }
}