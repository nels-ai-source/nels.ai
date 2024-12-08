using System.Collections.Generic;

namespace Nels.Abp.SysMng.Permission;

public class PermissionGroupDto
{
    public string Name { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public List<PermissionGrantDto> Permissions { get; set; } = new List<PermissionGrantDto>();
}
