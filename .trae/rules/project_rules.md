### 技术栈
- **后端**: ABP Framework + .NET Core + Entity Framework Core
- **前端**: React + TypeScript + Ant Design Pro + UmiJS
- **AI框架**: Semantic Kernel + Kernel Memory
- **数据库**: PostgreSQL (通过EF Core)

### 项目结构
├── src/                           # 后端代码
│   ├── 00 Nels.Abp/              # ABP基础框架扩展
│   ├── 10 Nels.Abp.SysMng/       # 系统管理模块
│   ├── 20 Nels.SemanticKernel/   # AI内核模块
│   ├── 40 Nels.Aigc/             # AIGC核心业务模块
│   └── Nels.HttpApi.Host/         # API主机项目
└── frontend/                      # 前端代码
    ├── src/
    │   ├── pages/                 # 页面组件
    │   ├── components/            # 通用组件
    │   ├── services/              # API服务
    │   └── types/                 # TypeScript类型定义
    └── config/                    # 配置文件