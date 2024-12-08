using System.Runtime.InteropServices;

namespace OMP.LSWTSS.CApi1;

[StructLayout(LayoutKind.Sequential)]
public struct asCArray<T>
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Handle
    {
        public nint Ptr { get; set; }

        public static implicit operator nint(Handle handle) => handle.Ptr;

        public static explicit operator Handle(nint ptr) => new() { Ptr = ptr };

        public readonly asCArray<T> Get()
        {
            return Marshal.PtrToStructure<asCArray<T>>(Ptr)!;
        }
    }
    
    public required nint Array { get; set; }

    public required uint Length { get; set; }

    public readonly T this[uint index]
    {
        get
        {
            return Marshal.PtrToStructure<T>(Array + (int)(index * (uint)Marshal.SizeOf<T>()))!;
        }
    }
}