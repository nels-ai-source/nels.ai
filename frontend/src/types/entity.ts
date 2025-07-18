
import { UUID } from 'crypto';
export interface Entity {
    id: UUID;
}

export interface CreationAuditedEntity extends Entity {
    creatorId?: UUID;
    creationTime: Date;
}

export interface AuditedEntity extends CreationAuditedEntity {
    lastModifierId?: UUID;
    lastModificationTime?: Date;
}

export interface FullAuditedEntity extends AuditedEntity {
    isDeleted: boolean;
    deleterId?: UUID;
    deletionTime?: Date;
}