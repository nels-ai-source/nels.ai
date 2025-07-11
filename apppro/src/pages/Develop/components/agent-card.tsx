import { deleteAgent } from '@/services/aigc/agent';
import type { Agent } from '@/types/agent';
import { MoreOutlined, StarOutlined } from '@ant-design/icons';
import { history, useIntl } from '@umijs/max';
import { Button, Card, Dropdown, Modal, Tag } from 'antd';
import React, { useState } from 'react';

interface AgentCardProps {
  agent: Agent;
  onChange: () => void;
}

export const AgentCard: React.FC<AgentCardProps> = ({ agent, onChange }) => {
  const { Meta } = Card;
  const intl = useIntl();
  const [isLoading, setIsLoading] = useState(false);

  const handleDelete = async () => {
    try {
      setIsLoading(true);
      await deleteAgent(agent.id);
      onChange();
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Card
      key={agent.id}
      className="group cursor-pointer"
      onClick={() => {
        history.push(`/develop/detail/${agent.id}`);
      }}
      loading={isLoading}
    >
      <Meta
        avatar={<img src={agent.icon} className="w-16 h-16 rounded-lg object-cover" />}
        title={agent.name}
        description={
          <>
            <div
              className="h-[40px] line-clamp-2 overflow-hidden text-xs"
              style={{
                display: '-webkit-box',
                WebkitLineClamp: 2,
                WebkitBoxOrient: 'vertical',
              }}
            >
              {agent.description}
            </div>
          </>
        }
      />

      <>
        <div className="mt-2">
          <Tag>{intl.formatMessage({ id: 'agent.tag' })}</Tag>
        </div>
        <div className="flex items-center justify-between text-xs text-secondary">
          <div className="flex items-center gap-x-1">
            {/* <span>{user?.name?.slice(0, 4)}</span> */}
            <span className="w-1 h-1 rounded-full bg-secondary"></span>
            <span>
              {intl.formatMessage({ id: 'agent.lastEdit' })}{' '}
              {new Date(agent.lastModificationTime ?? agent.creationTime).toLocaleDateString(
                undefined,
                {
                  year: '2-digit',
                  month: '2-digit',
                  day: '2-digit',
                  hour: '2-digit',
                  minute: '2-digit',
                },
              )}
            </span>
          </div>
          <div className="flex items-center gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
            <Button
              type="text"
              icon={
                <StarOutlined
                  onClick={(event) => {
                    event.stopPropagation();
                  }}
                />
              }
              className="hover:text-primary"
            />

            <Dropdown
              menu={{
                items: [
                  {
                    key: 'duplicate',
                    label: intl.formatMessage({ id: 'agent.actions.duplicate' }),
                    onClick: (event) => {
                      event.domEvent?.stopPropagation();
                      onChange();
                    },
                  },
                  {
                    key: 'delete',
                    label: intl.formatMessage({ id: 'agent.actions.delete' }),
                    danger: true,
                    onClick: (event) => {
                      event.domEvent?.stopPropagation();
                      Modal.confirm({
                        title: intl.formatMessage({ id: 'agent.deleteConfirm.title' }),
                        content: intl.formatMessage({ id: 'agent.deleteConfirm.content' }),
                        okText: intl.formatMessage({ id: 'agent.actions.confirm' }),
                        cancelText: intl.formatMessage({ id: 'agent.actions.cancel' }),
                        onOk: () => handleDelete(),
                      });
                    },
                  },
                ],
              }}
            >
              <Button
                type="text"
                icon={<MoreOutlined />}
                className="hover:text-primary"
                onClick={(event) => {
                  event.stopPropagation();
                }}
              />
            </Dropdown>
          </div>
        </div>
      </>
    </Card>
  );
};

export default AgentCard;
