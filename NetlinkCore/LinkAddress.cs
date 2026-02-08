using System;
using System.Net;
using System.Net.Sockets;

namespace NetlinkCore;

public readonly struct LinkAddress
{
    public IPAddress Address { get; }
    public byte PrefixLength { get; }
    public AddressFamily AddressFamily => Address.AddressFamily;

    public LinkAddress(IPAddress address, byte prefixLength)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(prefixLength);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(prefixLength, address.AddressFamily == AddressFamily.InterNetwork ? 32 : 128);
        Address = address;
        PrefixLength = prefixLength;
    }
}