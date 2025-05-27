import { UUID } from 'crypto';
import { Entity, AuditedEntity, FullAuditedEntity } from './entity';

export interface Tool extends Entity {
    pluginName: string;
    pluginIcon: string;
    name: string;
    icon: string;
    description: string;
    inputParameters: PluginParameter[];
}
export interface PluginParameter {
    name: string;
    description: string;
    type: string;
}
