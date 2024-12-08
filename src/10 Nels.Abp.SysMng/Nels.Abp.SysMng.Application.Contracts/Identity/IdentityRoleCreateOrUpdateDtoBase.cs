using Nels.Abp.SysMng.FunctionPage;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace Nels.Abp.SysMng.Identity;
public class IdentityRoleCreateOrUpdateDtoBase : ExtensibleObject
{
    [Required]
    [StringLength(AppConsts.MaxNameLength)]
    public string Name { get; set; } = string.Empty;

    public bool IsDefault { get; set; } = false;

    public bool IsPublic { get; set; } = false;

    protected IdentityRoleCreateOrUpdateDtoBase() : base(false)
    {

    }
}
