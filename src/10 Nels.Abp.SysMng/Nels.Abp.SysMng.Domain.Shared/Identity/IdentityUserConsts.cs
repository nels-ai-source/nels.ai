namespace Nels.Abp.SysMng.Identity;

public static class IdentityUserConsts
{

    public static int MaxUserNameLength { get; set; } = 256;

    public static int MaxNameLength { get; set; } = 64;

    public static int MaxSurnameLength { get; set; } = 64;

    public static int MaxNormalizedUserNameLength { get; set; } = MaxUserNameLength;

    public static int MaxEmailLength { get; set; } = 256;

    public static int MaxNormalizedEmailLength { get; set; } = MaxEmailLength;

    public static int MaxPhoneNumberLength { get; set; } = 16;

    /// <summary>
    /// Default value: 128
    /// </summary>
    public static int MaxPasswordLength { get; set; } = 128;

    /// <summary>
    /// Default value: 256
    /// </summary>
    public static int MaxPasswordHashLength { get; set; } = 256;

    /// <summary>
    /// Default value: 256
    /// </summary>
    public static int MaxSecurityStampLength { get; set; } = 256;

    /// <summary>
    /// Default value: 16
    /// </summary>
    public static int MaxLoginProviderLength { get; set; } = 16;
}

