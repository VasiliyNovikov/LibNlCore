using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using NetlinkCore.Interop.Generic;

namespace NetlinkCore.Generic;

public class EthToolNetlinkSocket() : GenericNetlinkSocket<GENL_ETHTOOL_MSG>("ethtool")
{
    public Dictionary<string, bool> GetFeatures(int linkIndex)
    {
        throw new NotImplementedException();
    }

    internal FeaturesResponse GetFeaturesRaw(int linkIndex)
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
                var features = new FeaturesResponse();
                foreach (var attr in message.Attributes)
                    if (attr.Name is GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_HW or
                                     GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_ACTIVE or
                                     GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_WANTED or
                                     GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_NOCHANGE)
                    {
                        var bitSet = attr.Name switch
                        {
                            GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_HW => features.Supported ??= new(),
                            GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_ACTIVE => features.Active ??= new(),
                            GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_WANTED => features.Wanted ??= new(),
                            GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_NOCHANGE => features.NoChange ??= new(),
                            _ => throw new InvalidOperationException()
                        };
                        foreach (var bitsetAttr in attr.AsNested<GENL_ETHTOOL_A_BITSET>())
                            switch (bitsetAttr.Name)
                            {
                                case GENL_ETHTOOL_A_BITSET.ETHTOOL_A_BITSET_BITS:
                                {
                                    var bits = bitSet.Bits ??= [];
                                    foreach (var bitsAttr in bitsetAttr.AsNested<GENL_ETHTOOL_A_BITSET_BITS_BIT>())
                                        if (bitsAttr.Name == GENL_ETHTOOL_A_BITSET_BITS_BIT.ETHTOOL_A_BITSET_BITS_BIT)
                                        {
                                            var bit = new FeatureBit();
                                            bits.Add(bit);
                                            foreach (var bitAttr in bitsAttr.AsNested<GENL_ETHTOOL_A_BITSET_BIT>())
                                                switch (bitAttr.Name)
                                                {
                                                    case GENL_ETHTOOL_A_BITSET_BIT.ETHTOOL_A_BITSET_BIT_INDEX:
                                                        bit.Index = bitAttr.AsValue<int>();
                                                        break;
                                                    case GENL_ETHTOOL_A_BITSET_BIT.ETHTOOL_A_BITSET_BIT_NAME:
                                                        bit.Name = bitAttr.AsString();
                                                        break;
                                                    case GENL_ETHTOOL_A_BITSET_BIT.ETHTOOL_A_BITSET_BIT_VALUE:
                                                        bit.Value = true;
                                                        break;
                                                }
                                        }
                                    break;
                                }
                                case GENL_ETHTOOL_A_BITSET.ETHTOOL_A_BITSET_NOMASK:
                                    bitSet.NoMask = true;
                                    break;
                                case GENL_ETHTOOL_A_BITSET.ETHTOOL_A_BITSET_SIZE:
                                    bitSet.Size = bitsetAttr.AsValue<uint>();
                                    break;
                                case GENL_ETHTOOL_A_BITSET.ETHTOOL_A_BITSET_MASK:
                                    bitSet.Mask = bitsetAttr.AsValue<ulong>();
                                    break;
                                case GENL_ETHTOOL_A_BITSET.ETHTOOL_A_BITSET_VALUE:
                                    bitSet.Value = bitsetAttr.AsValue<ulong>();
                                    break;
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
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public FeatureBitSet? Supported { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public FeatureBitSet? Active { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public FeatureBitSet? Wanted { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public FeatureBitSet? NoChange { get; set; }
    }

    internal sealed class FeatureBitSet
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? NoMask { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public uint? Size { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ulong? Value { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ulong? Mask { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<FeatureBit>? Bits { get; set; }
    }

    internal sealed class FeatureBit
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Index { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Value { get; set; }
    }
}
