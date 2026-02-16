using System;

using NetlinkCore.Interop.Generic;
using NetlinkCore.Protocol;

namespace NetlinkCore.Generic;

public sealed class ControlNetlinkSocket() : GenericNetlinkSocket<GENL_CTRL_CMD>(Family)
{
    private const ushort FamilyId = 0x10; // GENL_ID_CTRL
    private const byte FamilyVersion = 1;
    private static readonly GenericNetlinkFamily Family = new(FamilyId, FamilyVersion);

    public GenericNetlinkFamily GetFamily(string familyName)
    {
        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Small);
        var writer = GetWriter<GENL_CTRL_ATTR>(buffer);
        writer.Flags = NetlinkMessageFlags.Request;
        writer.Command = GENL_CTRL_CMD.CTRL_CMD_GETFAMILY;
        writer.Attributes.Write(GENL_CTRL_ATTR.CTRL_ATTR_FAMILY_NAME, familyName);
        foreach (var message in Get(buffer, writer))
            if (message.Command == GENL_CTRL_CMD.CTRL_CMD_NEWFAMILY)
            {
                ushort id = 0;
                byte version = 0;
                foreach (var attribute in message.Attributes)
                {
                    switch (attribute.Name)
                    {
                        case GENL_CTRL_ATTR.CTRL_ATTR_FAMILY_ID:
                            id = attribute.AsValue<ushort>();
                            break;
                        case GENL_CTRL_ATTR.CTRL_ATTR_VERSION:
                            version = (byte)attribute.AsValue<uint>();
                            break;
                    }
                }
                return id == 0
                    ? throw new InvalidOperationException("Family ID not found in response")
                    : new(id, version);
            }
        throw new InvalidOperationException($"Family {familyName} not found");
    }
}