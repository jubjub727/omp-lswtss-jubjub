using System.Collections.Generic;

namespace OMP.LSWTSS.CApi1;

public interface IAngelScriptClassSchema : IClassSchema
{
    public uint? SteamIndex { get; set; }

    public uint? EGSIndex { get; set; }

    public List<IAngelScriptClassMethodSchema> Methods { get; set; }
}