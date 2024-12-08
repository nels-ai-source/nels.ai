using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Nels.Abp.SysMng.FunctionPage;

public class App : AuditedEntity<Guid>, IAggregateRoot<Guid>, IHasOrderCode
{
    public App() { }
    public App(Guid id) : base(id) { }

    [Required]
    [MaxLength(AppConsts.MaxNameLength)]
    public virtual string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(AppConsts.MaxDisplayNameLength)]
    public virtual string DisplayName { get; set; } = string.Empty;

    [MaxLength(AppConsts.MaxIconLength)]
    public virtual string Icon { get; set; } = string.Empty;

    [MaxLength(AppConsts.MaxDescriptionLength)]
    public virtual string Description { get; set; } = string.Empty;

    [Required]
    public virtual bool IsEnable { get; set; } = true;

    [MaxLength(AppConsts.MaxSortLength)]
    public virtual string Sort { get; set; } = string.Empty;
}
