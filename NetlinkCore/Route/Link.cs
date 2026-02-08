using NetworkingPrimitivesCore;

namespace NetlinkCore.Route;

public record Link(int Index, string Name, bool Up, MACAddress? MacAddress, int? MasterIndex, int RXQueueCount, int TXQueueCount);