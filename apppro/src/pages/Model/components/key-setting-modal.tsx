import { setKey } from '@/services/aigc/model';
import { Model, ModelProvider, ModelSetKey, ProviderConfig } from '@/types/model';
import { useIntl } from '@umijs/max';
import { Button, Form, Input, Modal } from 'antd';
import React, { useEffect, useState } from 'react';
import { getProviderIcon } from '../util';
import { ProviderSelect } from './provider-select';

interface KeySettingModalProps {
  open: boolean;
  model?: Model;
  mode: 'provider' | 'model';
  onCancel: () => void;
  onChange: (result: boolean) => void;
}

export const KeySettingModal: React.FC<KeySettingModalProps> = ({
  open,
  model,
  mode,
  onCancel,
  onChange,
}) => {
  const intl = useIntl();
  const [form] = Form.useForm();
  const [isLoading, setIsLoading] = useState(false);

  const [selectedProvider, setSelectedProvider] = useState<ModelProvider | null>(null);
  const [selectedModel, setSelectedModel] = useState<Model | null>(null);

  useEffect(() => {
    if (open) {
      form.resetFields();
      setSelectedProvider(mode === 'provider' ? ModelProvider.OpenAI : null);
      setSelectedModel(model || null);
    }
  }, [open]);

  const handleSave = async () => {
    try {
      setIsLoading(true);
      const values = await form.validateFields();
      console.log('values', values);

      setKey({
        provider: selectedProvider || null,
        id: selectedModel?.id || null,
        accessKey: values.accessKey,
        secretKey: values.secretKey,
      } as ModelSetKey);

      onChange(true);
    } catch (error) {
      console.error('Failed to update keys:', error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Modal
      title={intl.formatMessage({
        id: mode === 'provider' ? 'model.operation.setKeys' : 'model.operation.setKey',
      })}
      open={open}
      onCancel={onCancel}
      footer={[
        <Button key="cancel" onClick={onCancel} loading={isLoading}>
          {intl.formatMessage({ id: 'actions.cancel' })}
        </Button>,
        <Button key="submit" type="primary" onClick={handleSave} loading={isLoading}>
          {intl.formatMessage({ id: 'actions.save' })}
        </Button>,
      ]}
      width={600}
    >
      <Form form={form} layout="vertical">
        {mode === 'provider' ? (
          <Form.Item name="provider" label={intl.formatMessage({ id: 'model.provider' })}>
            <ProviderSelect
              value={ModelProvider.OpenAI}
              onChange={setSelectedProvider}
            ></ProviderSelect>
          </Form.Item>
        ) : (
          <Form.Item name="model" label={intl.formatMessage({ id: 'model.name' })}>
            <div className="flex items-center gap-2">
              <img
                src={model?.provider ? getProviderIcon(model.provider) : ''}
                className="w-6 h-6 object-cover"
                alt={model?.provider ? ModelProvider[model.provider] : ''}
              />

              {model?.name}
            </div>
          </Form.Item>
        )}

        {(selectedProvider || selectedModel) && (
          <>
            {ProviderConfig[
              (selectedProvider || selectedModel?.provider) as ModelProvider
            ]?.attributes?.includes('accessKey') && (
              <Form.Item
                name="accessKey"
                label={intl.formatMessage({ id: 'model.accessKey' })}
                rules={[
                  {
                    required: true,
                    message: intl.formatMessage({ id: 'model.required.accessKey' }),
                  },
                ]}
              >
                <Input />
              </Form.Item>
            )}
            {ProviderConfig[
              (selectedProvider || selectedModel?.provider) as ModelProvider
            ]?.attributes?.includes('secretKey') && (
              <Form.Item
                name="secretKey"
                label={intl.formatMessage({ id: 'model.secretKey' })}
                rules={[
                  {
                    required: true,
                    message: intl.formatMessage({ id: 'model.required.secretKey' }),
                  },
                ]}
              >
                <Input.Password />
              </Form.Item>
            )}
          </>
        )}
      </Form>
    </Modal>
  );
};

export default KeySettingModal;
