using Microsoft.VisualStudio.TestTools.UnitTesting;

using NetlinkCore.Generic;
using NetlinkCore.Interop.Generic;
using NetlinkCore.Route;

namespace NetlinkCore.Tests;

[TestClass]
public class EthToolNetlinkSocketTests
{
    [TestMethod]
    public void EthToolNetlinkSocket_Create()
    {
        using var socket = new EthToolNetlinkSocket();
    }

    [TestMethod]
    public void EthToolNetlinkSocket_GetStringSet()
    {
        using var socket = new EthToolNetlinkSocket();
        var featureStrings = socket.GetStringSet(ethtool_stringset.ETH_SS_FEATURES);
        Assert.IsNotEmpty(featureStrings);
        Assert.IsTrue(featureStrings.Contains("tx-checksum-ipv4"));
    }

    [TestMethod]
    public void EthToolNetlinkSocket_FeatureIndices()
    {
        using var socket = new EthToolNetlinkSocket();
        var featureStrings = socket.GetStringSet(ethtool_stringset.ETH_SS_FEATURES);
        Assert.AreEqual(EthernetFeature.TxScatterGather, (EthernetFeature)featureStrings["tx-scatter-gather"]);
        Assert.AreEqual(EthernetFeature.TxUdpSegmentation, (EthernetFeature)featureStrings["tx-udp-segmentation"]);
        Assert.AreEqual(EthernetFeature.HsrDupOffload, (EthernetFeature)featureStrings["hsr-dup-offload"]);
    }

    [TestMethod]
    public void EthToolNetlinkSocket_GetFeatures()
    {
        using var rtSocket = new RouteNetlinkSocket();
        var lo = rtSocket.GetLink("lo");
        using var socket = new EthToolNetlinkSocket();
        var features = socket.GetFeatures(lo.Index);
        Assert.IsTrue(features[EthernetFeature.Loopback]);
    }

    [TestMethod]
    public void EthToolNetlinkSocket_SetFeatures()
    {
        const string name = "ethtstv1";
        const string peer = "ethtstv2";
        const EthernetFeature feature = EthernetFeature.TxTcpSegmentation;
        const bool defaultValue = true;

        using var rtSocket = new RouteNetlinkSocket();
        rtSocket.CreateVEth(name, peer);
        try
        {
            var link = rtSocket.GetLink(name);

            using var socket = new EthToolNetlinkSocket();
            var features = socket.GetFeatures(link.Index);
            Assert.AreEqual(defaultValue, features[feature]);

            var featuresToSet = new EthernetFeatures(features) { [feature] = !defaultValue };
            socket.SetFeatures(link.Index, features, featuresToSet);

            features = socket.GetFeatures(link.Index);
            Assert.AreNotEqual(defaultValue, features[feature]);

            featuresToSet = new EthernetFeatures(features) { [feature] = defaultValue };
            socket.SetFeatures(link.Index, features, featuresToSet);

            features = socket.GetFeatures(link.Index);
            Assert.AreEqual(defaultValue, features[feature]);
        }
        finally
        {
            rtSocket.DeleteLink(name);
        }
    }
}