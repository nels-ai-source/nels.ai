export default {
  agent: {
    lastEdit: '最近编辑',
    tag: '智能体',
    // 搜索和筛选相关
    search: {
      placeholder: '搜索智能体',
      typeSelectPlaceholder: '选择类型',
      allTypes: '全部',
      failedToFetch: '获取智能体列表失败',
    },
    // 智能体类型
    types: {
      chatCompletion: '单 Agent（自主规划模式）',
      workflow: '单 Agent（对话流模式）',
      multiAgent: '多 Agents',
    },
    actions: {
      duplicate: '复制',
      delete: '删除',
      confirm: '确认',
      cancel: '取消',
      create: '创建',
    },
    deleteConfirm: {
      title: '确定要删除这个智能体吗？',
      content: '此操作无法撤销。',
    },
    createModal: {
      title: '创建智能体',
      nameLabel: '智能体名称',
      nameRequired: '请输入智能体名称！',
      descriptionLabel: '描述',
      iconLabel: '图标',
      iconRequired: '请上传智能体图标！',
    },
    editModal: {
      title: '编辑智能体',
    },
    detail: {
      promptTitle: '人设与回复逻辑',
      optimizePrompt: '自动优化提示词',
      promptPlaceholder: '请输入提示词...',
      activeLinePlaceholder: '在此处输入...',
      conversationTitle: '对话体验',
      prologue: '开场白文案',
      presetQuestions: '预设问题',
      save: '保存',
      exampleName: '张三',
      exampleLocation: '北京',
      editorContentUpdated: '编辑器内容已更新:',
      optimizePromptLog: '优化提示词',
      previewAndDebug: '预览与调试',
      // 侧边栏相关文本
      sidebar: {
        orchestration: '编排',
        skills: '技能',
        knowledge: '知识',
        plugin: '插件',
        text: '文本',
        autoCall: '自动调用',
      },
      // 技能组件相关文本
      skillComponent: {
        emptyText: '插件能够让智能体调用外部 API，例如搜索信息、浏览网页、生成图片等，扩展智能体的能力和使用场景。',
      },
      // 知识组件相关文本
      knowledgeComponent: {
        emptyText: '将文档、URL、三方数据源上传为文本知识库后，用户发送消息时，智能体能够引用文本知识中的内容回答用户问题。',
        alreadyAdded: '该知识库已经添加过了',
        addSuccess: '已添加知识库',
        removeSuccess: '已移除知识库',
        copySuccess: '知识库名称已复制到剪贴板',
        copyFailed: '复制失败',
        noDescription: '暂无描述',
        copyTooltip: '复制知识库名称',
        removeTooltip: '移除知识库',
        selectTitle: '选择知识库',
        knowledgeAlt: '知识库',
      },
    },
  },
};
