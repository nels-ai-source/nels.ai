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
    model: '嵌入模型',
    import: '导入方式',
    placeholder: {
      name: '请输入名称',
      description: '请输入描述',
      model: '请选择嵌入模型',
    },
    required: {
      name: '名称不能为空',
      model: '嵌入模型不能为空',
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
};
