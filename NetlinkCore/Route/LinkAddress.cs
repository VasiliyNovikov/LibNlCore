using System;
using System.Globalization;
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
    
    public static LinkAddress Parse(string addressString)
    {
        var slashIndex = addressString.IndexOf('/');
        if (slashIndex < 0)
        {
            var address = IPAddress.Parse(addressString);
            var prefixLength = (byte)(address.AddressFamily == AddressFamily.InterNetwork ? 32 : 128);
            return new(address, prefixLength);
        }
        else
        {
            var address = IPAddress.Parse(addressString.AsSpan(0, slashIndex));
            var prefixLength = byte.Parse(addressString.AsSpan(slashIndex + 1), CultureInfo.InvariantCulture);
            return new(address, prefixLength);
        }
    }
}