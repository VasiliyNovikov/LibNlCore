using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NetlinkCore.Generic;
using NetlinkCore.Route;

namespace NetlinkCore.Tests;

[TestClass]
public class EthToolNetlinkSocketTests
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    [TestMethod]
    public void EthToolNetlinkSocket_Create()
    {
        using var socket = new EthToolNetlinkSocket();
    }

    [TestMethod]
    public void EthToolNetlinkSocket_GetFeaturesRaw()
    {
        using var rtSocket = new RouteNetlinkSocket();
        var lo = rtSocket.GetLink("eth0");
        using var socket = new EthToolNetlinkSocket();
        var features = socket.GetFeaturesRaw(lo.Index);
        Console.Error.WriteLine(JsonSerializer.Serialize(features, JsonOptions));
    }

    [TestMethod]
    public void EthToolNetlinkSocket_GetFeatures()
    {
        using var rtSocket = new RouteNetlinkSocket();
        var lo = rtSocket.GetLink("lo");
        using var socket = new EthToolNetlinkSocket();
        var features = socket.GetFeatures(lo.Index);
        Assert.IsNotEmpty(features);
        Assert.IsTrue(features["loopback"]);
    }

    [TestMethod]
    public void EthToolNetlinkSocket_SetFeatures()
    {
        const string name = "ethtstv1";
        const string peer = "ethtstv2";
        const string feature = "tx-checksum-ipv4";
        using var rtSocket = new RouteNetlinkSocket();
        rtSocket.CreateVEth(name, peer);
        try
        {
            var link = rtSocket.GetLink(name);

            using var socket = new EthToolNetlinkSocket();
            var features = socket.GetFeatures(link.Index);
            Assert.IsFalse(features[feature]);

            var featuresToSet = new Dictionary<string, bool> { [feature] = true };
            socket.SetFeatures(link.Index, featuresToSet);

            features = socket.GetFeatures(link.Index);
            Assert.IsTrue(features[feature]);

            featuresToSet = new Dictionary<string, bool> { [feature] = false };
            socket.SetFeatures(link.Index, featuresToSet);

            features = socket.GetFeatures(link.Index);
            Assert.IsFalse(features[feature]);
        }
        finally
        {
            rtSocket.DeleteLink(name);
        }
    }
}