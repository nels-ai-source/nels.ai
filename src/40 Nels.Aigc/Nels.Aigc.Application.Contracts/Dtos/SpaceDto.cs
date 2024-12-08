using Nels.Aigc.Consts;
using Nels.Aigc.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Nels.Aigc.Dtos;

public class SpaceDto : AuditedEntityDto<Guid>
{

    [Required]
    public virtual SpaceType SpaceType { get; set; }

    [Required]
    [StringLength(SpaceConsts.MaxNameLength)]
    public virtual string Name { get; set; } = string.Empty;

    [StringLength(SpaceConsts.MaxDescriptionLength)]
    public virtual string Description { get; set; } = string.Empty;
    public virtual List<SpaceUserDto> SpaceUsers { get; set; }
}
public class SpaceUserDto : AuditedEntityDto<Guid>
{

    [Required]
    public virtual Guid SpaceId { get; private set; }

    [Required]
    public virtual Guid UserId { get; private set; }

    [Required]
    public virtual bool IsOwner { get; private set; } = false;
}
