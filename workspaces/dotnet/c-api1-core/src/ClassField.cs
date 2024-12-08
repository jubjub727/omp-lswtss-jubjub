using System.Runtime.InteropServices;

namespace OMP.LSWTSS.CApi1;

public static class ClassField
{
    public static nint GetPointerValue(nint classPtr, int offset)
    {
        return Marshal.ReadIntPtr(nint.Add(classPtr, offset));
    }

    public static void SetPointerValue(nint classPtr, int offset, nint ptrValue)
    {
        Marshal.WriteIntPtr(nint.Add(classPtr, offset), ptrValue);
    }

    public static sbyte GetByteValue(nint classPtr, int offset)
    {
        return unchecked((sbyte)Marshal.ReadByte(nint.Add(classPtr, offset)));
    }

    public static void SetByteValue(nint classPtr, int offset, sbyte byteValue)
    {
        Marshal.WriteByte(nint.Add(classPtr, offset), unchecked((byte)byteValue));
    }

    public static byte GetUByteValue(nint classPtr, int offset)
    {
        return Marshal.ReadByte(nint.Add(classPtr, offset));
    }

    public static void SetUByteValue(nint classPtr, int offset, byte byteValue)
    {
        Marshal.WriteByte(nint.Add(classPtr, offset), byteValue);
    }

    public static short GetShortValue(nint classPtr, int offset)
    {
        return Marshal.ReadInt16(nint.Add(classPtr, offset));
    }

    public static void SetShortValue(nint classPtr, int offset, short shortValue)
    {
        Marshal.WriteInt16(nint.Add(classPtr, offset), shortValue);
    }

    public static ushort GetUShortValue(nint classPtr, int offset)
    {
        return unchecked((ushort)Marshal.ReadInt16(nint.Add(classPtr, offset)));
    }

    public static void SetUShortValue(nint classPtr, int offset, ushort shortValue)
    {
        Marshal.WriteInt16(nint.Add(classPtr, offset), unchecked((short)shortValue));
    }

    public static int GetIntValue(nint classPtr, int offset)
    {
        return Marshal.ReadInt32(nint.Add(classPtr, offset));
    }

    public static void SetIntValue(nint classPtr, int offset, int intValue)
    {
        Marshal.WriteInt32(nint.Add(classPtr, offset), intValue);
    }

    public static uint GetUIntValue(nint classPtr, int offset)
    {
        return unchecked((uint)Marshal.ReadInt32(nint.Add(classPtr, offset)));
    }

    public static void SetUIntValue(nint classPtr, int offset, uint intValue)
    {
        Marshal.WriteInt32(nint.Add(classPtr, offset), unchecked((int)intValue));
    }

    public static long GetLongValue(nint classPtr, int offset)
    {
        return Marshal.ReadInt64(nint.Add(classPtr, offset));
    }

    public static void SetLongValue(nint classPtr, int offset, long longValue)
    {
        Marshal.WriteInt64(nint.Add(classPtr, offset), longValue);
    }

    public static ulong GetULongValue(nint classPtr, int offset)
    {
        return unchecked((ulong)Marshal.ReadInt64(nint.Add(classPtr, offset)));
    }

    public static void SetULongValue(nint classPtr, int offset, ulong longValue)
    {
        Marshal.WriteInt64(nint.Add(classPtr, offset), unchecked((long)longValue));
    }
}