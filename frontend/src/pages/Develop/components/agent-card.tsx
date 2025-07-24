import React, { useState } from 'react';
import { Card, Button, Dropdown, Modal, Tag, Avatar } from 'antd';
import { MoreOutlined, StarOutlined } from '@ant-design/icons';
import { history, useIntl } from '@umijs/max';

import { deleteAgent } from '@/services/aigc/agent';
import type { Agent } from '@/types/agent';

interface AgentCardProps {
  data: Agent;
  onEdit: (data: Agent) => void;
  onChanged: () => void;
  cardConfig?: {
    showAvatar?: boolean;
    showTags?: boolean;
    showActions?: boolean;
    avatarField?: string;
    titleField?: string;
    descriptionField?: string;
    tagField?: string;
    statusField?: string;
  };
}

export const AgentCard: React.FC<AgentCardProps> = ({
  data,
  onEdit,
  onChanged,
  cardConfig = {
    showAvatar: true,
    showTags: true,
    showActions: true,
    avatarField: 'icon',
    titleField: 'name',
    descriptionField: 'description',
    tagField: 'type',
    statusField: 'type',
  }
}) => {
  const { Meta } = Card;
  const intl = useIntl();
  const [loading, setLoading] = useState(false);

  const handleDelete = async () => {
    try {
      setLoading(true);
      await deleteAgent(data.id);
      onChanged();
    } catch (error) {
      console.error('Failed to delete agent:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleCardClick = () => {
    history.push(`/develop/detail/${data.id}`);
  };

  const getFieldValue = (field: string) => {
    return data[field as keyof typeof data];
  };


  const getAvatarSrc = (field: string): string => {
    const value = getFieldValue(field);
    return typeof value === 'string' ? value : '/default-avatar.png';
  };


  return (
    <Card
      className="group cursor-pointer"
      onClick={handleCardClick}
      loading={loading}
    >
      <Meta
        avatar={
          cardConfig.showAvatar ? (
            <Avatar
              src={getAvatarSrc(cardConfig.avatarField!)}
              size={48}
              shape="square"
            />
          ) : undefined
        }
        title={data.name}
        description={
          <div
            className="h-8 line-clamp-2 overflow-hidden text-xs"
            style={{
              display: '-webkit-box',
              WebkitLineClamp: 2,
              WebkitBoxOrient: 'vertical',
            }}
          >
            {data.description}
          </div>
        }
      />
      <div className="mt-1">
        <Tag>{intl.formatMessage({ id: 'agent.tag' })}</Tag>
      </div>
      <div className="flex items-center justify-between text-xs text-secondary mt-1">
        <div className="flex items-center gap-x-1">
          {/* <span>{user?.name?.slice(0, 4)}</span> */}
          <span className="w-1 h-1 rounded-full bg-secondary"></span>
          <span>
            {intl.formatMessage({ id: 'agent.lastEdit' })}{' '}
            {new Date(data.lastModificationTime ?? data.creationTime).toLocaleDateString(
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
                  key: 'edit',
                  label: intl.formatMessage({ id: 'actions.edit' }),
                  onClick: (event) => {
                    event.domEvent?.stopPropagation();
                    onEdit(data);
                  },
                },
                {
                  key: 'duplicate',
                  label: intl.formatMessage({ id: 'actions.duplicate' }),
                  onClick: (event) => {
                    event.domEvent?.stopPropagation();
                  },
                },
                {
                  key: 'delete',
                  label: intl.formatMessage({ id: 'actions.delete' }),
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
    </Card>
  );
};