import { UUID } from 'crypto';
import { Entity, AuditedEntity, FullAuditedEntity } from './entity';

export enum ModelProvider {
    OpenAI = 0,
    Azure = 1,
    Anthropic = 2,
}

export enum ModelType {
    Text = 0,
    Image = 1,
    Audio = 2,
}

export enum ModelConnector {
    OpenAI = 0,
    Azure = 1,
    Anthropic = 2,
}

export enum ModelCapability {
    Chat = 0,
    Completion = 1,
    ImageGeneration = 2,
    ImageAnalysis = 3,
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
    capabilities?: string;
    modelCapabilities: ModelCapability[];
}
