using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Nels.Abp.SysMng.FunctionPage;

public class BusinessUnitOutputDto : AuditedEntityDto<Guid>, IHasChildren<BusinessUnitOutputDto, Guid>
{
    public virtual Guid? ParentId { get; set; }
    public virtual string Name { get; set; } = string.Empty;
    public virtual string DisplayName { get; set; } = string.Empty;
    public virtual string Description { get; set; } = string.Empty;
    public virtual string Icon { get; set; } = string.Empty;
    public virtual bool IsEnable { get; set; } = true;
    public virtual string Sort { get; set; } = string.Empty;
    public List<BusinessUnitOutputDto>? Children { get; set; }
}
