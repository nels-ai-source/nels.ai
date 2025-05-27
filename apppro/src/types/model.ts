import { FullAuditedEntity } from './entity';

export enum ModelProvider {
  OpenAI = 1,
  AzureOpenAI = 2,
  // Anthropic = 3,
  Google = 4,
  DashScope = 100,
  DeepSeek = 101,
}

export enum ModelType {
  TextGeneration = 1,
  Embedding = 2,
  MultiModal = 3,
}

export enum ModelConnector {
  OpenAI = 1,
  AzureOpenAI = 2,
  Google = 3,
  HuggingFace = 4,
  MistralAI = 5,
  Ollama = 6,
  Onnx = 7,
  Amazon = 8,
}

export enum ModelCapability {
  TextGeneration = 1,
  ImageComprehend = 2,
  AudioComprehend = 3,
  VideoComprehend = 4,
  Embedding = 5,
  Reasoning = 6,
  FunctionCall = 7,
  JsonOutput = 8,
}

export interface Model extends FullAuditedEntity {
  provider: ModelProvider;
  type: ModelType;
  connector: ModelConnector;
  name: string;
  endpoint: string;
  accessKey: string;
  secretKey: string;
  isEnabled: boolean;
  deploymentName: string;
  maxTokens: number;
  capabilities?: string;
  description?: string;
  modelCapabilities: ModelCapability[];
}
export interface ModelFilter {
  keyword?: string | null;
  provider?: ModelProvider | null;
  type?: ModelType | null;
  maxTokens?: number | null;
  modelCapabilities?: ModelCapability[] | null;
}
export interface ModelSetKey {
  provider?: ModelProvider;
  id?: string;
  accessKey: string;
  secretKey: string;
}

export const ModelInstanceConsts = {
  maxNameLength: 64,
  maxDescriptionLength: 512,
  maxEndpointLength: 256,
  maxAccessKeyLength: 128,
  maxSecretKeyLength: 128,
  maxDeploymentNameLength: 128,
  maxCapabilitiesLength: 256,
};

export const ProviderConfig = {
  [ModelProvider.OpenAI]: {
    endpoint: 'https://api.openai.com/v1',
    attributes: ['endpoint', 'accessKey', 'name'],
  },
  [ModelProvider.AzureOpenAI]: {
    endpoint: '',
    attributes: ['endpoint', 'accessKey', 'name', 'deploymentName'],
  },
  [ModelProvider.Google]: {
    endpoint: '',
    attributes: ['accessKey', 'name'],
  },
  [ModelProvider.DashScope]: {
    endpoint: 'https://dashscope.aliyuncs.com/compatible-mode/v1',
    attributes: ['endpoint', 'accessKey', 'name'],
  },
  [ModelProvider.DeepSeek]: {
    endpoint: 'https://api.deepseek.com/v1',
    attributes: ['endpoint', 'accessKey', 'name'],
  },
};
