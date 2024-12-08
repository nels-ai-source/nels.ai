using Volo.Abp.Domain.Entities;

namespace Nels.Abp.SysMng.Identity;

public class IdentityRoleUpdateDto : IdentityRoleCreateOrUpdateDtoBase, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; } = string.Empty;
}
