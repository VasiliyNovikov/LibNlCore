using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

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

    public EthernetFeatures GetFeatures(int linkIndex)
    {
        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Large);
        var writer = GetWriter<GENL_ETHTOOL_A_FEATURES>(buffer);
        writer.Flags = NetlinkMessageFlags.Request;
        writer.Command = GENL_ETHTOOL_MSG.ETHTOOL_MSG_FEATURES_GET;
        using (var headerScope = writer.Attributes.WriteNested<GENL_ETHTOOL_A_HEADER>(GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_HEADER))
        {
            headerScope.Writer.Write(GENL_ETHTOOL_A_HEADER.ETHTOOL_A_HEADER_DEV_INDEX, linkIndex);
            headerScope.Writer.Write(GENL_ETHTOOL_A_HEADER.ETHTOOL_A_HEADER_FLAGS, GENL_ETHTOOL_FLAGS.ETHTOOL_FLAG_COMPACT_BITSETS);
        }
        foreach (var message in Get(buffer, writer))
            if (message.Command == GENL_ETHTOOL_MSG.ETHTOOL_MSG_FEATURES_GET)
            {
                ulong supported = 0;
                ulong active = 0;
                ulong wanted = 0;
                ulong noChange = 0;
                foreach (var attr in message.Attributes)
                    if (attr.Name is GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_ACTIVE or
                                     GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_HW or
                                     GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_WANTED or
                                     GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_NOCHANGE)
                    {
                        scoped ref var bitSet = ref Unsafe.NullRef<ulong>();
                        switch (attr.Name)
                        {
                            case GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_ACTIVE:
                                bitSet = ref active;
                                break;
                            case GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_HW:
                                bitSet = ref supported;
                                break;
                            case GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_WANTED:
                                bitSet = ref wanted;
                                break;
                            case GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_NOCHANGE:
                                bitSet = ref noChange;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }

                        var noMask = false;
                        var size = 0;
                        var mask = 0UL;
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
                                    mask = BinaryPrimitives.ReadUInt64LittleEndian(bitsetAttr.Data);
                                    break;
                                case GENL_ETHTOOL_A_BITSET.ETHTOOL_A_BITSET_VALUE:
                                    bitSet = BinaryPrimitives.ReadUInt64LittleEndian(bitsetAttr.Data);
                                    break;
                            }
                        if (noMask)
                            mask = ulong.MaxValue >> (64 - size);
                        bitSet &= mask;
                    }
                return new EthernetFeatures(active, supported, wanted, noChange);
            }
        throw new InvalidOperationException("Did not receive features response");
    }

    public void SetFeatures(int linkIndex, EthernetFeatures origFeatures, EthernetFeatures features)
    {
        var update = features.Active;
        var updateMask = origFeatures.Active ^ update;
        if (updateMask == 0)
            return;

        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Large);
        var writer = GetWriter<GENL_ETHTOOL_A_FEATURES>(buffer);
        writer.Flags = NetlinkMessageFlags.Request | NetlinkMessageFlags.Ack;
        writer.Command = GENL_ETHTOOL_MSG.ETHTOOL_MSG_FEATURES_SET;
        using (var headerScope = writer.Attributes.WriteNested<GENL_ETHTOOL_A_HEADER>(GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_HEADER))
        {
            headerScope.Writer.Write(GENL_ETHTOOL_A_HEADER.ETHTOOL_A_HEADER_DEV_INDEX, linkIndex);
            headerScope.Writer.Write(GENL_ETHTOOL_A_HEADER.ETHTOOL_A_HEADER_FLAGS, GENL_ETHTOOL_FLAGS.ETHTOOL_FLAG_COMPACT_BITSETS);
        }
        using (var featureScope = writer.Attributes.WriteNested<GENL_ETHTOOL_A_BITSET>(GENL_ETHTOOL_A_FEATURES.ETHTOOL_A_FEATURES_WANTED))
        {
            featureScope.Writer.Write(GENL_ETHTOOL_A_BITSET.ETHTOOL_A_BITSET_SIZE, 64);
            BinaryPrimitives.WriteUInt64LittleEndian(featureScope.Writer.PrepareWrite(GENL_ETHTOOL_A_BITSET.ETHTOOL_A_BITSET_MASK, 8), updateMask);
            BinaryPrimitives.WriteUInt64LittleEndian(featureScope.Writer.PrepareWrite(GENL_ETHTOOL_A_BITSET.ETHTOOL_A_BITSET_VALUE, 8), update);
        }
        Post(buffer, writer);
    }
}