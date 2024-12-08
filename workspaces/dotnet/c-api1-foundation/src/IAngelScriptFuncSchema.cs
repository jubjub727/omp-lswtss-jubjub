namespace OMP.LSWTSS.CApi1;

public interface IAngelScriptFuncSchema : IFuncSchema
{
    public uint? SteamIndex { get; set; }

    public uint? EGSIndex { get; set; }
}