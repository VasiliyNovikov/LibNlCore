using Microsoft.VisualStudio.TestTools.UnitTesting;

using NetlinkCore.Generic;

namespace NetlinkCore.Tests;

[TestClass]
public class GenericNetlinkSocketTests
{
    [TestMethod]
    public void GenericNetlinkSocket_Create()
    {
        using var socket = new GenericNetlinkSocket();
        Assert.AreNotEqual(0u, socket.PortId);
    }

    [TestMethod]
    public void GenericNetlinkSocket_GetFamily()
    {
        using var socket = new GenericNetlinkSocket();
        var (id, version) = socket.GetFamily("nlctrl");
        Assert.AreNotEqual(0u, id);
        Assert.AreNotEqual(0u, version);
    }
}