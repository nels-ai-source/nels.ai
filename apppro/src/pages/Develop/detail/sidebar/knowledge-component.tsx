import { Agent } from '@/types/agent';
import { DeleteOutlined, PlusOutlined, SettingOutlined, ToolOutlined } from '@ant-design/icons';
import { Button, List } from 'antd';
import { UUID } from 'crypto';
import React from 'react';
import { SidebarSection } from './sidebar-section';
export const KnowledgeComponent: React.FC<{
  agent: Agent;
  onChange: (updates: Partial<Agent>) => void;
}> = ({ agent, onChange }) => {
  const handleDeleteKnowledge = (knowledgeId: UUID) => {
    if (agent?.knowledges) {
      const updatedKnowledges = agent.knowledges.filter(
        (knowledge) => knowledge.id !== knowledgeId,
      );
      onChange({ knowledges: updatedKnowledges });
    }
  };
  return (
    <SidebarSection
      title="知识"
      defaultActiveKey={['konwsledge']}
      items={[
        {
          key: 'konwsledge',
          label: '文本',
          children:
            agent === null || agent?.knowledges === null || agent?.knowledges?.length === 0 ? (
              <p className="pl-6 text-gray-500">
                将文档、URL、三方数据源上传为文本知识库后，用户发送消息时，智能体能够引用文本知识中的内容回答用户问题。
              </p>
            ) : (
              <List
                size="small"
                itemLayout="horizontal"
                dataSource={agent?.knowledges}
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
                            src={`/images/plugin/${item.icon}`}
                            className="w-6 h-6 rounded-lg object-cover flex items-center"
                          />
                        )
                      }
                      title={<>{item.name}</>}
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
                        <Button
                          key="delete"
                          type="text"
                          size="small"
                          icon={<DeleteOutlined />}
                          onClick={() => handleDeleteKnowledge(item.id!)}
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
              />
            </>
          ),
        },
      ]}
      extra={
        <Button type="text" size="small" icon={<SettingOutlined />}>
          自动调用
        </Button>
      }
    />
  );
};
export default KnowledgeComponent;