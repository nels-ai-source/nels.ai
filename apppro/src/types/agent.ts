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
