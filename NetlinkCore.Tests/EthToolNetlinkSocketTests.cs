using Microsoft.VisualStudio.TestTools.UnitTesting;

using NetlinkCore.Generic;

namespace NetlinkCore.Tests;

[TestClass]
public class EthToolNetlinkSocketTests
{
    [TestMethod]
    public void EthToolNetlinkSocket_Create()
    {
        using var socket = new EthToolNetlinkSocket();
    }
}