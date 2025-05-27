import { createModeList } from '@/services/aigc/model';
import {
  Model,
  ModelCapability,
  ModelInstanceConsts,
  ModelProvider,
  ModelType,
  ProviderConfig,
} from '@/types/model';
import { CloseOutlined } from '@ant-design/icons';
import { useIntl } from '@umijs/max';
import { Button, Form, Input, Modal, Select, Space } from 'antd';
import React, { useEffect, useState } from 'react';
import { ProviderSelect } from './provider-select';

interface ModelFormItemProps {
  data: Model;
  field: any;
  remove: (index: number) => void;
  intl: ReturnType<typeof useIntl>;
}

const ModelFormItem: React.FC<ModelFormItemProps> = ({ data, field, remove, intl }) => (
  <Space key={field.key} style={{ width: '100%' }}>
    <Form.Item
      noStyle
      name={[field.name, 'type']}
      rules={[
        {
          required: true,
          message: intl.formatMessage({ id: 'model.required.type' }),
        },
      ]}
    >
      <Select
        placeholder={intl.formatMessage({ id: 'model.placeholder.type' })}
        style={{ width: 120 }}
      >
        {Object.entries(ModelType)
          .filter(([key]) => isNaN(Number(key)))
          .map(([key, value]) => (
            <Select.Option key={value} value={value}>
              {intl.formatMessage({ id: `ModelType.${key}` })}
            </Select.Option>
          ))}
      </Select>
    </Form.Item>
    <Form.Item
      noStyle
      name={[field.name, 'name']}
      rules={[
        {
          required: true,
          message: intl.formatMessage({ id: 'model.required.name' }),
        },
      ]}
    >
      <Input
        placeholder={intl.formatMessage({ id: 'model.placeholder.name' })}
        maxLength={ModelInstanceConsts.maxNameLength}
      />
    </Form.Item>
    {ProviderConfig[data.provider]?.attributes.includes('deploymentName') && (
      <Form.Item
        noStyle
        name={[field.name, 'deploymentName']}
        rules={[
          {
            required: true,
            message: intl.formatMessage({ id: 'model.required.deploymentName' }),
          },
        ]}
      >
        <Input
          style={{ width: 100 }}
          placeholder={intl.formatMessage({ id: 'model.placeholder.deploymentName' })}
          maxLength={ModelInstanceConsts.maxDeploymentNameLength}
        />
      </Form.Item>
    )}
    <Form.Item
      noStyle
      name={[field.name, 'maxTokens']}
      initialValue={0}
      rules={[
        {
          required: true,
          message: intl.formatMessage({ id: 'model.required.maxTokens' }),
        },
      ]}
    >
      <Input
        style={{ width: 100 }}
        type="number"
        placeholder={intl.formatMessage({ id: 'model.maxTokens' })}
        min={0}
        suffix="k"
      />
    </Form.Item>
    <Form.Item
      noStyle
      name={[field.name, 'modelCapabilities']}
      rules={[
        {
          required: true,
          message: intl.formatMessage({ id: 'model.required.capabilities' }),
        },
      ]}
    >
      <Select
        mode="multiple"
        placeholder={intl.formatMessage({ id: 'model.placeholder.capabilities' })}
        maxTagCount={1}
        style={{ width: 180 }}
      >
        {Object.entries(ModelCapability)
          .filter(([key]) => isNaN(Number(key)))
          .map(([key, value]) => (
            <Select.Option key={value} value={value}>
              {intl.formatMessage({ id: `ModelCapability.${key}` })}
            </Select.Option>
          ))}
      </Select>
    </Form.Item>
    <CloseOutlined onClick={() => remove(field.name)} />
  </Space>
);

interface CreateModalProps {
  open: boolean;
  onCancel: () => void;
  onCreate: (result: boolean) => void;
}
interface ModelFormValues {
  values: Model;
  models: Model[];
}

export const CreateModal: React.FC<CreateModalProps> = ({ open, onCancel, onCreate }) => {
  const intl = useIntl();
  const [form] = Form.useForm();
  const [isLoading, setIsLoading] = useState(false);
  const [data, setData] = useState<ModelFormValues>(
    () => ({ values: { provider: ModelProvider.OpenAI }, models: [{}] } as ModelFormValues),
  );

  const handleProviderChange = (value: ModelProvider) => {
    form.setFieldValue('provider', value);

    const url = ProviderConfig[value]?.endpoint ?? null;
    const newData = { ...data.values, provider: value };

    newData.endpoint = url;
    form.setFieldValue('endpoint', url);
    setData({ ...data, values: newData });
  };

  useEffect(() => {
    if (open) {
      form.resetFields();
      handleProviderChange(ModelProvider.OpenAI);
    }
  }, [open]);

  const handleCreate = async () => {
    const values = await form.validateFields();
    const models: Model[] = values.models.map((model: Model) => ({
      ...model,
      ...values,
    }));
    try {
      await createModeList(models);
      onCreate(true);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Modal
      title={intl.formatMessage({ id: 'model.createModalTitle' })}
      open={open}
      onCancel={onCancel}
      footer={[
        <Button key="cancel" onClick={onCancel} loading={isLoading}>
          {intl.formatMessage({ id: 'actions.cancel' })}
        </Button>,
        <Button key="submit" type="primary" onClick={handleCreate} loading={isLoading}>
          {intl.formatMessage({ id: 'actions.create' })}
        </Button>,
      ]}
      width={800}
    >
      <Form form={form} layout="vertical" initialValues={data}>
        <Form.Item
          name="provider"
          label={intl.formatMessage({ id: 'model.provider' })}
          rules={[
            {
              required: true,
              message: intl.formatMessage({ id: 'model.required.provider' }),
            },
          ]}
        >
          <ProviderSelect
            value={data.values.provider}
            onChange={(value) => handleProviderChange(value)}
          />
        </Form.Item>
        {ProviderConfig[data.values.provider]?.attributes.includes('endpoint') && (
          <Form.Item
            name="endpoint"
            label={intl.formatMessage({ id: 'model.endpoint' })}
            rules={[
              {
                required: true,
                message: intl.formatMessage({ id: 'model.required.endpoint' }),
              },
            ]}
          >
            <Input maxLength={ModelInstanceConsts.maxEndpointLength} showCount />
          </Form.Item>
        )}
        {ProviderConfig[data.values.provider]?.attributes.includes('accessKey') && (
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
            <Input maxLength={ModelInstanceConsts.maxAccessKeyLength} showCount />
          </Form.Item>
        )}
        {ProviderConfig[data.values.provider]?.attributes.includes('secretKey') && (
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
            <Input maxLength={ModelInstanceConsts.maxSecretKeyLength} showCount />
          </Form.Item>
        )}
        <Form.Item label={intl.formatMessage({ id: 'model.models' })}>
          <Form.List name="models">
            {(fields, { add, remove }) => (
              <div className="flex flex-col gap-4">
                {fields.map((field) => (
                  <ModelFormItem
                    data={data.values}
                    key={field.key}
                    field={field}
                    remove={(i: number) => fields.length > 1 && remove(i)}
                    intl={intl}
                  />
                ))}
                <Button type="dashed" onClick={() => add()} block>
                  + {intl.formatMessage({ id: 'model.operation.addModel' })}
                </Button>
              </div>
            )}
          </Form.List>
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default CreateModal;
