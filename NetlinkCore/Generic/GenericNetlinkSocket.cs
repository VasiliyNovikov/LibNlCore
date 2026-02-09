using System;

using NetlinkCore.Protocol.Generic;

namespace NetlinkCore.Generic;

public class GenericNetlinkSocket<TCmd>(GenericNetlinkFamily family) : NetlinkSocket(NetlinkFamily.Generic)
    where TCmd : unmanaged, Enum
{
    protected GenericNetlinkSocket(string familyName)
        : this(GenericNetlinkFamilyResolver.Resolve(familyName))
    {
    }

    private protected GenericNetlinkMessageWriter<TCmd, TAttr> GetWriter<TAttr>(Span<byte> buffer)
        where TAttr : unmanaged, Enum
    {
        return new GenericNetlinkMessageWriter<TCmd, TAttr>(buffer)
        {
            PortId = PortId,
            FamilyId = family.Id,
            Header = default,
            Version = family.Version,
        };
    }

    private protected GenericNetlinkMessageCollection<TCmd, TAttr> Get<TAttr>(Span<byte> buffer, GenericNetlinkMessageWriter<TCmd, TAttr> message)
        where TAttr : unmanaged, Enum
    {
        return new(base.Get(buffer, message.Writer));
    }

    private void Post<TAttr>(Span<byte> buffer, GenericNetlinkMessageWriter<TCmd, TAttr> message)
        where TAttr : unmanaged, Enum
    {
        base.Post(buffer, message.Writer);
    }
}