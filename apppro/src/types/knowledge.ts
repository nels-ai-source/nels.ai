import { Entity, FullAuditedEntity } from './entity';

export interface Knowledge extends FullAuditedEntity {
  name: string;
  icon: string;
  type: string;
  isEnabled: boolean;
  description: string;
  documents: KnowledgeDocument[];
  documentCount: number;
  length: number;
  agentUseCount: number;
  retrievalCount: number;
}
export interface KnowledgeDocument extends Entity {
  name: string;
  type: string;
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
