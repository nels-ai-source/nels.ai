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
  <Space key={field.key} style={{ width: '100%' }} data-oid="5b_ek7t">
    <Form.Item
      noStyle
      name={[field.name, 'type']}
      rules={[
        {
          required: true,
          message: intl.formatMessage({ id: 'model.required.type' }),
        },
      ]}
      data-oid="wv1m:qh"
    >
      <Select
        placeholder={intl.formatMessage({ id: 'model.placeholder.type' })}
        style={{ width: 120 }}
        data-oid="ztgxm9s"
      >
        {Object.entries(ModelType)
          .filter(([key]) => isNaN(Number(key)))
          .map(([key, value]) => (
            <Select.Option key={value} value={value} data-oid="j5py8-u">
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
      data-oid="m70:bv0"
    >
      <Input
        placeholder={intl.formatMessage({ id: 'model.placeholder.name' })}
        maxLength={ModelInstanceConsts.maxNameLength}
        data-oid="yw6:6:x"
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
        data-oid="6yq7_mq"
      >
        <Input
          style={{ width: 100 }}
          placeholder={intl.formatMessage({ id: 'model.placeholder.deploymentName' })}
          maxLength={ModelInstanceConsts.maxDeploymentNameLength}
          data-oid="4-xetqp"
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
      data-oid="ks7k4fo"
    >
      <Input
        style={{ width: 100 }}
        type="number"
        placeholder={intl.formatMessage({ id: 'model.maxTokens' })}
        min={0}
        suffix="k"
        data-oid="o5jlfy-"
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
      data-oid="-33z_lh"
    >
      <Select
        mode="multiple"
        placeholder={intl.formatMessage({ id: 'model.placeholder.capabilities' })}
        maxTagCount={1}
        style={{ width: 180 }}
        data-oid="ytc2ayi"
      >
        {Object.entries(ModelCapability)
          .filter(([key]) => isNaN(Number(key)))
          .map(([key, value]) => (
            <Select.Option key={value} value={value} data-oid="2bycv.4">
              {intl.formatMessage({ id: `ModelCapability.${key}` })}
            </Select.Option>
          ))}
      </Select>
    </Form.Item>
    <CloseOutlined onClick={() => remove(field.name)} data-oid="q:o1sy-" />
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
    () => ({ values: { provider: ModelProvider.OpenAI }, models: [{}] }) as ModelFormValues,
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
        <Button key="cancel" onClick={onCancel} loading={isLoading} data-oid="cytdxbr">
          {intl.formatMessage({ id: 'actions.cancel' })}
        </Button>,
        <Button
          key="submit"
          type="primary"
          onClick={handleCreate}
          loading={isLoading}
          data-oid="vumn2dt"
        >
          {intl.formatMessage({ id: 'actions.create' })}
        </Button>,
      ]}
      width={800}
      data-oid="soj61up"
    >
      <Form form={form} layout="vertical" initialValues={data} data-oid="qaon:pj">
        <Form.Item
          name="provider"
          label={intl.formatMessage({ id: 'model.provider' })}
          rules={[
            {
              required: true,
              message: intl.formatMessage({ id: 'model.required.provider' }),
            },
          ]}
          data-oid="ex_jzz3"
        >
          <ProviderSelect
            value={data.values.provider}
            onChange={(value) => handleProviderChange(value)}
            data-oid="kfo300e"
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
            data-oid="jm_.1o2"
          >
            <Input maxLength={ModelInstanceConsts.maxEndpointLength} showCount data-oid="2uqyk9c" />
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
            data-oid="sg_.vtt"
          >
            <Input
              maxLength={ModelInstanceConsts.maxAccessKeyLength}
              showCount
              data-oid="w_ye97f"
            />
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
            data-oid="e8-v.fe"
          >
            <Input
              maxLength={ModelInstanceConsts.maxSecretKeyLength}
              showCount
              data-oid="yw1e7vj"
            />
          </Form.Item>
        )}
        <Form.Item label={intl.formatMessage({ id: 'model.models' })} data-oid="i6twfht">
          <Form.List name="models" data-oid="0voty6m">
            {(fields, { add, remove }) => (
              <div className="flex flex-col gap-4" data-oid="zz9h4kq">
                {fields.map((field) => (
                  <ModelFormItem
                    data={data.values}
                    key={field.key}
                    field={field}
                    remove={(i: number) => fields.length > 1 && remove(i)}
                    intl={intl}
                    data-oid="k.i5w_t"
                  />
                ))}
                <Button type="dashed" onClick={() => add()} block data-oid="vrb-9ou">
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
