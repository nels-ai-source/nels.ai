// @ts-ignore
/* eslint-disable */

declare namespace API {
  type CurrentUser = {
    name?: string;
    avatar?: string;
    id?: string;
    email?: string;
    emailVerified?: boolean;
    userName?: string;
    surName?: string;
    phoneNumber?: string;
    phoneNumberVerified?: boolean;
    tenantId?: string;
    roles?: string[];
    isAuthenticated?: boolean;
    permissions?: string[];
  };

  type LoginResult = {
    access_token?: string;
    token_type?: string;
    expires_in?: int;
  };

  type PageParams = {
    current?: number;
    pageSize?: number;
  };

  type LoginParams = {
    username?: string;
    password?: string;
    client_id?: string;
    grant_type?: string;
    scope?: string;
    client_secret?: string;
  };

  type ErrorResponse = {
    /** 业务约定的错误码 */
    errorCode: string;
    /** 业务上的错误信息 */
    errorMessage?: string;
    /** 业务上的请求是否成功 */
    success?: boolean;
  };

  type KnowledgeItem = {
    id: string;
    creationTime: string;
    creatorId: string;
    lastModificationTime: string;
    lastModifierId: string;
    spaceId: string;
    name: string;
    description: string;
    documentCount: number;
    length: number;
    agentUseCount: number;
    retrievalCount: number;
    modelId: string;
    searchType: number;
    isEnabled: boolean;
    knowledgeDocuments?: KnowledgeDocument[];
  };
  type KnowledgeDocumentParagraph = {
    id: string;
    creationTime: string;
    creatorId: string;
    lastModificationTime: string;
    lastModifierId: string;
    knowledgeId: string;
    knowledgeDocumentId: string;
    index: number;
    content: string;
    isEnabled: boolean;
    retrievalCount: number;
    length: number;
    embedding: boolean;
  };

  type KnowledgeDocument = {
    id: string;
    creationTime: string;
    creatorId: string;
    lastModificationTime: string;
    lastModifierId: string;
    knowledgeId: string;
    fileId: string;
    name: string;
    documentType: number;
    length: number;
    retrievalCount: number;
    paragraphCount: number;
    isEnabled: boolean;
    knowledgeDocumentParagraphs?: KnowledgeDocumentParagraph[];
  };

  type ModelItem = {
    id: string;
    name: string;
    deploymentName: string;
    provider: number;
    providerName: string;
    type: number;
    typeName: string;
    isDefault: boolean;
    endpoint: string;
    properties: string;
    description: string;
    isEnabled: boolean;
    children: ModelItem[];
    modelCapabilities: number[];
  };
  type ModelSettingDto = {
    provider: number;
    endpoint?: string;
    accessKey: string;
    secretKey?: string;
    deploymentName?: string;
  };

  type Agent = {
    id: string;
    name: string;
    description: string;
    agentType: AgentType;
  };
  enum AgentType {
    Llm = 0,
    Workflow = 1,
  }
}
