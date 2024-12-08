using Nels.Aigc.Consts;
using Nels.Aigc.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Nels.Aigc.Entities;

public class Space : AuditedEntity<Guid>, IAggregateRoot<Guid>
{
    public Space() { }
    public Space(Guid id) : base(id) { }

    [Required]
    public virtual SpaceType SpaceType { get; set; }

    [Required]
    [MaxLength(SpaceConsts.MaxNameLength)]
    public virtual string Name { get; set; } = string.Empty;

    [MaxLength(SpaceConsts.MaxDescriptionLength)]
    public virtual string Description { get; set; } = string.Empty;
    public virtual List<SpaceUser> SpaceUsers { get; set; } = [];
    public void AddSpaceUser(Guid id, Guid userId)
    {
        if (SpaceUsers.Any(x => x.UserId == userId)) return;
        SpaceUsers.Add(new SpaceUser(id, this.Id, userId));
    }
    public void RemoveSpaceUser(Guid userId)
    {
        var user = SpaceUsers.FirstOrDefault(x => x.UserId == userId);
        if (user == null) return;
        SpaceUsers.Remove(user);
    }
    public void AddSpaceOwnerUser(Guid id, Guid userId)
    {
        if (SpaceUsers.Any(x => x.UserId == userId)) return;
        SpaceUsers.Add(new SpaceUser(id, this.Id, userId, true));
    }
}
public class SpaceUser : AuditedEntity<Guid>
{
    protected SpaceUser() { }
    internal SpaceUser(Guid id, Guid spaceId, Guid userId, bool isOwner = false) : base(id)
    {
        SpaceId = spaceId;
        UserId = userId;
        IsOwner = isOwner;
    }

    [Required]
    public virtual Guid SpaceId { get; private set; }

    [Required]
    public virtual Guid UserId { get; private set; }

    [Required]
    public virtual bool IsOwner { get; private set; } = false;
}
