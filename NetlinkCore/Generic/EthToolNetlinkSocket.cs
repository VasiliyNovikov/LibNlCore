using System;
using System.Collections.Generic;

using NetlinkCore.Interop.Generic;

namespace NetlinkCore.Generic;

public class EthToolNetlinkSocket() : GenericNetlinkSocket<GENL_ETHTOOL_MSG>("ethtool")
{
    public LinkFeatures GetFeatures(int linkIndex)
    {
        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Large);
        var writer = GetWriter<GENL_ETHTOOL_A_FEATURES>(buffer);
        writer.Flags = NetlinkMessageFlags.Request;
        writer.Command = GENL_ETHTOOL_MSG.ETHTOOL_MSG_FEATURES_GET;
        using var headerAttrs = writer.Attributes.WriteNested<GENL_ETHTOOL_A_HEADER>(GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_HEADER);
        headerAttrs.Writer.Write(GENL_ETHTOOL_A_HEADER.ETHTOOL_A_HEADER_DEV_INDEX, linkIndex);
        foreach (var message in Get(buffer, writer))
            if (message.Command == GENL_ETHTOOL_MSG.ETHTOOL_MSG_FEATURES_GET)
            {
                var features = new LinkFeatures();
                foreach (var attr in message.Attributes)
                {
                    Dictionary<string, bool> featureDict;
                    switch (attr.Name)
                    {
                        case GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_HW:
                            featureDict = features.Supported;
                            break;
                        case GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_WANTED:
                            featureDict = features.Wanted;
                            break;
                        case GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_ACTIVE:
                            featureDict = features.Active;
                            break;
                        case GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_NOCHANGE:
                            featureDict = features.NoChange;
                            break;
                        default:
                            continue;
                    }
                    foreach (var bitsetAttr in attr.AsNested<GENL_ETHTOOL_A_BITSET>())
                    {
                        if (bitsetAttr.Name == GENL_ETHTOOL_A_BITSET.ETHTOOL_A_BITSET_BITS)
                        {
                            foreach (var bitsAttr in bitsetAttr.AsNested<GENL_ETHTOOL_A_BITSET_BITS_BIT>())
                            {
                                if (bitsAttr.Name == GENL_ETHTOOL_A_BITSET_BITS_BIT.ETHTOOL_A_BITSET_BITS_BIT)
                                {
                                    string? featureName = null;
                                    var featureValue = false;
                                    foreach (var bitAttr in bitsAttr.AsNested<GENL_ETHTOOL_A_BITSET_BIT>())
                                    {
                                        if (bitAttr.Name == GENL_ETHTOOL_A_BITSET_BIT.ETHTOOL_A_BITSET_BIT_NAME)
                                            featureName = bitAttr.AsString();
                                        else if (bitAttr.Name == GENL_ETHTOOL_A_BITSET_BIT.ETHTOOL_A_BITSET_BIT_VALUE)
                                            featureValue = true;
                                    }
                                    if (featureName != null)
                                        featureDict[featureName] = featureValue;
                                }
                            }
                        }
                    }
                }
                return features;
            }
        throw new InvalidOperationException("Did not receive features response");
    }
}