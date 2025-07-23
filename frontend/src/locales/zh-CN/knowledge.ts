export default {
  knowledge: {
    name: '名称',
    description: '描述',
    documentCount: '文档数量',
    length: '字符数',
    retrievalCount: '调用次数',
    creationTime: '创建时间',
    status: '状态',
    format: '格式类型',
    embeddingModel: '嵌入模型',
    import: '导入方式',
    agentType: 'Agent 类型',
    filterByAgentType: '按 Agent 类型筛选',
    placeholder: {
      name: '请输入名称',
      description: '请输入描述',
      model: '请选择嵌入模型',
    },
    required: {
      name: '名称不能为空',
      model: '嵌入模型不能为空',
      embeddingModel: '嵌入模型不能为空'
    },

    unit: {
      count: '个文档',
      char: '字符',
      times: '命中',
    },
    create: {
      title: '创建知识库',
    },
    edit: {
      title: '编辑知识库',
    },
    detail: {
      addDocument: '添加内容',
      deleteDocument: '删除文档',
      updateSettings: '查看或调整配置',
      updateParagraph: '编辑段落',
      deleteParagraph: '删除段落',
      settingParagraph: '段落设置',
    },
    document: {
      title: '编辑文档',
      name: '文档名称',
      placeholder: {
        name: '请输入文档名称',
      },
      required: {
        name: '文档名称不能为空',
      },
    },
    operation: {
      create: '创建知识库',
      edit: '编辑知识库',
      delete: '删除知识库',
    },

    selector: {
      title: '选择知识库',
      add: '添加',
      remove: '移除',
    },

    formatType: {
      text: {
        title: '文本格式',
        description: '支持文本类型的知识库创建',
      },
      table: {
        title: '表格格式',
        description: '支持表格类型的知识库创建',
      },
      image: {
        title: '照片类型',
        description: '支持图片类型的知识库创建',
      },
    },

    importType: {
      label: '导入类型',
      local: {
        title: '本地文档',
        description: '支持本地文档类型的知识库创建',
      },
      online: {
        title: '在线数据',
        description: '支持在线数据类型的知识库创建',
      },
    },

    upload: {
      title: '添加文档',
      step: {
        file: '上传文件',
        settings: '分段设置',
      },
      file: {
        required: '请上传文件',
        dragText: '点击上传或拖拽文档到这里',
        hint: '支持 PDF、TXT、DOC、DOCX、MD，最多可上传 300 个文件，每个文件不超过 100MB， PDF 最多 500 页',
      },
      parse: {
        title: '文档解析策略',
        required: '请选择文档解析策略',
        accurate: {
          title: '精准解析',
          description: '将从文档中提取图片、表格等元素，需要耗费更长的时间',
        },
        fast: {
          title: '快速解析',
          description: '不会对文档提取图像、表格等元素，适用于纯文本',
        },
      },
      segment: {
        title: '分段策略',
        required: '请选择分段策略',
        auto: {
          title: '自动分段与清晰',
          description: '自动分段与预处理规则',
        },
        custom: {
          title: '自定义',
          description: '自定义分段规则、分段长度及预处理规则',
        },
        hierarchy: {
          title: '按层级分段',
          description: '按照文档层级结构分段，将文档转化为有层级信息的树结构',
        },
      },
    },
  },
  knowledgeSettings: {
    title: '知识库设置',
    sections: {
      recall: '召回',
      reply: '回复',
      source: '来源',
    },
    labels: {
      invokeMethod: '调用方式',
      searchStrategy: '搜索策略',
      maxRecallCount: '最大召回数量',
      minMatchScore: '最小匹配度',
      replyMode: '回复模式',
      customReply: '自定义回复',
      showSource: '显示来源',
      sourceDisplayMode: '展示方式',
      autoInvoke: '自动调用',
      manualInvoke: '按需调用',
      hybrid: '混合',
      semantic: '语义',
      fulltext: '全文',
      defaultReply: '默认',
      customReplyMode: '自定义',
      cardMode: '卡片',
      textMode: '文本内容',
    },
    tooltips: {
      invokeMethod: '选择是否每轮对话自动召回或按需从特定知识库召回',
      searchStrategy: '从知识库中获取知识的检索方式，不同的检索策略可以更有效地找到正确的信息，提高其生成的答案的准确性和可用性。混合：同时使用语义和全文搜索；语义：使用语义向量搜索；全文：使用全文关键词搜索',
      maxRecallCount: '从知识库中返回给大模型的最大段落数，数值越大返回的内容越多',
      minMatchScore: '根据设置的匹配度选取段落返回给大模型，低于设置匹配度的内容不会被召回',
      replyMode: '选择回复模式，默认回复或自定义回复内容。默认：使用系统默认的回复方式；自定义：使用自定义的回复内容',
      customReply: '当无法从知识库中找到相关内容时的自定义回复',
      showSource: '是否在回复中显示知识来源信息',
      sourceDisplayMode: '选择知识来源的展示方式。卡片：以卡片形式展示知识来源；文本内容：以文本内容形式展示知识来源',
    },
    placeholders: {
      customReply: '抱歉，您的回复超出了我的知识范围，并且无法在当前阶段回答',
    },
    marks: {
      maxRecallCount: {
        default: '默认',
      },
      minMatchScore: {
        default: '默认',
      },
    },
  },
};
