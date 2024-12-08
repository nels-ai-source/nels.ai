using JetBrains.Annotations;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Nels.Abp.SysMng.Identity;

public abstract class IdentityUserCreateOrUpdateDtoBase : ExtensibleObject
{
    [Required]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
    public string UserName { get; set; } = string.Empty;

    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
    public string Name { get; set; } = string.Empty;

    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxSurnameLength))]
    public string? Surname { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
    public string Email { get; set; } = string.Empty;

    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
    public string? PhoneNumber { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public bool LockoutEnabled { get; set; }

    [CanBeNull]
    public string[]? RoleNames { get; set; }

    protected IdentityUserCreateOrUpdateDtoBase() : base(false)
    {

    }
}
