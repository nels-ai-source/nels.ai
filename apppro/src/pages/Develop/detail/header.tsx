import {
  DownOutlined,
  FormOutlined,
  LeftOutlined,
  OneToOneOutlined,
  PartitionOutlined,
  ClusterOutlined
} from '@ant-design/icons';
import { useIntl } from '@umijs/max';
import type { MenuProps } from 'antd';
import { Button, Dropdown, Space, Select } from 'antd';
import React, { useState } from 'react';
import { CreateModal } from '../components/create-modal';
import { Agent, AgentType } from '@/types/agent';
import { Icon } from 'lucide-react';

interface HeaderProps {
  agent: Agent | null;
  onChange: (updates: Partial<Agent>) => void;
  onSave: () => void;
}

export const Header: React.FC<HeaderProps> = ({ onChange, onSave, agent }) => {
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const intl = useIntl();
  const items = [
    {
      label: '单 Agent（自主规划模式）',
      key: 1,
      icon: <OneToOneOutlined className="w-6 h-6 object-cover" />,
    },
    {
      label: '单 Agent（对话流模式）',
      key: 2,
      icon: <PartitionOutlined className="w-6 h-6 object-cover" />,
    }, {
      label: '多 Agents',
      key: 3,
      icon: <ClusterOutlined className="w-6 h-6 object-cover" />,
    }
  ];

  return (
    <>
      <CreateModal
        open={isCreateModalOpen}
        onOpenChange={setIsCreateModalOpen}
        onChange={(values) => {
          onChange(values);
          setIsCreateModalOpen(false);
        }}

        type="edit"
        values={agent || {}}
      />
      <header
        className="bg-gray-50 border-b border-gray-200 z-10 flex items-center justify-between h-14 px-2 md:h-16 md:px-4 shadow-sm"

      >
        <Space className="flex items-center">
          <Button type="text" icon={<LeftOutlined />} onClick={() => {
            window.history.back();
          }} />
          <img src={agent?.icon} alt="avatar" style={{ width: '32px', height: '32px', borderRadius: '4px' }} />
          <div className="flex items-center">{agent?.name}</div>
          <Button
            type="text"
            icon={<FormOutlined />}
            title={agent?.description || ''}
            onClick={() => {
              setIsCreateModalOpen(true);
            }}

          />

          <Select onChange={(value) => onChange({ type: value })} value={agent?.type || AgentType.chatCompletion} style={{ width: '250px' }}>
            {Object.entries(AgentType)
              .filter(([key]) => isNaN(Number(key)))
              .map(([, value]) => (
                <Select.Option key={value} value={value}>
                  <div className="flex items-center">
                    {items.find((item) => item?.key === value)?.icon}
                    {items.find((item) => item?.key === value)?.label}
                  </div>
                </Select.Option>
              ))}
          </Select>
        </Space>

        <div className="flex items-center space-x-3">
          <Button type="primary" onClick={onSave}>
            保存
          </Button>
        </div>
      </header>
    </>
  );
};

export default Header;
