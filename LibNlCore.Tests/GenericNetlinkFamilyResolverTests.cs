using LinuxCore;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using LibNlCore.Generic;

namespace LibNlCore.Tests;

[TestClass]
public class GenericNetlinkFamilyResolverTests
{
    [TestMethod]
    public void GenericNetlinkFamilyResolver_Resolve()
    {
        var (id, version) = GenericNetlinkFamilyResolver.Resolve("nlctrl");
        Assert.AreNotEqual(0, id);
        Assert.AreNotEqual(0, version);

        var (id2, version2) = GenericNetlinkFamilyResolver.Resolve("nlctrl");
        Assert.AreEqual(id, id2);
        Assert.AreEqual(version, version2);
    }

    [TestMethod]
    public void GenericNetlinkFamilyResolver_Resolve_NonExistent()
    {
        var e = Assert.ThrowsExactly<NetlinkException>(() => GenericNetlinkFamilyResolver.Resolve("nonexistent"));
        Assert.AreEqual(LinuxErrorNumber.NoSuchFileOrDirectory, e.ErrorNumber);
        
        e = Assert.ThrowsExactly<NetlinkException>(() => GenericNetlinkFamilyResolver.Resolve("nonexistent"));
        Assert.AreEqual(LinuxErrorNumber.NoSuchFileOrDirectory, e.ErrorNumber);
    }
}