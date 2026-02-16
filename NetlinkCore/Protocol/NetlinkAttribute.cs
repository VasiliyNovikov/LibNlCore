using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace NetlinkCore.Protocol;

internal readonly ref struct NetlinkAttribute<TAttr>(TAttr name, bool isNested, bool isNetworkByteOrder, ReadOnlySpan<byte> data) where TAttr : unmanaged, Enum
{
    public TAttr Name => name;
    public bool IsNested => isNested;
    public bool IsNetworkByteOrder => isNetworkByteOrder;
    public ReadOnlySpan<byte> Data { get; } = data;

    public T AsValue<T>() where T : unmanaged
    {
        return Data.Length == Unsafe.SizeOf<T>()
            ? MemoryMarshal.Read<T>(Data)
            : throw new InvalidOperationException($"Cannot convert attribute value to type {typeof(T).Name} because the size does not match");
    }

    public unsafe string AsString()
    {
        fixed (byte* ptr = Data)
            return Utf8StringMarshaller.ConvertToManaged(ptr)!;
    }

    public NetlinkAttributeCollection<TNestedAttr> AsNested<TNestedAttr>() where TNestedAttr : unmanaged, Enum => new(Data);
    public NetlinkAttributeCollection<TAttr> AsNested() => AsNested<TAttr>();
}