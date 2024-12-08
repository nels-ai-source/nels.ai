namespace Nels.Abp.SysMng;

public static class SysMngDbProperties
{
    public static string DbTablePrefix { get; set; } = "sys_";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "SysMng";
}
