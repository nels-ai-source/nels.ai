import { updateModel } from '@/services/aigc/model';
import { Model, ModelCapability, ModelProvider, ModelType, ProviderConfig } from '@/types/model';
import { useIntl } from '@umijs/max';
import { Button, Form, Input, Modal, Select } from 'antd';
import React, { useEffect, useState } from 'react';
import { getEnumLabel, getProviderIcon } from '../util';

interface EditModalProps {
  open: boolean;
  model: Model;
  onCancel: () => void;
  onChange: (result: boolean) => void;
}

export const EditModal: React.FC<EditModalProps> = ({ open, model, onCancel, onChange }) => {
  const intl = useIntl();
  const [form] = Form.useForm();
  const [isLoading, setIsLoading] = useState(false);
  const [data, setData] = useState<Model>(model);

  useEffect(() => {
    if (open) {
      form.setFieldsValue({
        ...model,
        type: model.type,
        modelCapabilities: model.modelCapabilities,
      });
      setData(model);
    }
  }, [open, model]);

  const handleEdit = async () => {
    try {
      setIsLoading(true);
      const values = await form.validateFields();
      const updatedModel: Model = {
        ...model,
        ...values,
      };
      await updateModel(updatedModel);
      onChange(true);
    } catch (error) {
      console.error('Failed to update model:', error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Modal
      title={intl.formatMessage({ id: 'model.editModalTitle' })}
      open={open}
      onCancel={onCancel}
      footer={[
        <Button key="cancel" onClick={onCancel} loading={isLoading} data-oid="3l2v6:8">
          {intl.formatMessage({ id: 'actions.cancel' })}
        </Button>,
        <Button
          key="submit"
          type="primary"
          onClick={handleEdit}
          loading={isLoading}
          data-oid="vdq0k6v"
        >
          {intl.formatMessage({ id: 'actions.save' })}
        </Button>,
      ]}
      width={800}
      data-oid="79gq236"
    >
      <Form form={form} layout="vertical" initialValues={data} data-oid="17gj:bn">
        <Form.Item label={intl.formatMessage({ id: 'model.provider' })} data-oid="y1-c3td">
          <div className="flex items-center gap-2" data-oid="hqwj4ij">
            <img
              src={getProviderIcon(data.provider)}
              className="w-6 h-6 object-cover"
              alt={ModelProvider[data.provider]}
              data-oid="xjy0csh"
            />

            <span data-oid="a7lctqr">
              {intl.formatMessage(getEnumLabel(ModelProvider, 'ModelProvider', data.provider))}
            </span>
          </div>
        </Form.Item>

        <Form.Item
          name="type"
          label={intl.formatMessage({ id: 'model.type' })}
          rules={[{ required: true, message: intl.formatMessage({ id: 'model.required.type' }) }]}
          data-oid="62b-:q8"
        >
          <Select data-oid="bwe512n">
            {Object.entries(ModelType)
              .filter(([key]) => isNaN(Number(key)))
              .map(([key, value]) => (
                <Select.Option key={value} value={value} data-oid="9l:h_1i">
                  {intl.formatMessage({ id: `ModelType.${key}` })}
                </Select.Option>
              ))}
          </Select>
        </Form.Item>

        <Form.Item
          name="name"
          label={intl.formatMessage({ id: 'model.name' })}
          rules={[{ required: true, message: intl.formatMessage({ id: 'model.required.name' }) }]}
          data-oid="lrg5bw4"
        >
          <Input data-oid="gu2go-o" />
        </Form.Item>

        {ProviderConfig[data.provider]?.attributes.includes('endpoint') && (
          <Form.Item
            name="endpoint"
            label={intl.formatMessage({ id: 'model.endpoint' })}
            rules={[
              {
                required: true,
                message: intl.formatMessage({ id: 'model.required.endpoint' }),
              },
            ]}
            data-oid="0mo3yln"
          >
            <Input data-oid="-l.d4_0" />
          </Form.Item>
        )}

        {ProviderConfig[data.provider]?.attributes.includes('deploymentName') && (
          <Form.Item
            name="deploymentName"
            label={intl.formatMessage({ id: 'model.deploymentName' })}
            rules={[
              {
                required: true,
                message: intl.formatMessage({ id: 'model.required.deploymentName' }),
              },
            ]}
            data-oid="t880uia"
          >
            <Input data-oid="-l3662b" />
          </Form.Item>
        )}

        <Form.Item
          name="maxTokens"
          label={intl.formatMessage({ id: 'model.maxTokens' })}
          rules={[
            {
              required: true,
              message: intl.formatMessage({ id: 'model.required.maxTokens' }),
            },
          ]}
          data-oid="l8o8fp6"
        >
          <Input type="number" min={0} suffix="k" data-oid="ct786df" />
        </Form.Item>

        <Form.Item
          name="modelCapabilities"
          label={intl.formatMessage({ id: 'model.capabilities' })}
          rules={[
            {
              required: true,
              message: intl.formatMessage({ id: 'model.required.capabilities' }),
            },
          ]}
          data-oid="ru5dq.l"
        >
          <Select
            mode="multiple"
            placeholder={intl.formatMessage({ id: 'model.capabilities' })}
            maxTagCount={5}
            data-oid="lmp:3z0"
          >
            {Object.entries(ModelCapability)
              .filter(([key]) => isNaN(Number(key)))
              .map(([key, value]) => (
                <Select.Option key={value} value={value} data-oid="e2jqfml">
                  {intl.formatMessage({ id: `ModelCapability.${key}` })}
                </Select.Option>
              ))}
          </Select>
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default EditModal;
