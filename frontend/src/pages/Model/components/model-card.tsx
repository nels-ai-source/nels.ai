import { Permissions } from '@/access';
import { deleteModel, setIsEnabled } from '@/services/aigc/model';
import { Model, ModelCapability, ModelProvider, ModelType } from '@/types/model';
import { EditOutlined, KeyOutlined, MoreOutlined } from '@ant-design/icons';
import { FormattedMessage, useAccess, useIntl } from '@umijs/max';
import { Button, Card, Dropdown, Modal, Space, Tag } from 'antd';
import React, { useState } from 'react';
import { getEnumLabel, getProviderIcon } from '../util';
import { EditModal } from './edit-modal';
import { KeySettingModal } from './key-setting-modal';

interface ModelCardProps {
  model: Model;
  onChange: () => void;
}

export const ModelCard: React.FC<ModelCardProps> = ({ model, onChange }) => {
  const access = useAccess();
  const { Meta } = Card;
  const intl = useIntl();
  const [isLoading, setIsLoading] = useState(false);
  const handleDelete = async () => {
    try {
      setIsLoading(true);
      await deleteModel(model.id);
      onChange();
    } finally {
      setIsLoading(false);
    }
  };
  const handleSetEnabled = async (isEnabled: boolean) => {
    try {
      setIsLoading(true);
      await setIsEnabled(model.id, isEnabled);
      onChange();
    } finally {
      setIsLoading(false);
    }
  };
  const [editModalVisible, setEditModalVisible] = useState(false);
  const handleEdit = () => {
    setEditModalVisible(true);
  };
  const [isKeySettingModalOpen, setIsKeySettingModalOpen] = useState(false);
  return (
    <>
      <EditModal
        model={model}
        open={editModalVisible}
        onCancel={() => setEditModalVisible(false)}
        onChange={(result) => {
          if (!result) {
            return;
          }
          setEditModalVisible(false);
          onChange();
        }}
      />

      <KeySettingModal
        open={isKeySettingModalOpen}
        mode="model"
        model={model}
        onCancel={() => setIsKeySettingModalOpen(false)}
        onChange={onChange}
      />

      <Card className={'group mb-4'}>
        <Meta
          avatar={
            <img
              src={getProviderIcon(model.provider)}
              className="w-16 h-16 rounded-lg object-cover"
            />
          }
          title={
            <>
              <div className="flex justify-between items-center">
                <div>{model.name}</div>
                <Tag color={model.isEnabled ? 'processing' : 'default'}>
                  {model.isEnabled ? (
                    <FormattedMessage id="status.running" />
                  ) : (
                    <FormattedMessage id="status.disable" />
                  )}
                </Tag>
              </div>
              <div>
                <Tag color="processing">
                  {intl.formatMessage(getEnumLabel(ModelType, 'ModelType', model.type))}
                </Tag>
                {model.modelCapabilities.map((option) => (
                  <Tag key={option}>
                    {intl.formatMessage(getEnumLabel(ModelCapability, 'ModelCapability', option))}
                  </Tag>
                ))}
              </div>
            </>
          }
          description={
            <div
              style={{
                display: '-webkit-box',
                WebkitLineClamp: 2,
                WebkitBoxOrient: 'vertical',
                overflow: 'hidden',
              }}
            >
              {model.description}
            </div>
          }
        />

        <div className="flex items-center justify-between text-xs text-secondary">
          <Space className="flex items-center gap-x-1">
            <span>
              @{intl.formatMessage(getEnumLabel(ModelProvider, 'ModelProvider', model.provider))}
            </span>
            {model.maxTokens && (
              <span>
                <FormattedMessage id="model.maxTokens" />
                {model.maxTokens}k
              </span>
            )}
            <span>
              <FormattedMessage id="actions.edit" />{' '}
              {new Date(model.lastModificationTime ?? model.creationTime).toLocaleDateString(
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
          </Space>
          <div className="flex items-center gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
            {access.checkAccess(Permissions.Model.Update) && (
              <Button
                type="text"
                icon={<EditOutlined />}
                className="hover:text-primary"
                loading={isLoading}
                onClick={handleEdit}
              />
            )}
            {access.checkAccess(Permissions.Model.SetKey) && (
              <Button
                type="text"
                icon={<KeyOutlined />}
                className="hover:text-primary"
                loading={isLoading}
                onClick={() => setIsKeySettingModalOpen(true)}
              />
            )}
            <Dropdown
              menu={{
                items: [
                  access.checkAccess(Permissions.Model.Update)
                    ? {
                        key: 'duplicate',
                        label: !model.isEnabled
                          ? intl.formatMessage({ id: 'status.enable' })
                          : intl.formatMessage({ id: 'status.disable' }),
                        onClick: () => {
                          handleSetEnabled(!model.isEnabled);
                        },
                      }
                    : null,
                  access.checkAccess(Permissions.Model.Delete)
                    ? {
                        key: 'delete',
                        label: intl.formatMessage({ id: 'actions.delete' }),
                        danger: true,
                        onClick: () => {
                          Modal.confirm({
                            title: intl.formatMessage({ id: 'deleteConfirm.title' }),
                            content: intl.formatMessage({ id: 'deleteConfirm.content' }),
                            okText: intl.formatMessage({ id: 'actions.confirm' }),
                            cancelText: intl.formatMessage({ id: 'actions.cancel' }),
                            onOk: handleDelete,
                          });
                        },
                      }
                    : null,
                ],
              }}
            >
              <Button
                type="text"
                icon={<MoreOutlined />}
                className="hover:text-primary"
                loading={isLoading}
              />
            </Dropdown>
          </div>
        </div>
      </Card>
    </>
  );
};
