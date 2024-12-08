using System.Collections.Generic;

namespace Nels.Abp.SysMng.Permission;

public class PermissionGrantDto
{
    public string Name { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string? ParentName { get; set; }

    public bool IsGranted { get; set; }

    public List<PermissionGrantDto>? Children { get; set; }
}
