using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Nels.Abp.SysMng.FunctionPage;

public class BusinessUnit : AuditedEntity<Guid>, IAggregateRoot<Guid>, IHasOrderCode
{
    public BusinessUnit() { }
    public BusinessUnit(Guid id) : base(id) { }
    [Required]
    public virtual Guid ApplicationId { get; set; }

    [Required]
    [MaxLength(BusinessUnitConsts.MaxNameLength)]
    public virtual string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(BusinessUnitConsts.MaxDisplayNameLength)]
    public virtual string DisplayName { get; set; } = string.Empty;

    [MaxLength(BusinessUnitConsts.MaxIconLength)]
    public virtual string Icon {  get; set; }=string.Empty;


    [MaxLength(BusinessUnitConsts.MaxDescriptionLength)]
    public virtual string Description { get; set; } = string.Empty;

    [Required]
    public virtual bool IsEnable { get; set; } = true;

    [MaxLength(BusinessUnitConsts.MaxSortLength)]
    public virtual string Sort { get; set; } = string.Empty;
}
