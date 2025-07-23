# Agent 更新策略 - 优雅实现方案

## 当前实现：差异化更新（推荐）

已在 `AgentAppService.cs` 中实现，具有以下优势：
- ✅ 只对真正变化的数据进行操作
- ✅ 减少数据库操作次数
- ✅ 保持数据完整性
- ✅ 通用性强，可复用

## 方案二：使用 EF Core 的自动变更跟踪

```csharp
[UnitOfWork]
protected override async Task<Agent> UpdateAsync(Agent entity)
{
    // 获取现有实体（带跟踪）
    var existingAgent = await Repository.GetAsync(entity.Id, includeDetails: true);
    
    // 使用 EF Core 的集合导航属性自动处理
    existingAgent.Questions.Clear();
    foreach (var question in entity.Questions)
    {
        existingAgent.Questions.Add(question);
    }
    
    existingAgent.Knowledges.Clear();
    foreach (var knowledge in entity.Knowledges)
    {
        existingAgent.Knowledges.Add(knowledge);
    }
    
    existingAgent.Tools.Clear();
    foreach (var tool in entity.Tools)
    {
        existingAgent.Tools.Add(tool);
    }
    
    // EF Core 会自动处理变更跟踪
    return await Repository.UpdateAsync(existingAgent);
}
```

**优势：**
- 利用 EF Core 的变更跟踪机制
- 代码简洁
- 自动处理关系

**劣势：**
- 需要配置正确的导航属性
- 可能产生额外的查询

## 方案三：使用 Merge 模式

```csharp
private async Task MergeCollectionAsync<TEntity>(
    IRepository<TEntity, Guid> repository,
    Expression<Func<TEntity, bool>> filterExpression,
    ICollection<TEntity> newItems,
    Func<TEntity, TEntity, bool> matchPredicate,
    Action<TEntity, TEntity> updateAction)
    where TEntity : class, IEntity<Guid>
{
    var existingItems = await repository.GetListAsync(filterExpression);
    var existingDict = existingItems.ToDictionary(x => x.Id);
    var newDict = newItems.ToDictionary(x => x.Id);
    
    // 使用 MERGE 逻辑
    var operations = new List<Task>();
    
    // 删除不存在的
    var toDelete = existingDict.Keys.Except(newDict.Keys);
    if (toDelete.Any())
    {
        operations.Add(repository.DeleteManyAsync(
            existingItems.Where(x => toDelete.Contains(x.Id))));
    }
    
    // 添加新的
    var toAdd = newDict.Keys.Except(existingDict.Keys);
    if (toAdd.Any())
    {
        operations.Add(repository.InsertManyAsync(
            newItems.Where(x => toAdd.Contains(x.Id))));
    }
    
    // 更新现有的
    var toUpdate = existingDict.Keys.Intersect(newDict.Keys);
    foreach (var id in toUpdate)
    {
        updateAction(existingDict[id], newDict[id]);
    }
    if (toUpdate.Any())
    {
        operations.Add(repository.UpdateManyAsync(
            existingItems.Where(x => toUpdate.Contains(x.Id))));
    }
    
    await Task.WhenAll(operations);
}
```

## 方案四：使用 Repository 模式扩展

```csharp
public interface IAgentRepository : IRepository<Agent, Guid>
{
    Task SyncQuestionsAsync(Guid agentId, IEnumerable<AgentPresetQuestions> questions);
    Task SyncKnowledgesAsync(Guid agentId, IEnumerable<AgentKnowledge> knowledges);
    Task SyncToolsAsync(Guid agentId, IEnumerable<AgentTool> tools);
}

// 在 AgentAppService 中使用
[UnitOfWork]
protected override async Task<Agent> UpdateAsync(Agent entity)
{
    var agentRepo = (IAgentRepository)Repository;
    
    await Task.WhenAll(
        agentRepo.SyncQuestionsAsync(entity.Id, entity.Questions),
        agentRepo.SyncKnowledgesAsync(entity.Id, entity.Knowledges),
        agentRepo.SyncToolsAsync(entity.Id, entity.Tools)
    );
    
    return await base.UpdateAsync(entity);
}
```

## 方案五：使用事件驱动模式

```csharp
// 定义领域事件
public class AgentKnowledgesChangedEvent : DomainEvent
{
    public Guid AgentId { get; set; }
    public IEnumerable<AgentKnowledge> NewKnowledges { get; set; }
}

// 在 Agent 实体中
public void UpdateKnowledges(IEnumerable<AgentKnowledge> newKnowledges)
{
    // 业务逻辑验证
    
    // 发布事件
    AddDomainEvent(new AgentKnowledgesChangedEvent 
    { 
        AgentId = Id, 
        NewKnowledges = newKnowledges 
    });
}

// 事件处理器
public class AgentKnowledgesChangedEventHandler : 
    ILocalEventHandler<AgentKnowledgesChangedEvent>
{
    public async Task HandleEventAsync(AgentKnowledgesChangedEvent eventData)
    {
        // 处理知识库同步逻辑
    }
}
```

## 性能对比

| 方案 | 数据库操作次数 | 内存使用 | 复杂度 | 推荐度 |
|------|---------------|----------|--------|--------|
| 先删后插 | 高 | 低 | 低 | ⭐⭐ |
| 差异化更新 | 中 | 中 | 中 | ⭐⭐⭐⭐⭐ |
| EF 变更跟踪 | 中 | 高 | 低 | ⭐⭐⭐ |
| Merge 模式 | 低 | 中 | 高 | ⭐⭐⭐⭐ |
| Repository 扩展 | 低 | 低 | 中 | ⭐⭐⭐⭐ |
| 事件驱动 | 中 | 中 | 高 | ⭐⭐⭐ |

## 建议

1. **当前项目**：使用已实现的差异化更新方案，平衡了性能和复杂度
2. **大型项目**：考虑 Repository 扩展模式，提供更好的封装
3. **高并发场景**：考虑 Merge 模式，减少数据库锁定时间
4. **复杂业务逻辑**：考虑事件驱动模式，提供更好的解耦