import EditModal from "@/pages/Model/components/edit-modal";

export default {
  agent: {
    lastEdit: '最近编辑',
    tag: '智能体',
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
      conversationTitle: '对话体验',
      prologue: '开场白文案',
      presetQuestions: '预设问题',
    },
  },
};
