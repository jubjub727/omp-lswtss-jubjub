using System.Collections.Generic;

namespace OMP.LSWTSS.CApi1;

public interface INativeClassSchema : IClassSchema
{
    public List<INativeClassMethodSchema> Methods { get; set; }
}