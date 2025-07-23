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
  knowledgeOption?: KnowledgeOption;
}

export enum SearchStrategy {
  Hybrid = 0,
  Semantic = 1,
  FullText = 2,
}

export enum ReplyMode {
  Default = 0,
  Custom = 1,
}

export enum SourceDisplayMode {
  Card = 0,
  Text = 1,
}

export interface KnowledgeOption extends Entity {
  autoInvoke: boolean;
  searchStrategy: SearchStrategy;
  maxRecallCount: number;
  minMatchScore: number;
  replyMode: ReplyMode;
  customReply: string;
  showSource: boolean;
  sourceDisplayMode: SourceDisplayMode;
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
  agentId: string;
  knowledgeId: string;
  icon?: string;
  name: string;
  description?: string;
}
export interface AgentFilter {
  keyword?: string | null;
  type?: AgentType;
  skipCount?: number;
  maxResultCount?: number;
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
