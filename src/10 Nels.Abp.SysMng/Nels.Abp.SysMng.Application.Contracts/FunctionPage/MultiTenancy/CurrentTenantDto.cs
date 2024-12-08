using System;

namespace Nels.Abp.SysMng.FunctionPage.MultiTenancy;

public class CurrentTenantDto
{
    public Guid? Id { get; set; }

    public string Name { get; set; }

    public bool IsAvailable { get; set; }
}
