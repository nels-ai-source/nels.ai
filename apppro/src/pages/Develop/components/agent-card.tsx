import type { Agent } from '@/types/agent';
import { MoreOutlined, StarOutlined } from '@ant-design/icons';
import { history, useIntl } from '@umijs/max';
import { Button, Card, Dropdown, Modal, Tag } from 'antd';
import { UUID } from 'crypto';
import React from 'react';

interface AgentCardProps {
  agent: Agent;
  onCreateGallery: (agent: Agent) => void;
  onDeleteGallery: (agentId: UUID) => void;
}

export const AgentCard: React.FC<AgentCardProps> = ({
  agent,
  onCreateGallery,
  onDeleteGallery,
}) => {
  const { Meta } = Card;
  const intl = useIntl();
  return (
    <Card
      key={agent.id}
      className="group cursor-pointer"
      onClick={() => {
        history.push(`/develop/detail/${agent.id}`);
      }}
      data-oid="0:z.i5l"
    >
      <Meta
        avatar={
          <img
            src={`/images/agent/${agent.icon}`}
            className="w-16 h-16 rounded-lg object-cover"
            data-oid="1g9ktv2"
          />
        }
        title={agent.name}
        description={
          <>
            <div className="h-[40px] line-clamp-2 overflow-hidden mb-2" data-oid="pracpo6">
              {agent.description}
            </div>
          </>
        }
        data-oid="puqk6c."
      />

      <>
        <Tag data-oid="khumwqr">{intl.formatMessage({ id: 'agent.tag' })}</Tag>
        <div
          className="flex items-center justify-between text-xs text-secondary"
          data-oid="r7pgmng"
        >
          <div className="flex items-center gap-x-1" data-oid="28exeaq">
            {/* <span>{user?.name?.slice(0, 4)}</span> */}
            <span className="w-1 h-1 rounded-full bg-secondary" data-oid="_0y-4nc"></span>
            <span data-oid="lap:f.6">
              {intl.formatMessage({ id: 'agent.lastEdit' })}{' '}
              {new Date(agent.lastModificationTime ?? '').toLocaleDateString(undefined, {
                year: '2-digit',
                month: '2-digit',
                day: '2-digit',
                hour: '2-digit',
                minute: '2-digit',
              })}
            </span>
          </div>
          <div
            className="flex items-center gap-2 opacity-0 group-hover:opacity-100 transition-opacity"
            data-oid="2232vyl"
          >
            <Button
              type="text"
              icon={
                <StarOutlined
                  onClick={(event) => {
                    event.stopPropagation();
                  }}
                  data-oid="n06.t:w"
                />
              }
              className="hover:text-primary"
              data-oid="6ot.:dn"
            />

            <Dropdown
              menu={{
                items: [
                  {
                    key: 'duplicate',
                    label: intl.formatMessage({ id: 'agent.actions.duplicate' }),
                    onClick: (event) => {
                      event.domEvent?.stopPropagation();
                      onCreateGallery(agent);
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
                        onOk: () => onDeleteGallery(agent.id),
                      });
                    },
                  },
                ],
              }}
              data-oid="7idjba8"
            >
              <Button
                type="text"
                icon={<MoreOutlined data-oid="s-wmmq4" />}
                className="hover:text-primary"
                onClick={(event) => {
                  event.stopPropagation();
                }}
                data-oid="l47udif"
              />
            </Dropdown>
          </div>
        </div>
      </>
    </Card>
  );
};

export default AgentCard;
