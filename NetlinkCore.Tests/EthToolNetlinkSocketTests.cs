using Microsoft.VisualStudio.TestTools.UnitTesting;

using NetlinkCore.Generic;
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
    public void EthToolNetlinkSocket_GetFeatures()
    {
        using var rtSocket = new RouteNetlinkSocket();
        var lo = rtSocket.GetLink("lo");
        using var socket = new EthToolNetlinkSocket();
        var features = socket.GetFeatures(lo.Index);
    }
}