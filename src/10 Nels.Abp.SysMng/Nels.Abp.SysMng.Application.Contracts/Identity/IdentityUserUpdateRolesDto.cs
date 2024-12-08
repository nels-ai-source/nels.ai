using System.ComponentModel.DataAnnotations;

namespace Nels.Abp.SysMng.Identity;

public class IdentityUserUpdateRolesDto
{
    [Required]
    public string[]? RoleNames { get; set; }
}
