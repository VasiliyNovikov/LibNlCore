using System;
using System.Net.Sockets;

using LinuxCore;

using NetlinkCore.Interop;
using NetlinkCore.Protocol;

namespace NetlinkCore;

public abstract class NetlinkSocket : LinuxSocketBase
{
    public uint PortId { get; }

    protected NetlinkSocket(NetlinkFamily family) : base(LinuxAddressFamily.Netlink, LinuxSocketType.Raw, (ProtocolType)family)
    {
        SetOption(Constants.NETLINK_CAP_ACK, 1);
        SetOption(Constants.NETLINK_EXT_ACK, 1);
        SetOption(Constants.NETLINK_GET_STRICT_CHK, 1);
        var address = new sockaddr_nl
        {
            nl_family = (ushort)LinuxAddressFamily.Netlink,
            nl_pad = 0,
            nl_pid = 0,
            nl_groups = 0
        };
        Bind(address);
        Connect(address);
        GetAddress(out address);
        PortId = address.nl_pid;
    }

    private void SetOption(int option, int value) => base.SetOption(LinuxSocketOptionLevel.Netlink, option, value);

    private protected NetlinkMessageCollection<THeader, TAttr> Get<THeader, TAttr>(Span<byte> buffer, NetlinkMessageWriter<THeader, TAttr> message)
        where THeader : unmanaged
        where TAttr : unmanaged, Enum
    {
        Send(message.Writer.Written);
        return new(this, buffer);
    }

    private protected void Post<THeader, TAttr>(Span<byte> buffer, NetlinkMessageWriter<THeader, TAttr> message)
        where THeader : unmanaged
        where TAttr : unmanaged, Enum
    {
        foreach (var _ in Get(buffer, message)) ;
    }
}