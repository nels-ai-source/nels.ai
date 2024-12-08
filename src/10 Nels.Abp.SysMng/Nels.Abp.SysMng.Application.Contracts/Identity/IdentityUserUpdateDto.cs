using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Validation;

namespace Nels.Abp.SysMng.Identity;

public class IdentityUserUpdateDto : IdentityUserCreateOrUpdateDtoBase, IHasConcurrencyStamp
{
    [DisableAuditing]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    public string Password { get; set; } = string.Empty;

    public string ConcurrencyStamp { get; set; } = string.Empty;
}
