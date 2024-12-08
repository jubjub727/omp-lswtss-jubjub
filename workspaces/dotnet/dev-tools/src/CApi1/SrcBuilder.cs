namespace OMP.LSWTSS.CApi1;

public class SrcBuilder
{
    private readonly System.Text.StringBuilder stringBuilder;

    public int Ident { get; set; }

    public SrcBuilder()
    {
        stringBuilder = new System.Text.StringBuilder();
    }

    public void Append()
    {
        stringBuilder.Append('\n');
    }

    public void Append(string? value)
    {
        if (value == null)
        {
            return;
        }

        foreach (var line in value.Split("\n"))
        {
            if (line != "")
            {
                stringBuilder.Append(new string(' ', Ident * 4));
                stringBuilder.Append(line);
            }
            stringBuilder.Append('\n');
        }
    }

    public override string ToString()
    {
        return stringBuilder.ToString();
    }
}