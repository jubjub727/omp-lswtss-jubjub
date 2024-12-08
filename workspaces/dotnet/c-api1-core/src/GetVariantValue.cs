using OMP.LSWTSS.CApi1;

public static class GetVariantValue
{
    public static uint Execute(uint steamValue, uint egsValue)
    {
        switch (GetVariant.Execute())
        {
            case Variant.Steam:
                return steamValue;
            case Variant.EGS:
                return egsValue;
            default:
                throw new System.InvalidOperationException();
        }
    }
}