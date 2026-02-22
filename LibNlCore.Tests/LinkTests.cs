using System.Linq;
using System.Net;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using LibNlCore.Links;
using LibNlCore.Route;

using NetworkingPrimitivesCore;

namespace LibNlCore.Tests;

[TestClass]
public class LinkTests
{
    [TestMethod]
    public void LinkCollection_Open_Close()
    {
        using var collection = new LinkCollection();
        Assert.IsNotNull(collection);
    }

    [TestMethod]
    public void LinkCollection_GetLink()
    {
        using var collection = new LinkCollection();
        var link = collection["lo"];
        Assert.IsGreaterThan(0, link.Index);
        Assert.AreEqual("lo", link.Name);
        Assert.AreEqual(default(MACAddress), link.MacAddress);

        var addrs = link.Addresses.OrderBy(a => a.AddressFamily).ToArray();
        Assert.HasCount(2, addrs);
        Assert.AreEqual(IPAddress.Loopback, addrs[0].Address);
        Assert.AreEqual(8, addrs[0].PrefixLength);

        Assert.AreEqual(IPAddress.IPv6Loopback, addrs[1].Address);
        Assert.AreEqual(128, addrs[1].PrefixLength);
    }

    [TestMethod]
    public void LinkCollection_GetLinks()
    {
        using var collection = new LinkCollection();
        var links = collection.ToArray();
        Assert.IsGreaterThan(1, links.Length);
        var lo = links.FirstOrDefault(l => l.Name == "lo");
        Assert.IsNotNull(lo);
        Assert.IsGreaterThan(0, lo.Index);
    }

    [TestMethod]
    public void BridgeLink_Create_Delete()
    {
        const string bridgeName = "test_br0";
        const string bridgeAddress = "10.0.10.1";
        const byte bridgeAddressPrefix = 30;

        using var collection = new LinkCollection();

        Assert.ThrowsExactly<AssertFailedException>(() => Script.Exec("ip", "link", "show", bridgeName));

        var bridge = collection.CreateBridge(bridgeName);
        try
        {
            var linkInfo = Script.Exec("ip", "link", "show", bridgeName);
            Assert.AreNotEqual("", linkInfo);
            Assert.Contains(bridgeName, linkInfo);
            Assert.Contains("DOWN", linkInfo);

            Assert.IsGreaterThan(0, bridge.Index);
            Assert.AreEqual(bridgeName, bridge.Name);
            Assert.IsFalse(bridge.Up);
            //Assert.AreEqual(RtnlBridgePortState.Disabled, bridge.PortState);

            bridge.Addresses.Add(new(IPAddress.Parse(bridgeAddress), bridgeAddressPrefix));

            Assert.Contains($"{bridgeAddress}/{bridgeAddressPrefix}", Script.Exec("ip", "address", "show", bridgeName));

            bridge.Up = true;

            linkInfo = Script.Exec("ip", "link", "show", bridgeName);
            Assert.Contains("UP", linkInfo);

            //Assert.AreEqual(RtnlBridgePortState.Disabled, bridge.PortState);

            collection.Delete(bridge);

            Assert.ThrowsExactly<AssertFailedException>(() => Script.Exec("ip", "link", "show", bridgeName));
        }
        finally
        {
            Script.ExecNoThrow("ip", "link", "del", bridgeName);
        }
    }

    [TestMethod]
    public void VEthLink_Create_Delete()
    {
        const string vethName = "test_veth0";
        const string vethPeerName = "test_veth1";
        const string vethAddress = "10.0.10.1/30";
        const string vethPeerAddress = "10.0.10.2/30";

        using var collection = new LinkCollection();

        Assert.ThrowsExactly<AssertFailedException>(() => Script.Exec("ip", "link", "show", vethName));
        Assert.ThrowsExactly<AssertFailedException>(() => Script.Exec("ip", "link", "show", vethPeerName));
        
        try
        {
            var (veth, vethPeer) = collection.CreateVEth(vethName, vethPeerName);

            var linkInfo = Script.Exec("ip", "link", "show", vethName);
            Assert.AreNotEqual("", linkInfo);
            Assert.Contains(vethName, linkInfo);
            Assert.Contains("veth", linkInfo);
            Assert.Contains("DOWN", linkInfo);

            var peerInfo = Script.Exec("ip", "link", "show", vethPeerName);
            Assert.AreNotEqual("", peerInfo);
            Assert.Contains(vethPeerName, peerInfo);
            Assert.Contains("veth", peerInfo);
            Assert.Contains("DOWN", peerInfo);

            Assert.IsGreaterThan(0, veth.Index);
            Assert.IsGreaterThan(0, vethPeer.Index);

            Assert.AreEqual(vethName, veth.Name);
            Assert.AreEqual(vethPeerName, vethPeer.Name);

            veth.Addresses.Add(LinkAddress.Parse(vethAddress));
            Assert.Contains(vethAddress, Script.Exec("ip", "address", "show", vethName));

            vethPeer.Addresses.Add(LinkAddress.Parse(vethPeerAddress));
            Assert.Contains(vethPeerAddress, Script.Exec("ip", "address", "show", vethPeerName));

            veth.Up = true;
            vethPeer.Up = true;

            Assert.Contains("UP", Script.Exec("ip", "address", "show", vethName));
            Assert.Contains("UP", Script.Exec("ip", "address", "show", vethPeerName));

            collection.Delete(veth);

            Assert.ThrowsExactly<AssertFailedException>(() => Script.Exec("ip", "link", "show", vethName));
            Assert.ThrowsExactly<AssertFailedException>(() => Script.Exec("ip", "link", "show", vethPeerName));
        }
        finally
        {
            Script.ExecNoThrow("ip", "link", "del", vethName);
        }
    }
}