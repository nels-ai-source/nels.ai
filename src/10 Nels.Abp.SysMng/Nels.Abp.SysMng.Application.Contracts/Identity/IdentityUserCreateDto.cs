using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Validation;

namespace Nels.Abp.SysMng.Identity;

public class IdentityUserCreateDto : IdentityUserCreateOrUpdateDtoBase
{
    [DisableAuditing]
    [Required]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    public string Password { get; set; } = string.Empty;
}
