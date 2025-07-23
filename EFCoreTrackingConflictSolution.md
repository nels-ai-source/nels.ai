# EF Core 实体跟踪冲突解决方案

## 问题分析

遇到的错误：`System.InvalidOperationException: The instance of entity type 'AgentPresetQuestions' cannot be tracked because another instance with the same key value for {'Id'} is already being tracked.`

### 根本原因
1. **EF Core 变更跟踪器限制**：同一个 DbContext 中不能同时跟踪具有相同主键的多个实体实例
2. **实体状态混乱**：在差异化更新过程中，实体可能同时存在于跟踪器和新的集合中
3. **Repository 模式的复杂性**：ABP Framework 的 Repository 可能会自动跟踪某些查询结果

## 解决方案对比

### 方案一：优化的先删后插（当前采用）✅

```csharp
[UnitOfWork]
protected override async Task<Agent> UpdateAsync(Agent entity)
{
    // 并行删除
    var deleteTasks = new[]
    {
        presetQuestionsRepository.DeleteAsync(x => x.AgentId == entity.Id),
        agentKnowledgeRepository.DeleteAsync(x => x.AgentId == entity.Id),
        agentToolRepository.DeleteAsync(x => x.AgentId == entity.Id)
    };
    await Task.WhenAll(deleteTasks);

    // 并行插入
    var insertTasks = new List<Task>();
    if (entity.Questions.Any())
        insertTasks.Add(presetQuestionsRepository.InsertManyAsync(entity.Questions));
    if (entity.Knowledges.Any())
        insertTasks.Add(agentKnowledgeRepository.InsertManyAsync(entity.Knowledges));
    if (entity.Tools.Any())
        insertTasks.Add(agentToolRepository.InsertManyAsync(entity.Tools));
    
    if (insertTasks.Any())
        await Task.WhenAll(insertTasks);

    return await base.UpdateAsync(entity);
}
```

**优势：**
- ✅ 避免实体跟踪冲突
- ✅ 逻辑简单清晰
- ✅ 使用并行操作提高性能
- ✅ 事务安全

**劣势：**
- ❌ 对于大量数据可能性能不是最优
- ❌ 会触发所有相关实体的删除/插入事件

### 方案二：使用 DetachAll + 差异化更新

```csharp
private async Task UpdateCollectionWithDetachAsync<TEntity>(
    IRepository<TEntity, Guid> repository,
    Expression<Func<TEntity, bool>> filterExpression,
    ICollection<TEntity> newItems)
    where TEntity : class, IEntity<Guid>
{
    // 清除所有跟踪的实体
    var context = await repository.GetDbContextAsync();
    var trackedEntities = context.ChangeTracker.Entries<TEntity>()
        .Where(e => e.State != EntityState.Detached)
        .ToList();
    
    foreach (var entry in trackedEntities)
    {
        entry.State = EntityState.Detached;
    }

    // 执行差异化更新
    var existingItems = await repository.GetListAsync(filterExpression);
    // ... 差异化逻辑
}
```

### 方案三：使用独立的 DbContext

```csharp
private async Task UpdateCollectionWithSeparateContextAsync<TEntity>(
    ICollection<TEntity> newItems,
    Guid agentId)
    where TEntity : class, IEntity<Guid>
{
    using var scope = ServiceProvider.CreateScope();
    var separateRepository = scope.ServiceProvider
        .GetRequiredService<IRepository<TEntity, Guid>>();
    
    // 在独立的上下文中执行操作
    await separateRepository.DeleteAsync(x => x.AgentId == agentId);
    if (newItems.Any())
    {
        await separateRepository.InsertManyAsync(newItems);
    }
}
```

## 最佳实践建议

### 1. 代码质量改进

