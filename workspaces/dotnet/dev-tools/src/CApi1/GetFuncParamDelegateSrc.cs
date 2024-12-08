using System;
using System.Collections.Generic;

namespace OMP.LSWTSS.CApi1;

public static class GetFuncParamDelegateSrc
{
    public static string Execute(IFuncParamSchema funcParamSchema)
    {
        var funcParamTypeSrc = GetTypeSrc.Execute(funcParamSchema.Type);

        var funcParamTypeAttributeSrcs = new List<string>();

        var funcParamTypeMarshalAsAttributeSrc = GetTypeMarshalAsAttributeSrc.Execute(funcParamSchema.Type);

        if (funcParamTypeMarshalAsAttributeSrc != null)
        {
            funcParamTypeAttributeSrcs.Add(funcParamTypeMarshalAsAttributeSrc);
        }

        if (funcParamSchema.IsOutRef)
        {
            funcParamTypeAttributeSrcs.Add("System.Runtime.InteropServices.Out");
        }
        else if (funcParamSchema.IsInRef)
        {
            funcParamTypeAttributeSrcs.Add("System.Runtime.InteropServices.In");
        }

        var funcParamModifierSrc = GetFuncParamModifierSrc.Execute(funcParamSchema);

        var funcParamDelegateSrc = "";

        if (funcParamTypeAttributeSrcs.Count > 0)
        {
            funcParamDelegateSrc += $"[param: {string.Join(", ", funcParamTypeAttributeSrcs)}]\n";
        }

        if (funcParamModifierSrc != null)
        {
            funcParamDelegateSrc += $"{funcParamModifierSrc} ";
        }

        funcParamDelegateSrc += $"{funcParamTypeSrc} ";

        funcParamDelegateSrc += funcParamSchema.Name;

        return funcParamDelegateSrc;
    }
}