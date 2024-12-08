namespace OMP.LSWTSS.CApi1;

public interface INativeClassVtableMethodSchema : INativeClassMethodSchema
{
    public uint VtableIndex { get; set; }
}