import { Agent } from '@/types/agent';

import { ConversationComponent } from './conversation-component';
import { KnowledgeComponent } from './knowledge-component';
import { SkillComponent } from './skill-component';

import './sidebar.css';

interface SidebarProps {
  agent: Agent;
  onChange: (updates: Partial<Agent>) => void;
}

export function Sidebar({ agent, onChange }: SidebarProps) {
  return (
    <aside className="w-[50%] p-2 border-l border-gray-200">
      <SkillComponent agent={agent} onChange={onChange} />
      <KnowledgeComponent agent={agent} onChange={onChange} />
      <ConversationComponent agent={agent} onChange={onChange} />
    </aside>
  );
}
