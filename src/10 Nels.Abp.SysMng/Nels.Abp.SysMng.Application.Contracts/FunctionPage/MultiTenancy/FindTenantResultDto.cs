using System;

namespace Nels.Abp.SysMng.FunctionPage.MultiTenancy;

[Serializable]
public class FindTenantResultDto
{
    public bool Success { get; set; }

    public Guid? TenantId { get; set; }

    public string Name { get; set; }

    public bool IsActive { get; set; }
}
