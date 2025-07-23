import { Entity, FullAuditedEntity } from './entity';
import { AgentType } from './agent';

export interface Knowledge extends FullAuditedEntity {
  name: string;
  icon: string;
  type: string;
  isEnabled: boolean;
  description: string;
  documents: KnowledgeDocument[];
  documentCount: number;
  length: number;
  embeddingModelId: string;
  agentUseCount: number;
  retrievalCount: number;
  // 添加关联的 agent 类型信息
  associatedAgentTypes?: AgentType[];
  primaryAgentType?: AgentType;
}
export interface KnowledgeDocument extends Entity {
  name: string;
  type: string;
  fileId?: string;
  paragraphs: KnowledgeDocumentParagraph[];
}
export interface KnowledgeDocumentParagraph extends Entity {
  index: number;
  content: string;
  isEnabled: boolean;
  retrievalCount: number;
  length: number;
  embedding: boolean;
}
