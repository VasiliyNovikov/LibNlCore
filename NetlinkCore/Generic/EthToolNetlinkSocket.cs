using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using NetlinkCore.Interop.Generic;

namespace NetlinkCore.Generic;

public sealed class EthToolNetlinkSocket() : GenericNetlinkSocket<GENL_ETHTOOL_MSG>("ethtool")
{
    internal StringSet GetStringSet(ethtool_stringset stringSet)
    {
        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Large);
        var writer = GetWriter<GENL_ETHTOOL_A_STRSET>(buffer);
        writer.Flags = NetlinkMessageFlags.Request;
        writer.Command = GENL_ETHTOOL_MSG.ETHTOOL_MSG_STRSET_GET;
        using (writer.Attributes.WriteNested<GENL_ETHTOOL_A_HEADER>(GENL_ETHTOOL_A_STRSET.ETHTOOL_A_STRSET_HEADER))
        {
        }
        using (var setsAttrs = writer.Attributes.WriteNested<GENL_ETHTOOL_A_STRINGSETS>(GENL_ETHTOOL_A_STRSET.ETHTOOL_A_STRSET_STRINGSETS))
        {
            using var setAttrs = setsAttrs.Writer.WriteNested<GENL_ETHTOOL_A_STRINGSET>(GENL_ETHTOOL_A_STRINGSETS.ETHTOOL_A_STRINGSETS_STRINGSET);
            setAttrs.Writer.Write(GENL_ETHTOOL_A_STRINGSET.ETHTOOL_A_STRINGSET_ID, stringSet);
        }
        foreach (var message in Get(buffer, writer))
            if (message.Command == GENL_ETHTOOL_MSG.ETHTOOL_MSG_STRSET_GET)
                foreach (var attr in message.Attributes)
                    if (attr.Name == GENL_ETHTOOL_A_STRSET.ETHTOOL_A_STRSET_STRINGSETS)
                        foreach (var setsAttr in attr.AsNested<GENL_ETHTOOL_A_STRINGSETS>())
                            if (setsAttr.Name == GENL_ETHTOOL_A_STRINGSETS.ETHTOOL_A_STRINGSETS_STRINGSET)
                                foreach (var setAttr in setsAttr.AsNested<GENL_ETHTOOL_A_STRINGSET>())
                                    if (setAttr.Name == GENL_ETHTOOL_A_STRINGSET.ETHTOOL_A_STRINGSET_STRINGS)
                                    {
                                        var strings = new StringSet();
                                        foreach (var stringsAttr in setAttr.AsNested<GENL_ETHTOOL_A_STRINGS>())
                                            if (stringsAttr.Name == GENL_ETHTOOL_A_STRINGS.ETHTOOL_A_STRINGS_STRING)
                                            {
                                                int? index = null;
                                                string? name = null;
                                                foreach (var stringAttr in stringsAttr.AsNested<GENL_ETHTOOL_A_STRING>())
                                                    switch (stringAttr.Name)
                                                    {
                                                        case GENL_ETHTOOL_A_STRING.ETHTOOL_A_STRING_INDEX:
                                                            index = stringAttr.AsValue<int>();
                                                            break;
                                                        case GENL_ETHTOOL_A_STRING.ETHTOOL_A_STRING_VALUE:
                                                            name = stringAttr.AsString();
                                                            break;
                                                    }
                                                if (index is not null && !string.IsNullOrEmpty(name))
                                                    strings.Add(index.Value, name);
                                            }
                                        return strings;
                                    }
        throw new InvalidOperationException("Failed to get string set");
    }

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
        {
            headerAttrs.Writer.Write(GENL_ETHTOOL_A_HEADER.ETHTOOL_A_HEADER_DEV_INDEX, linkIndex);
            headerAttrs.Writer.Write(GENL_ETHTOOL_A_HEADER.ETHTOOL_A_HEADER_FLAGS, GENL_ETHTOOL_FLAGS.ETHTOOL_FLAG_COMPACT_BITSETS);
        }
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
                        var noMask = false;
                        var size = 0;
                        foreach (var bitsetAttr in attr.AsNested<GENL_ETHTOOL_A_BITSET>())
                            switch (bitsetAttr.Name)
                            {
                                case GENL_ETHTOOL_A_BITSET.ETHTOOL_A_BITSET_NOMASK:
                                    noMask = true;
                                    break;
                                case GENL_ETHTOOL_A_BITSET.ETHTOOL_A_BITSET_SIZE:
                                    size = bitsetAttr.AsValue<int>();
                                    break;
                                case GENL_ETHTOOL_A_BITSET.ETHTOOL_A_BITSET_MASK:
                                    bitSet.Mask = BinaryPrimitives.ReadUInt64LittleEndian(bitsetAttr.Data);
                                    break;
                                case GENL_ETHTOOL_A_BITSET.ETHTOOL_A_BITSET_VALUE:
                                    bitSet.Value = BinaryPrimitives.ReadUInt64LittleEndian(bitsetAttr.Data);
                                    break;
                            }
                        if (noMask)
                            bitSet.Mask = ulong.MaxValue >> (64 - size);
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

    internal class FeatureBitSet
    {
        public ulong Value { get; set; }
        public ulong Mask { get; set; }
    }
}