using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using LibNlCore.Protocol;
using LibNlCore.Protocol.Generic.EthTool;

namespace LibNlCore.Generic.EthTool;

public sealed class EthToolNetlinkSocket() : GenericNetlinkSocket<EthToolCommand>("ethtool")
{
    internal Dictionary<int, string> GetStringSet(EthToolStringSet stringSet)
    {
        using var buffer = new NetlinkBuffer(NetlinkBufferSize.Large);
        var writer = GetWriter<EthToolStringSetRequestAttributes>(buffer);
        writer.Flags = NetlinkMessageFlags.Request;
        writer.Command = EthToolCommand.StringSetGet;
        using (writer.Attributes.WriteNested<EthToolHeaderAttributes>(EthToolStringSetRequestAttributes.Header))
        {
        }
        using (var setsAttrs = writer.Attributes.WriteNested<EthToolStringSetsAttributes>(EthToolStringSetRequestAttributes.StringSets))
        {
            using var setAttrs = setsAttrs.Writer.WriteNested<EthToolStringSetAttributes>(EthToolStringSetsAttributes.StringSet);
            setAttrs.Writer.Write(EthToolStringSetAttributes.Id, stringSet);
        }
        foreach (var message in Get(buffer, writer))
            if (message.Command == EthToolCommand.StringSetGet)
                foreach (var attr in message.Attributes)
                    if (attr.Name == EthToolStringSetRequestAttributes.StringSets)
                        foreach (var setsAttr in attr.AsNested<EthToolStringSetsAttributes>())
                            if (setsAttr.Name == EthToolStringSetsAttributes.StringSet)
                                foreach (var setAttr in setsAttr.AsNested<EthToolStringSetAttributes>())
                                    if (setAttr.Name == EthToolStringSetAttributes.Strings)
                                    {
                                        var strings = new Dictionary<int, string>();
                                        foreach (var stringsAttr in setAttr.AsNested<EthToolStringsAttributes>())
                                            if (stringsAttr.Name == EthToolStringsAttributes.String)
                                            {
                                                int? index = null;
                                                string? name = null;
                                                foreach (var stringAttr in stringsAttr.AsNested<EthToolStringAttributes>())
                                                    switch (stringAttr.Name)
                                                    {
                                                        case EthToolStringAttributes.Index:
                                                            index = stringAttr.AsValue<int>();
                                                            break;
                                                        case EthToolStringAttributes.Value:
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
        var writer = GetWriter<EthToolFeaturesAttributes>(buffer);
        writer.Flags = NetlinkMessageFlags.Request;
        writer.Command = EthToolCommand.FeaturesGet;
        using (var headerScope = writer.Attributes.WriteNested<EthToolHeaderAttributes>(EthToolFeaturesAttributes.Header))
        {
            headerScope.Writer.Write(EthToolHeaderAttributes.LinkIndex, linkIndex);
            headerScope.Writer.Write(EthToolHeaderAttributes.Flags, EthToolFlags.CompactBitsets);
        }
        foreach (var message in Get(buffer, writer))
            if (message.Command == EthToolCommand.FeaturesGet)
            {
                ulong supported = 0;
                ulong active = 0;
                ulong wanted = 0;
                ulong noChange = 0;
                foreach (var attr in message.Attributes)
                    if (attr.Name is EthToolFeaturesAttributes.Active or
                                     EthToolFeaturesAttributes.Supported or
                                     EthToolFeaturesAttributes.Wanted or
                                     EthToolFeaturesAttributes.NoChange)
                    {
                        scoped ref var bitSet = ref Unsafe.NullRef<ulong>();
                        switch (attr.Name)
                        {
                            case EthToolFeaturesAttributes.Active:
                                bitSet = ref active;
                                break;
                            case EthToolFeaturesAttributes.Supported:
                                bitSet = ref supported;
                                break;
                            case EthToolFeaturesAttributes.Wanted:
                                bitSet = ref wanted;
                                break;
                            case EthToolFeaturesAttributes.NoChange:
                                bitSet = ref noChange;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }

                        var noMask = false;
                        var size = 0;
                        var mask = 0UL;
                        foreach (var bitsetAttr in attr.AsNested<EthToolBitsetAttributes>())
                            switch (bitsetAttr.Name)
                            {
                                case EthToolBitsetAttributes.NoMask:
                                    noMask = true;
                                    break;
                                case EthToolBitsetAttributes.Size:
                                    size = bitsetAttr.AsValue<int>();
                                    break;
                                case EthToolBitsetAttributes.Mask:
                                    mask = BinaryPrimitives.ReadUInt64LittleEndian(bitsetAttr.Data);
                                    break;
                                case EthToolBitsetAttributes.Value:
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
        var writer = GetWriter<EthToolFeaturesAttributes>(buffer);
        writer.Flags = NetlinkMessageFlags.Request | NetlinkMessageFlags.Ack;
        writer.Command = EthToolCommand.FeaturesSet;
        using (var headerScope = writer.Attributes.WriteNested<EthToolHeaderAttributes>(EthToolFeaturesAttributes.Header))
        {
            headerScope.Writer.Write(EthToolHeaderAttributes.LinkIndex, linkIndex);
            headerScope.Writer.Write(EthToolHeaderAttributes.Flags, EthToolFlags.CompactBitsets);
        }
        using (var featureScope = writer.Attributes.WriteNested<EthToolBitsetAttributes>(EthToolFeaturesAttributes.Wanted))
        {
            featureScope.Writer.Write(EthToolBitsetAttributes.Size, 64);
            BinaryPrimitives.WriteUInt64LittleEndian(featureScope.Writer.PrepareWrite(EthToolBitsetAttributes.Mask, 8), updateMask);
            BinaryPrimitives.WriteUInt64LittleEndian(featureScope.Writer.PrepareWrite(EthToolBitsetAttributes.Value, 8), update);
        }
        Post(buffer, writer);
    }
}