import { createAgent, updateAgent } from '@/services/aigc/agent';
import { Agent } from '@/types/agent';
import { useIntl } from '@umijs/max';
import type { UploadFile } from 'antd';
import { message, Form, Input, Upload } from 'antd';
import type { UploadChangeParam } from 'antd/lib/upload/interface';
import React, { useEffect, useState } from 'react';
import { ModalForm } from '@ant-design/pro-components';

interface AgentCreateModalProps {
  open: boolean;
  values?: Partial<Agent>;
  type?: 'create' | 'edit';
  onChange: (updates: Partial<Agent>) => void;
  onOpenChange: (visible: boolean) => void;
}

export const CreateModal: React.FC<AgentCreateModalProps> = ({ open, type = 'create', values, onChange, onOpenChange }) => {
  const intl = useIntl();
  const [form] = Form.useForm();
  const [iconPath, setIconPath] = useState(`/images/agent/default_icon${Math.floor(Math.random() * 6) + 1}.png`);
  const [isLoading, setIsLoading] = useState(false);
  useEffect(() => {
    if (open) {
      form.resetFields();
      if (type === 'edit' && values?.icon) {
        setIconPath(values.icon);
      }
      form.setFieldsValue({
        icon: iconPath,
      });
    }
  }, [open, iconPath]);

  const handleCreateOrUpdate = async () => {
    const values = await form.validateFields();
    try {
      setIsLoading(true);
      if (type === 'create') {
        await createAgent(values);
      } else {
        await updateAgent(values);
      }
      message.success(intl.formatMessage({ id: 'actions.success' }));
      onChange(values);
    } catch {
      message.error(intl.formatMessage({ id: 'actions.failed' }));
    } finally {
      setIsLoading(false);
    }
  };

  const handleIconChange = (info: UploadChangeParam<UploadFile<any>>) => {
    if (info.file.status === 'done' && info.file.response) {
      setIconPath(info.file.response.url);
    }
  };

  return (
    <ModalForm
      form={form}
      loading={isLoading}
      title={intl.formatMessage({ id: type === 'create' ? 'agent.createModal.title' : 'agent.editModal.title' })}
      open={open}
      onOpenChange={(visible) => {
        if (!visible) {
          form.resetFields();
        }
        onOpenChange(visible);
      }}
      onFinish={handleCreateOrUpdate}
      initialValues={values}
      width={480}
    >
      <Form.Item name="id" hidden={true}></Form.Item>
      <Form.Item
        name="name"
        label={intl.formatMessage({ id: 'agent.createModal.nameLabel' })}
        rules={[
          {
            required: true,
            message: intl.formatMessage({ id: 'agent.createModal.nameRequired' }),
          },
        ]}
      >
        <Input maxLength={20} showCount />
      </Form.Item>
      <Form.Item
        name="description"
        label={intl.formatMessage({ id: 'agent.createModal.descriptionLabel' })}
      >
        <Input.TextArea rows={4} maxLength={500} showCount />
      </Form.Item>
      <Form.Item
        name="icon"
        label={intl.formatMessage({ id: 'agent.createModal.iconLabel' })}
        rules={[
          {
            required: true,
            message: intl.formatMessage({ id: 'agent.createModal.iconRequired' }),
          },
        ]}
      >
        <Upload
          name="avatar"
          listType="picture-card"
          className="avatar-uploader"
          showUploadList={false}
          onChange={handleIconChange}
        >
          <img src={iconPath} alt="avatar" style={{ width: '100%' }} />
        </Upload>
      </Form.Item>
    </ModalForm>
  );
};

export default CreateModal;
