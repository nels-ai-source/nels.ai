import React from 'react';
import { Button } from 'antd';
import { ProductOutlined } from '@ant-design/icons';
import { useIntl } from 'umi';
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
  const intl = useIntl();
  
  return (
    <aside className="w-[50%] border-l border-gray-200">
      <header className="sidebar-header flex items-center justify-between px-2">
        <div className="sidebar-title flex items-center space-x-2">
          {intl.formatMessage({ id: 'agent.detail.sidebar.orchestration' })}
        </div>
        <div className="flex items-center space-x-3">
          <Button
            type="text"
            size="small"
            icon={<ProductOutlined size={16} />}
            onClick={() => { }}
          />
        </div>
      </header>
      <div className="p-2">
        <SkillComponent agent={agent} onChange={onChange} />
        <KnowledgeComponent agent={agent} onChange={onChange} />
        <ConversationComponent agent={agent} onChange={onChange} />
      </div>
    </aside>
  );
}

export default Sidebar;
