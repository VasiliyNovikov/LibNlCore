using NetworkingPrimitivesCore;

namespace NetlinkCore;

public record Link(int Index, string Name, bool Up, MACAddress? MacAddress, int? MasterIndex);