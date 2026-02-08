using System;
using System.Net;
using System.Net.Sockets;

namespace NetlinkCore.Route;

public sealed class LinkAddress
{
    public IPAddress Address { get; }
    public byte PrefixLength { get; }
    public bool NoDad { get; }
    public AddressFamily AddressFamily => Address.AddressFamily;

    public LinkAddress(IPAddress address, byte prefixLength, bool noDad = false)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(prefixLength);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(prefixLength, address.AddressFamily == AddressFamily.InterNetwork ? 32 : 128);
        Address = address;
        PrefixLength = prefixLength;
        NoDad = noDad;
    }
}