```csharp
// 添加日志记录
protected override async Task<Agent> UpdateAsync(Agent entity)
{
    Logger.LogDebug("开始更新 Agent {AgentId} 的关联数据", entity.Id);
    
    try
    {
        // 删除操作
        var deleteStopwatch = Stopwatch.StartNew();
        var deleteTasks = new[]
        {
            presetQuestionsRepository.DeleteAsync(x => x.AgentId == entity.Id),
            agentKnowledgeRepository.DeleteAsync(x => x.AgentId == entity.Id),
            agentToolRepository.DeleteAsync(x => x.AgentId == entity.Id)
        };
        await Task.WhenAll(deleteTasks);
        deleteStopwatch.Stop();
        
        Logger.LogDebug("删除操作完成，耗时 {ElapsedMs}ms", deleteStopwatch.ElapsedMilliseconds);
        
        // 插入操作
        var insertStopwatch = Stopwatch.StartNew();
        var insertTasks = new List<Task>();
        
        if (entity.Questions.Any())
        {
            insertTasks.Add(presetQuestionsRepository.InsertManyAsync(entity.Questions));
            Logger.LogDebug("准备插入 {Count} 个预设问题", entity.Questions.Count);
        }
        
        if (entity.Knowledges.Any())
        {
            insertTasks.Add(agentKnowledgeRepository.InsertManyAsync(entity.Knowledges));
            Logger.LogDebug("准备插入 {Count} 个知识库关联", entity.Knowledges.Count);
        }
        
        if (entity.Tools.Any())
        {
            insertTasks.Add(agentToolRepository.InsertManyAsync(entity.Tools));
            Logger.LogDebug("准备插入 {Count} 个工具关联", entity.Tools.Count);
        }
        
        if (insertTasks.Any())
        {
            await Task.WhenAll(insertTasks);
        }
        insertStopwatch.Stop();
        
        Logger.LogDebug("插入操作完成，耗时 {ElapsedMs}ms", insertStopwatch.ElapsedMilliseconds);
        
        return await base.UpdateAsync(entity);
    }
    catch (Exception ex)
    {
        Logger.LogError(ex, "更新 Agent {AgentId} 关联数据时发生错误", entity.Id);
        throw;
    }
}
```

### 2. 性能监控

```csharp
// 添加性能计数器
private static readonly Counter<int> _updateOperationCounter = 
    Meter.CreateCounter<int>("agent_update_operations");

private static readonly Histogram<double> _updateDurationHistogram = 
    Meter.CreateHistogram<double>("agent_update_duration_ms");
```

### 3. 异常处理增强

```csharp
// 添加重试机制
[UnitOfWork]
protected override async Task<Agent> UpdateAsync(Agent entity)
{
    const int maxRetries = 3;
    var retryCount = 0;
    
    while (retryCount < maxRetries)
    {
        try
        {
            return await UpdateAsyncInternal(entity);
        }
        catch (DbUpdateConcurrencyException) when (retryCount < maxRetries - 1)
        {
            retryCount++;
            Logger.LogWarning("并发更新冲突，正在重试 ({RetryCount}/{MaxRetries})", 
                retryCount, maxRetries);
            await Task.Delay(TimeSpan.FromMilliseconds(100 * retryCount));
        }
    }
    
    throw new AbpException("更新操作在多次重试后仍然失败");
}
```

### 4. 单元测试建议

```csharp
[Fact]
public async Task UpdateAsync_ShouldHandleEntityTrackingConflicts()
{
    // Arrange
    var agent = await CreateTestAgentAsync();
    var updateDto = new UpdateAgentDto
    {
        Questions = new List<AgentPresetQuestionsDto>
        {
            new() { Id = Guid.NewGuid(), Content = "Test Question" }
        }
    };

    // Act & Assert
    var exception = await Record.ExceptionAsync(async () =>
    {
        await AgentAppService.UpdateAsync(agent.Id, updateDto);
    });

    exception.ShouldBeNull();
}
```

## 总结

当前采用的优化先删后插方案是最适合的解决方案，因为：

1. **可靠性高**：完全避免了 EF Core 跟踪冲突
2. **性能优化**：使用并行操作减少总体执行时间
3. **维护性好**：逻辑简单，易于理解和维护
4. **事务安全**：在 UnitOfWork 保护下确保数据一致性

虽然在理论上差异化更新可能更高效，但在实际项目中，简单可靠的方案往往更有价值。