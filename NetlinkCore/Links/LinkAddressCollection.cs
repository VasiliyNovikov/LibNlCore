using System.Collections;
using System.Collections.Generic;

using NetlinkCore.Route;

namespace NetlinkCore.Links;

public sealed class LinkAddressCollection : IEnumerable<LinkAddress>
{
    private readonly RouteNetlinkSocket _socket;
    private readonly int _linkIndex;

    internal LinkAddressCollection(RouteNetlinkSocket socket, int linkIndex)
    {
        _socket = socket;
        _linkIndex = linkIndex;
    }

    public void Add(LinkAddress address) => _socket.AddAddress(_linkIndex, address);

    public void Remove(LinkAddress address) => _socket.DeleteAddress(_linkIndex, address);

    public void Clear()
    {
        foreach (var addr in this)
            Remove(addr);
    }

    public IEnumerator<LinkAddress> GetEnumerator()
    {
        foreach (var address in _socket.GetAddresses(_linkIndex))
            yield return address;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}