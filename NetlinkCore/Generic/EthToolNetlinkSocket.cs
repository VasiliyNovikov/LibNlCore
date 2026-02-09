using NetlinkCore.Interop.Generic;

namespace NetlinkCore.Generic;

public class EthToolNetlinkSocket() : GenericNetlinkSocket<GENL_ETHTOOL_MSG>("ethtool")
{
}