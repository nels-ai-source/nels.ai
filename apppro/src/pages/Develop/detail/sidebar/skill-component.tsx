import { Agent, AgentTool } from '@/types/agent';
import {
  DeleteOutlined,
  ExclamationCircleOutlined,
  PlusOutlined,
  ToolOutlined,
} from '@ant-design/icons';
import { Button, List, Popover } from 'antd';
import { UUID } from 'crypto';
import React from 'react';
import { SidebarSection } from './sidebar-section';

interface ToolPopoverContentProps {
  tool: AgentTool;
}

export const ToolPopoverContent: React.FC<ToolPopoverContentProps> = ({ tool }) => {
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

export const SkillComponent: React.FC<{
  agent: Agent;
  onChange: (updates: Partial<Agent>) => void;
}> = ({ agent, onChange }) => {
  const handleDeleteTool = (toolId: UUID) => {
    if (agent?.tools) {
      const updatedTools = agent.tools.filter((tool) => tool.id !== toolId);
      onChange({ tools: updatedTools });
    }
  };
  return (
    <SidebarSection
      title="技能"
      defaultActiveKey={['plugin']}
      items={[
        {
          key: 'plugin',
          label: '插件',
          children:
            agent === null || agent?.tools === null || agent?.tools?.length === 0 ? (
              <p style={{ paddingInlineStart: 24 }}>
                插件能够让智能体调用外部
                API，例如搜索信息、浏览网页、生成图片等，扩展智能体的能力和使用场景。
              </p>
            ) : (
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
                          />
                        )
                      }
                      title={
                        <>
                          {item.pluginName}/{item.name}
                        </>
                      }
                      description={
                        <div
                          style={{
                            whiteSpace: 'nowrap',
                            overflow: 'hidden',
                            textOverflow: 'ellipsis',
                          }}
                        >
                          {item.description}
                        </div>
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
                          ></Button>
                        </Popover>,
                        <Button
                          key="delete"
                          type="text"
                          size="small"
                          icon={<DeleteOutlined />}
                          onClick={() => handleDeleteTool(item.id!)}
                        />,
                      ]}
                    ></List.Item>
                  </List.Item>
                )}
              />
            ),

          extra: (
            <>
              <Button
                type="text"
                size="small"
                icon={<PlusOutlined />}
                onClick={(event) => {
                  event.stopPropagation();
                }}
              ></Button>
            </>
          ),
        },
      ]}
    />
  );
};
export default SkillComponent;
