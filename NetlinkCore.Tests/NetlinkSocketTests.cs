using System.Linq;

using LinuxCore;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NetworkingPrimitivesCore;

namespace NetlinkCore.Tests;

[TestClass]
public class NetlinkSocketTests
{
    [TestMethod]
    public void RouteNetlinkSocket_Create()
    {
        using var socket = new RouteNetlinkSocket();
        Assert.AreNotEqual(0u, socket.PortId);
    }

    [TestMethod]
    public void RouteNetlinkSocket_GetLink()
    {
        using var socket = new RouteNetlinkSocket();
        var link = socket.GetLink("lo");
        Assert.IsNotNull(link);
        Assert.AreEqual("lo", link.Name);
        Assert.IsGreaterThan(0, link.Index);
        Assert.IsTrue(link.Up);
        Assert.IsNull(link.MasterIndex);
        Assert.AreEqual(default(MACAddress), link.MacAddress);
    }

    [TestMethod]
    public void RouteNetlinkSocket_GetLinks()
    {
        using var socket = new RouteNetlinkSocket();
        var links = socket.GetLinks();
        Assert.IsGreaterThan(1, links.Length);
        var lo = links.Single(l => l.Name == "lo");
        Assert.AreEqual("lo", lo.Name);
        Assert.IsGreaterThan(0, lo.Index);
        Assert.IsTrue(lo.Up);
        Assert.IsNull(lo.MasterIndex);
        Assert.AreEqual(default(MACAddress), lo.MacAddress);
    }

    [TestMethod]
    public void RouteNetlinkSocket_GetNonExistingLink()
    {
        using var socket = new RouteNetlinkSocket();
        var error = Assert.ThrowsExactly<NetlinkException>(() => socket.GetLink("lo1234"));
        Assert.AreEqual(LinuxErrorNumber.NoSuchDevice, error.ErrorNumber);
    }

    [TestMethod]
    public void RouteNetlinkSocket_Create_Delete_VEth()
    {
        using var socket = new RouteNetlinkSocket();
        const string name = "veth1test";
        const string peerName = "veth1ptest";

        socket.CreateVEth(name, peerName);

        var link = socket.GetLink(name);
        Assert.AreEqual(name, link.Name);
        Assert.IsGreaterThan(0, link.Index);
        Assert.IsFalse(link.Up);
        Assert.AreNotEqual(default(MACAddress), link.MacAddress);

        var peer = socket.GetLink(peerName);
        Assert.AreEqual(peerName, peer.Name);
        Assert.IsGreaterThan(0, peer.Index);
        Assert.IsFalse(peer.Up);
        Assert.AreNotEqual(default(MACAddress), peer.MacAddress);

        socket.DeleteLink(name);

        var error = Assert.ThrowsExactly<NetlinkException>(() => socket.GetLink(name));
        Assert.AreEqual(LinuxErrorNumber.NoSuchDevice, error.ErrorNumber);
        error = Assert.ThrowsExactly<NetlinkException>(() => socket.GetLink(peerName));
        Assert.AreEqual(LinuxErrorNumber.NoSuchDevice, error.ErrorNumber);
    }

    [TestMethod]
    public void RouteNetlinkSocket_Create_Delete_Bridge()
    {
        using var socket = new RouteNetlinkSocket();
        const string name = "br1test";
        var bridgeMac = MACAddress.Parse("02:12:34:56:78:9A");

        socket.CreateBridge(name);

        var link = socket.GetLink(name);
        Assert.AreEqual(name, link.Name);
        Assert.IsGreaterThan(0, link.Index);
        Assert.IsFalse(link.Up);
        Assert.AreNotEqual(default(MACAddress), link.MacAddress);

        var change = link with { MacAddress = bridgeMac, Up = true };
        socket.UpdateLink(link, change);

        link = socket.GetLink(name);
        Assert.AreEqual(bridgeMac, link.MacAddress);
        Assert.IsTrue(link.Up);

        socket.DeleteLink(name);

        var error = Assert.ThrowsExactly<NetlinkException>(() => socket.GetLink(name));
        Assert.AreEqual(LinuxErrorNumber.NoSuchDevice, error.ErrorNumber);
    }

    [TestMethod]
    public void RouteNetlinkSocket_Set_Unset_Master()
    {
        using var socket = new RouteNetlinkSocket();
        const string brName = "br2test";
        const string vethName = "veth2test";
        const string vethPeerName = "veth2ptest";

        socket.CreateBridge(brName);
        socket.CreateVEth(vethName, vethPeerName);
        try
        {
            var bridge = socket.GetLink(brName);
            var veth = socket.GetLink(vethName);

            Assert.IsNull(veth.MasterIndex);
            Assert.IsNull(bridge.MasterIndex);

            var vethChange = veth with { MasterIndex = bridge.Index };
            socket.UpdateLink(veth, vethChange);

            veth = socket.GetLink(vethName);
            Assert.AreEqual(bridge.Index, veth.MasterIndex);

            vethChange = veth with { MasterIndex = null };
            socket.UpdateLink(veth, vethChange);

            veth = socket.GetLink(vethName);
            Assert.IsNull(veth.MasterIndex);
        }
        finally
        {
            socket.DeleteLink(vethName);
            socket.DeleteLink(brName);
        }
    }
}