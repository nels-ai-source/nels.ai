import React from 'react';
import { Bot } from '../../../../types/bot';

import { ConversationExperience } from './conversationExperience';
import { SkillComponent } from './skillComponent';
import { KnowledgeComponent } from './knowledgeComponent';

import './sidebar.css';

interface SidebarProps {
    bot: Bot;
    onChange: (updates: Partial<Bot>) => void;
}

export function Sidebar({ bot, onChange }: SidebarProps) {
    return (
        <aside className="w-[50%] p-2 border-l border-gray-200">
            <SkillComponent bot={bot} onChange={onChange} />
            <KnowledgeComponent bot={bot} onChange={onChange} />
            <ConversationExperience bot={bot} onChange={onChange} />
        </aside>
    );
}
