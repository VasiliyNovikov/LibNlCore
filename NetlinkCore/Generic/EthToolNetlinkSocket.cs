using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using NetlinkCore.Interop.Generic;

namespace NetlinkCore.Generic;

public class EthToolNetlinkSocket() : GenericNetlinkSocket<GENL_ETHTOOL_MSG>("ethtool")
{
    public Dictionary<string, bool> GetFeatures(int linkIndex)
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
                var features = new Dictionary<string, bool>();
                foreach (var attr in message.Attributes)
                    if (attr.Name is GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_HW or GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_ACTIVE)
                        foreach (var bitsetAttr in attr.AsNested<GENL_ETHTOOL_A_BITSET>())
                        {
                            var noMask = false;
                            uint size = 0;
                            ulong value = 0;
                            ulong mask = 0;
                            if (bitsetAttr.Name == GENL_ETHTOOL_A_BITSET.ETHTOOL_A_BITSET_BITS)
                            {
                                foreach (var bitsAttr in bitsetAttr.AsNested<GENL_ETHTOOL_A_BITSET_BITS_BIT>())
                                    if (bitsAttr.Name == GENL_ETHTOOL_A_BITSET_BITS_BIT.ETHTOOL_A_BITSET_BITS_BIT)
                                    {
                                        string? featureName = null;
                                        var featureValue = false;
                                        foreach (var bitAttr in bitsAttr.AsNested<GENL_ETHTOOL_A_BITSET_BIT>())
                                            switch (bitAttr.Name)
                                            {
                                                case GENL_ETHTOOL_A_BITSET_BIT.ETHTOOL_A_BITSET_BIT_NAME:
                                                    featureName = bitAttr.AsString();
                                                    break;
                                                case GENL_ETHTOOL_A_BITSET_BIT.ETHTOOL_A_BITSET_BIT_VALUE:
                                                    featureValue = true;
                                                    break;
                                            }

                                        if (featureName != null)
                                            CollectionsMarshal.GetValueRefOrAddDefault(features, featureName, out _) |=
                                                featureValue;
                                    }
                            }
                            else if (bitsetAttr.Name == GENL_ETHTOOL_A_BITSET.ETHTOOL_A_BITSET_NOMASK)
                                noMask = true;
                            else if (bitsetAttr.Name == GENL_ETHTOOL_A_BITSET.ETHTOOL_A_BITSET_SIZE)
                                size = bitsetAttr.AsValue<uint>();
                            else if (bitsetAttr.Name == GENL_ETHTOOL_A_BITSET.ETHTOOL_A_BITSET_MASK)
                                mask = bitsetAttr.AsValue<ulong>();
                            else if (bitsetAttr.Name == GENL_ETHTOOL_A_BITSET.ETHTOOL_A_BITSET_VALUE)
                                value = bitsetAttr.AsValue<ulong>();

                            if (!noMask)
                            {
                                
                                throw new InvalidOperationException("Expected bitset with no mask");
                            }
                        }
                return features;
            }
        throw new InvalidOperationException("Did not receive features response");
    }

    public void SetFeatures(int linkIndex, Dictionary<string, bool> features)
    {
        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Large);
        var writer = GetWriter<GENL_ETHTOOL_A_FEATURES>(buffer);
        writer.Flags = NetlinkMessageFlags.Request | NetlinkMessageFlags.Ack;
        writer.Command = GENL_ETHTOOL_MSG.ETHTOOL_MSG_FEATURES_SET;
        using (var headerScope = writer.Attributes.WriteNested<GENL_ETHTOOL_A_HEADER>(GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_HEADER))
            headerScope.Writer.Write(GENL_ETHTOOL_A_HEADER.ETHTOOL_A_HEADER_DEV_INDEX, linkIndex);
        using (var featureScope = writer.Attributes.WriteNested<GENL_ETHTOOL_A_BITSET>(GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_WANTED))
        {
            featureScope.Writer.WriteFlag(GENL_ETHTOOL_A_BITSET.ETHTOOL_A_BITSET_NOMASK);
            featureScope.Writer.Write(GENL_ETHTOOL_A_BITSET.ETHTOOL_A_BITSET_SIZE, 64);
            using var bitsScope = featureScope.Writer.WriteNested<GENL_ETHTOOL_A_BITSET_BITS_BIT>(GENL_ETHTOOL_A_BITSET.ETHTOOL_A_BITSET_BITS);
            foreach (var feature in features)
            {
                using var bitScope = bitsScope.Writer.WriteNested<GENL_ETHTOOL_A_BITSET_BIT>(GENL_ETHTOOL_A_BITSET_BITS_BIT.ETHTOOL_A_BITSET_BITS_BIT);
                bitScope.Writer.Write(GENL_ETHTOOL_A_BITSET_BIT.ETHTOOL_A_BITSET_BIT_NAME, feature.Key);
                if (feature.Value)
                    bitScope.Writer.WriteFlag(GENL_ETHTOOL_A_BITSET_BIT.ETHTOOL_A_BITSET_BIT_VALUE);
            }
        }
        Post(buffer, writer);
    }

    internal sealed class FeaturesResponse
    {
        public FeatureBitSet? Supported { get; set; }
        public FeatureBitSet? Active { get; set; }
        public FeatureBitSet? Wanted { get; set; }
        public FeatureBitSet? NoChange { get; set; }
    }

    internal sealed class FeatureBitSet
    {
            public bool NoMask { get; set; }
            public uint Size { get; set; }
            public ulong Value { get; set; }
            public ulong Mask { get; set; }
            public List<FeatureBit>? Bits { get; set; }
    }

    internal sealed class FeatureBit
    {
        public string? Name { get; set; }
        public int Index { get; set; }
        public bool Value { get; set; }
    }
}
