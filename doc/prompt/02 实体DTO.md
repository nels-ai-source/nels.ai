#### 1.1 创建常量类和枚举

在 `src/40 Nels.Aigc/Nels.Aigc.Domain.Shared/Consts/` 目录下创建常量：
```csharp:src/40 Nels.Aigc/Nels.Aigc.Domain.Shared/Consts/YourEntityConsts.cs
public class YourEntityConsts
{
    public const int MaxNameLength = 100;
    public const int MaxDescriptionLength = 500;
}
```

#### 1.2 创建Domain层实体

在 `src/40 Nels.Aigc/Nels.Aigc.Domain/Entities/` 目录下创建实体类：

```csharp:src/40 Nels.Aigc/Nels.Aigc.Domain/Entities/YourEntity.cs
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Nels.Aigc.Entities;

public class YourEntity : AuditedEntity<Guid>, IAggregateRoot<Guid>
{
    public YourEntity() { }
    public YourEntity(Guid id) : base(id) { }

    [Required]
    [MaxLength(YourEntityConsts.MaxNameLength)]
    public virtual string Name { get; set; } = default!;

    [MaxLength(YourEntityConsts.MaxDescriptionLength)]
    public virtual string Description { get; set; } = default!;

    [Required]
    public virtual Guid SpaceId { get; set; } = default!;

    public virtual bool IsEnabled { get; set; } = true;

    // 导航属性
    public virtual List<YourChildEntity> Children { get; set; } = [];

    // 业务方法
    public void AddChild(Guid id, string name)
    {
        if (Children.Exists(x => x.Name == name))
        {
            return;
        }
        Children.Add(new YourChildEntity(id, this.Id, name));
    }

    public void RemoveChild(Guid childId)
    {
        var child = Children.Find(x => x.Id == childId);
        if (child is not null)
        {
            Children.Remove(child);
        }
    }
}

public class YourChildEntity : Entity<Guid>
{
    protected YourChildEntity() { }
    internal YourChildEntity(Guid id, Guid parentId, string name) : base(id)
    {
        ParentId = parentId;
        Name = name;
    }

    public virtual Guid ParentId { get; set; }

    [Required]
    [MaxLength(YourEntityConsts.MaxNameLength)]
    public virtual string Name { get; set; } = default!;
}
```

在 `src/40 Nels.Aigc/Nels.Aigc.EntityFrameworkCore/AigcDbContext.cs` 中添加实体上下文：

```csharp:src/40 Nels.Aigc/Nels.Aigc.EntityFrameworkCore/AigcDbContext.cs
// ... existing code ...
public DbSet<YourChildEntity> YourChildEntities { get; set; }
// ... existing code ...

在 `src/40 Nels.Aigc/Nels.Aigc.Application/YourEntityAppService.cs` 中添加应用服务：

```csharp:src/40 Nels.Aigc/Nels.Aigc.Application/YourEntityAppService.cs
// ... existing code ...
builder.Entity<YourEntity>(b=>)
{
    b.ToTable(AigcDbProperties.DbTablePrefix + nameof(Agent), SysMngDbProperties.DbSchema);
    // 如果有导航实体，根据导航属性添加导航关系例如：
    b.HasMany(x => x.Children).WithOne().HasForeignKey(x => x.YourEntityId);
    b.ConfigureByConvention();
}
// ... existing code ...
```

#### 1.2 创建DTO类

在 `src/40 Nels.Aigc/Nels.Aigc.Application.Contracts/Dtos/` 目录下创建DTO：

```csharp:src/40 Nels.Aigc/Nels.Aigc.Application.Contracts/Dtos/YourEntityDto.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Nels.Aigc.Dtos;

public class YourEntityDto : FullAuditedEntityDto<Guid>
{
    [Required]
    [StringLength(YourEntityConsts.MaxNameLength)]
    public virtual string Name { get; set; } = default!;

    [StringLength(YourEntityConsts.MaxDescriptionLength)]
    public virtual string Description { get; set; } = default!;

    public virtual List<YourChildEntityDto> Children { get; set; } = [];
}

public class YourChildEntityDto : EntityDto<Guid>
{
    public virtual Guid ParentId { get; set; }

    [Required]
    [StringLength(YourEntityConsts.MaxNameLength)]
    public virtual string Name { get; set; } = default!;
}


```

#### 1.3 添加映射

在 `src/40 Nels.Aigc/Nels.Aigc.Application/AigcApplicationAutoMapperProfile.cs` 中添加新的映射配置。
```csharp:40 Nels.Aigc/Nels.Aigc.Application/AigcApplicationAutoMapperProfile.cs
// ... existing code ...
 CreateMap<NewEntity, NewEntityDto>();
 CreateMap<NewEntityDto, NewEntity>();
// ... existing code ...
```
