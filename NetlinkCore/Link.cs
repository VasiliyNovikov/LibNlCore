using NetworkingPrimitivesCore;

namespace NetlinkCore;

public class Link(int ifIndex, string name, bool up, MACAddress? macAddress)
{
    public int IfIndex => ifIndex;
    public string Name => name;
    public bool Up => up;
    public MACAddress? MacAddress => macAddress;
}