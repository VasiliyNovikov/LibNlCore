using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using NetlinkCore.Interop.Generic;

namespace NetlinkCore.Generic;

public class EthToolNetlinkSocket() : GenericNetlinkSocket<GENL_ETHTOOL_MSG>("ethtool")
{
    public Dictionary<string, LinkFeatureStatus> GetFeatures(int linkIndex)
    {
        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Large);
        var writer = GetWriter<GENL_ETHTOOL_A_FEATURES>(buffer);
        writer.Flags = NetlinkMessageFlags.Request;
        writer.Command = GENL_ETHTOOL_MSG.ETHTOOL_MSG_FEATURES_GET;
        using (var headerAttrs = writer.Attributes.WriteNested<GENL_ETHTOOL_A_HEADER>(GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_HEADER))
            headerAttrs.Writer.Write(GENL_ETHTOOL_A_HEADER.ETHTOOL_A_HEADER_DEV_INDEX, linkIndex);
        foreach (var message in Get(buffer, writer))
            if (message.Command == GENL_ETHTOOL_MSG.ETHTOOL_MSG_FEATURES_GET)
            {
                var features = new Dictionary<string, LinkFeatureStatus>();
                foreach (var attr in message.Attributes)
                {
                    var currentFeatureStatus = LinkFeatureStatus.None;
                    switch (attr.Name)
                    {
                        case GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_HW:
                            currentFeatureStatus = LinkFeatureStatus.Supported;
                            break;
                        case GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_WANTED:
                            currentFeatureStatus = LinkFeatureStatus.Wanted;
                            break;
                        case GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_ACTIVE:
                            currentFeatureStatus = LinkFeatureStatus.Active;
                            break;
                        case GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_NOCHANGE:
                            currentFeatureStatus = LinkFeatureStatus.NoChange;
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
                                    {
                                        var featureStatus = currentFeatureStatus != LinkFeatureStatus.Supported || featureValue
                                            ? currentFeatureStatus
                                            : LinkFeatureStatus.None;
                                        CollectionsMarshal.GetValueRefOrAddDefault(features, featureName, out _) |= featureStatus;
                                    }
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
