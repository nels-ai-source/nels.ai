# Agent 服务升级说明

本文档说明了如何使用升级后的 Agent 服务，该服务已从 XStream 迁移到 Ant Design X 的 useXAgent 和 XRequest 组件。

## 主要变更

### 1. 导入变更
```typescript
// 旧版本
import { XStream } from '@ant-design/x';

// 新版本
import { useXAgent, XRequest } from '@ant-design/x';
```

### 2. 核心功能升级

#### useXAgent Hook (推荐)
useXAgent 是 Ant Design X 提供的现代化 Agent 调度 Hook，支持：
- 模型调度和配置管理
- 自动请求重试
- 流式数据处理
- 动态配置变更

```typescript
import { useXAgent } from '@ant-design/x';
import { createXAgentConfig } from '@/services/aigc/agent';

const [agent] = useXAgent(createXAgentConfig(agentId, conversationId));

// 发送请求
const result = await agent.request({
  messages: [{ role: 'user', content: 'Hello' }],
  stream: true,
});
```

#### XRequest 类 (兼容性)
XRequest 提供了更标准化的请求处理：
- 符合 OpenAI 标准的 LLM 请求
- 自定义流数据转换
- 更好的错误处理

```typescript
import { invokeStreamingWithXRequest } from '@/services/aigc/agent';

await invokeStreamingWithXRequest(
  conversationId,
  agentId,
  message,
  cancellationToken,
  {
    onMessageDelta: (data) => console.log(data),
    onDone: () => console.log('完成'),
  }
);
```

## 新增功能

### 1. 文件管理
新增了完整的文件管理功能：

```typescript
import { readFile, writeFile, listFiles, deleteFile, createDirectory } from '@/services/aigc/agent';

// 读取文件
const content = await readFile('/path/to/file.txt');

// 写入文件
await writeFile('/path/to/file.txt', 'Hello World');

// 列出目录文件
const files = await listFiles('/path/to/directory');

// 删除文件
await deleteFile('/path/to/file.txt');

// 创建目录
await createDirectory('/path/to/new-directory');
```

### 2. 配置管理
提供了灵活的配置管理功能：

```typescript
import { createXAgentConfig } from '@/services/aigc/agent';

const config = createXAgentConfig(agentId, conversationId);
// 配置包含：
// - baseURL: API 基础地址
// - model: 模型标识
// - dangerouslyApiKey: 认证密钥
// - transformRequest: 自定义请求参数
// - transformStream: 自定义流数据转换
```

## 向后兼容性

为了确保现有代码的兼容性，我们保留了原有的函数名：

```typescript
// 这两个函数是等价的
export const invokeStreamingWithXStream = invokeStreamingWithXRequest;
```

## 迁移指南

### 从 XStream 迁移到 useXAgent

1. **组件级别使用 useXAgent Hook**：
```typescript
// 旧版本
const handleChat = async () => {
  const stream = await invokeStreamingAsync(conversationId, agentId, message);
  await parseSSEStreamWithXStream(stream, cancellationToken, callbacks);
};

// 新版本
const [agent] = useXAgent(createXAgentConfig(agentId, conversationId));
const handleChat = async () => {
  const result = await agent.request({
    messages: [{ role: 'user', content: message }],
    stream: true,
  });
  
  for await (const chunk of result) {
    // 处理响应
  }
};
```

2. **服务级别使用 XRequest**：
```typescript
// 旧版本
await invokeStreamingWithXStream(conversationId, agentId, message, cancellationToken, callbacks);

// 新版本 (函数名保持不变，内部实现已升级)
await invokeStreamingWithXRequest(conversationId, agentId, message, cancellationToken, callbacks);
```

## 优势

1. **更好的性能**：XRequest 和 useXAgent 提供了更高效的流处理
2. **标准化**：符合 OpenAI 标准，更好的兼容性
3. **可配置性**：支持动态配置变更和自定义转换
4. **错误处理**：更完善的错误处理机制
5. **文件管理**：新增完整的文件操作功能

## 示例代码

完整的使用示例请参考 `agent-usage-example.tsx` 文件。

## 注意事项

1. 确保 `@ant-design/x` 版本为 1.4.0 或更高
2. 新的 API 需要后端支持相应的文件管理接口
3. 配置中的 `dangerouslyApiKey` 需要正确的认证令牌
4. 流式响应的事件格式可能需要根据后端实际返回格式调整