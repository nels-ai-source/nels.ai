import { UUID } from 'crypto';
import { Entity, AuditedEntity, FullAuditedEntity } from './entity';
import { Knowledge } from './knowledge';
import { Tool } from './plugin';

export interface Bot extends FullAuditedEntity {
    name: string;
    description: string;
    icon: string;
    instructions: string;
    prologue: string;
    suggestedQuestions: BotSuggestedQuestion[];
    tools: Tool[];
    knowledges: Knowledge[];
}

export interface BotSuggestedQuestion extends Entity {
    question: string;
    order: number;
}
