import React from 'react';
import { Button, List, Popover } from 'antd';
import { UUID } from 'crypto';
import { useIntl } from 'umi';
import {
  DeleteOutlined,
  ExclamationCircleOutlined,
  PlusOutlined,
  ToolOutlined,
} from '@ant-design/icons';

import { Agent, AgentTool } from '@/types/agent';
import { SidebarSection } from './sidebar-section';

interface ToolPopoverContentProps {
  tool: AgentTool;
}

const ToolPopoverContent: React.FC<ToolPopoverContentProps> = ({ tool }) => {
  return (
    <div className="max-w-[300px] p-1">
      <div className="mb-2">
        <div className="font-bold mb-1">{tool.name}</div>
        <div className="text-gray-600">{tool.description}</div>
      </div>

      <div>
        {tool.inputParamters.map((parameter) => (
          <div key={parameter.name} className="mb-1">
            <div className="font-bold mb-0.5">{parameter.name}</div>
            <div className="text-gray-600">{parameter.description}</div>
          </div>
        ))}
      </div>
    </div>
  );
};

interface SkillComponentProps {
  agent: Agent;
  onChange: (updates: Partial<Agent>) => void;
}

export const SkillComponent: React.FC<SkillComponentProps> = ({ agent, onChange }) => {
  const intl = useIntl();
  const handleDeleteTool = (toolId: UUID) => {
    if (agent?.tools) {
      const updatedTools = agent.tools.filter((tool) => tool.id !== toolId);
      onChange({ tools: updatedTools });
    }
  };

  const handleAddTool = (event: React.MouseEvent) => {
    event.stopPropagation();
  };

  const renderEmptyState = () => (
    <p className="pl-6 text-gray-500">
      {intl.formatMessage({ id: 'agent.detail.skillComponent.emptyText' })}
    </p>
  );

  const renderToolsList = () => (
    <List
      size="small"
      itemLayout="horizontal"
      dataSource={agent?.tools}
      renderItem={(item) => (
        <List.Item>
          <List.Item.Meta
            avatar={
              !item.icon ? (
                <ToolOutlined
                  width={24}
                  height={24}
                  className="rounded-lg object-cover flex items-center justify-center text-2xl"
                />
              ) : (
                <img
                  src={`/images/agent/${item.icon}`}
                  className="w-6 h-6 rounded-lg object-cover flex items-center"
                  alt={item.name}
                />
              )
            }
            title={
              <>{item.pluginName}/{item.name}</>
            }
            description={
              <div className="description-text">{item.description}</div>
            }
          />

          <List.Item
            actions={[
              <Popover
                key="info"
                placement="bottom"
                content={<ToolPopoverContent tool={item} />}
              >
                <Button
                  type="text"
                  size="small"
                  icon={<ExclamationCircleOutlined />}
                />
              </Popover>,
              <Button
                key="delete"
                type="text"
                size="small"
                icon={<DeleteOutlined />}
                onClick={() => handleDeleteTool(item.id!)}
              />,
            ]}
          />
        </List.Item>
      )}
    />
  );

  const hasTools = agent?.tools && agent.tools.length > 0;

  return (
    <SidebarSection
      title={intl.formatMessage({ id: 'agent.detail.sidebar.skills' })}
      defaultActiveKey={['plugin']}
      items={[
        {
          key: 'plugin',
          label: intl.formatMessage({ id: 'agent.detail.sidebar.plugin' }),
          children: hasTools ? renderToolsList() : renderEmptyState(),
          extra: (
            <Button
              type="text"
              size="small"
              icon={<PlusOutlined />}
              onClick={handleAddTool}
            />
          ),
        },
      ]}
    />
  );
};

export default SkillComponent;
