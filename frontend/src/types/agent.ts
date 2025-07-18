import { Entity, FullAuditedEntity } from './entity';
import { ToolParamter } from './plugin';

export interface Agent extends FullAuditedEntity {
  name: string;
  description: string;
  type: AgentType;
  icon: string;
  instructions: string;
  prologue: string;
  questions: AgentPresetQuestions[];
  tools: AgentTool[];
  knowledges: AgentKnowledge[];
}

export interface AgentPresetQuestions extends Entity {
  content: string;
  index: number;
}

export enum AgentType {
  chatCompletion = 1,
  workflow = 2,
  multiAgent = 3,
}

export interface AgentTool extends Entity {
  pluginName: string;
  icon: string;
  name: string;
  description: string;
  inputParamters: ToolParamter[];
  outputParamters: ToolParamter[];
}
export interface AgentKnowledge extends Entity {
  icon: string;
  name: string;
  description: string;
}
export interface AgentFilter {
  keyword?: string | null;
}

export interface ChatMessage {
  role: 'user' | 'assistant' | 'system';
  content: string;
  id?: string | undefined;
}

export interface ChatRequest {
  conversationId: string | null;
  agentId: string;
  messages: Array<ChatMessage>;
}

export interface ChatCreatedContent {
  chatId: string;
  conversationId: string;
}

// eslint-disable-next-line @typescript-eslint/no-empty-interface
export interface ChatInProgressContent extends ChatCreatedContent {
}

export interface MessageContent extends ChatCreatedContent {
  role: string;
  type: string;
  content: string;
  contentType: string;
}

export interface ChatFailedContent extends ChatCreatedContent {
  code: string;
  msg: string;
}

export interface ChatEventCallbacks {
  onChatCreated?: (data: ChatCreatedContent) => void;
  onChatInProgress?: (data: ChatInProgressContent) => void;
  onMessageDelta?: (data: MessageContent) => void;
  onMessageCompleted?: (data: MessageContent) => void;
  onChatFailed?: (data: ChatFailedContent) => void;
  onSuccess?: (messages: any) => void;
  onError?: (error: any) => void;
}

export interface CancellationToken {
  isCancelled: boolean;
  cancel(): void;
  onCancelled(callback: () => void): void;
}
