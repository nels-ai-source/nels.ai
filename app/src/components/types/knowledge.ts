import { UUID } from 'crypto';
import { Entity, AuditedEntity, FullAuditedEntity } from './entity';

export interface Knowledge extends FullAuditedEntity {
    name: string;
    icon: string;
    type: string;
    isEnabled: boolean;
    description: string;
    documents: KnowledgeDocument[];
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