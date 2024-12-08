namespace OMP.LSWTSS.CApi1;

public interface IPrimitiveTypeSchema : ITypeSchema
{
    public PrimitiveKind PrimitiveKind { get; set; }
}