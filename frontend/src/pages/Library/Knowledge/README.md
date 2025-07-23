# 知识库选择器组件使用说明

## 概述

知识库管理组件 (`KnowledgeManager`) 已经被修改为支持两种模式：
1. **管理模式**：原有的知识库管理功能（默认模式）
2. **选择模式**：作为知识库选择器在弹出框中使用

## 组件Props

### KnowledgeManager

```typescript
interface KnowledgeManagerProps {
  selectMode?: boolean; // 是否为选择模式，默认false
  onSelect?: (knowledge: Knowledge) => void; // 选择知识库的回调函数
  hidePageContainer?: boolean; // 是否隐藏PageContainer包装，默认false
}
```

## 使用方式

### 1. 管理模式（默认）

```tsx
import KnowledgeManager from '@/pages/Library/Knowledge';

// 正常的知识库管理页面
<KnowledgeManager />
```

### 2. 选择模式

```tsx
import KnowledgeManager from '@/pages/Library/Knowledge';

// 作为选择器使用
<KnowledgeManager
  selectMode={true}
  onSelect={(knowledge) => {
    console.log('选中的知识库:', knowledge);
  }}
  hidePageContainer={true}
/>
```

### 3. 使用封装的选择器组件

我们提供了一个封装好的 `KnowledgeSelector` 组件：

```tsx
import KnowledgeSelector from '@/pages/Library/Knowledge/components/knowledge-selector';

const [selectorOpen, setSelectorOpen] = useState(false);

<KnowledgeSelector
  open={selectorOpen}
  onOpenChange={setSelectorOpen}
  onSelect={(knowledge) => {
    console.log('选中的知识库:', knowledge);
  }}
  title="选择知识库"
/>
```

## 功能差异

### 管理模式
- 显示完整的页面容器和面包屑
- 显示创建知识库按钮
- 行操作包含：编辑、删除
- 知识库名称可点击跳转到详情页
- 显示创建和编辑模态框

### 选择模式
- 隐藏页面容器（可选）
- 隐藏创建知识库按钮
- 行操作只显示：添加
- 知识库名称不可点击跳转
- 不显示创建和编辑模态框
- 点击"添加"按钮会触发 `onSelect` 回调

## 完整使用示例

参考 `@/pages/examples/knowledge-selector-example.tsx` 文件中的完整示例。

## 注意事项

1. 在选择模式下，确保传入 `onSelect` 回调函数
2. 如果在模态框中使用，建议设置 `hidePageContainer={true}`
3. 选择模式下不会显示创建和编辑功能，保持界面简洁
4. 组件会自动处理权限检查，只有有权限的用户才能看到相应的操作按钮