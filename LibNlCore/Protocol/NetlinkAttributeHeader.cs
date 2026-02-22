using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LibNlCore.Protocol;

// struct nlattr
[StructLayout(LayoutKind.Sequential)]
internal struct NetlinkAttributeHeader<TAttr>
    where TAttr : unmanaged, Enum
{
    public ushort Length; // nla_len
    private ushort Type;  // nla_type

    public TAttr Name
    {
        get => Unsafe.BitCast<ushort, TAttr>(GetTypePart(NetlinkAttributeFlags.TypeMask));
        set => SetTypePart(NetlinkAttributeFlags.TypeMask, Unsafe.BitCast<TAttr, ushort>(value));
    }

    public bool IsNested
    {
        get => GetTypePart(NetlinkAttributeFlags.Nested) != 0;
        set => SetTypePart(NetlinkAttributeFlags.Nested, value ? (ushort)NetlinkAttributeFlags.Nested : (ushort)0);
    }

    public bool IsNetworkByteOrder
    {
        get => GetTypePart(NetlinkAttributeFlags.NetworkByteOrder) != 0;
        set => SetTypePart(NetlinkAttributeFlags.NetworkByteOrder, value ? (ushort)NetlinkAttributeFlags.NetworkByteOrder : (ushort)0);
    }

    private readonly ushort GetTypePart(NetlinkAttributeFlags mask) => (ushort)(Type & (ushort)mask);
    private void SetTypePart(NetlinkAttributeFlags mask, ushort value) => Type = (ushort)((Type & (ushort)~mask) | (value & (ushort)mask));
}