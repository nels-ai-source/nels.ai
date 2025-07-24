import {
  FormOutlined,
  LeftOutlined,
} from '@ant-design/icons';
import { useIntl } from '@umijs/max';
import { Button, Space } from 'antd';
import React, { useState } from 'react';
import { CreateModal } from '../components/create-modal';
import { Agent, AgentType } from '@/types/agent';
import { AgentTypeSelect } from '../components/agent-type-select';

interface HeaderProps {
  agent: Agent;
  onChange: (updates: Partial<Agent>) => void;
  onSave: () => void;
  canUpdate?: boolean;
}

export const Header: React.FC<HeaderProps> = ({ onChange, onSave, agent, canUpdate = false }) => {
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const intl = useIntl();

  return (
    <>
      <CreateModal
        open={isCreateModalOpen}
        onOpenChange={setIsCreateModalOpen}
        onSuccess={() => {
          setIsCreateModalOpen(false);
        }}
        type="edit"
        values={agent || {}}
      />
      <header
        className="bg-gray-50 border-b border-gray-200 z-10 flex items-center justify-between h-14 px-2 shadow-sm"
      >
        <Space className="flex items-center">
          <Button type="text" icon={<LeftOutlined />} onClick={() => {
            window.history.back();
          }} />
          <img src={agent?.icon} alt="avatar" style={{ width: '32px', height: '32px', borderRadius: '4px' }} />
          <div className="flex items-center">{agent?.name}</div>
          {canUpdate && (
            <Button
              type="text"
              icon={<FormOutlined />}
              title={agent?.description || ''}
              onClick={() => {
                setIsCreateModalOpen(true);
              }}
            />
          )}

          <AgentTypeSelect
            onChange={canUpdate ? (value) => onChange({ type: value as AgentType }) : undefined}
            value={agent?.type || AgentType.chatCompletion}
            style={{ width: '250px' }}
            disabled={!canUpdate}
          />
        </Space>

        <div className="flex items-center space-x-3">
          {canUpdate && (
            <Button type="primary" onClick={onSave}>
              {intl.formatMessage({ id: 'agent.detail.save' })}
            </Button>
          )}
        </div>
      </header>
    </>
  );
};

export default Header;
