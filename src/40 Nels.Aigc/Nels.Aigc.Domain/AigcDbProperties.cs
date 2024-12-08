namespace Nels.Aigc;

public static class AigcDbProperties
{
    public static string DbTablePrefix { get; set; } = "ai_";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Default";
}
