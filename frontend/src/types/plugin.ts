import { Entity, FullAuditedEntity } from './entity';

export interface Plugin extends FullAuditedEntity {
  name: string;
  icon: string;
  version: string;
  description: string;
  manifestUrl: string;
}

export interface Tool extends Entity {
  pluginId: string;
  name: string;
  description: string;
  inputParamters: ToolParamter[];
  outputParamters: ToolParamter[];
}

export interface ToolParamter {
  name: string;
  description: string;
  type: string;
  required: boolean;
}
