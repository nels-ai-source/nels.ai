using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Nels.Abp.SysMng.Identity;

public class IdentityUserDto : ExtensibleFullAuditedEntityDto<Guid>, IMultiTenant, IHasConcurrencyStamp
{
    public Guid? TenantId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Surname { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool EmailConfirmed { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;

    public bool PhoneNumberConfirmed { get; set; }

    public bool IsActive { get; set; }

    public bool LockoutEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public string ConcurrencyStamp { get; set; } = string.Empty;
}
