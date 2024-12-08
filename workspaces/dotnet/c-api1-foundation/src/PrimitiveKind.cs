using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OMP.LSWTSS.CApi1;

[JsonConverter(typeof(StringEnumConverter))]
public enum PrimitiveKind
{
    Pointer,
    Bool,
    Byte,
    UByte,
    Short,
    UShort,
    Int,
    UInt,
    Long,
    ULong,
    Float,
    Double,
}