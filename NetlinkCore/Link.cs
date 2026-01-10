using NetworkingPrimitivesCore;

namespace NetlinkCore;

public record Link(int IfIndex, string Name, bool Up, MACAddress? MacAddress);