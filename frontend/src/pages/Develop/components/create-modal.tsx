import React, { useEffect, useState } from 'react';
import { Form, Input, Upload, App } from 'antd';
import type { UploadFile } from 'antd';
import { ModalForm } from '@ant-design/pro-components';
import { useIntl } from '@umijs/max';
import { createAgent, updateAgent } from '@/services/aigc/agent';
import type { Agent } from '@/types/agent';
import type { UploadChangeParam } from 'antd/lib/upload/interface';

interface CreateModalProps {
  open: boolean;
  values?: Partial<Agent>;
  type?: 'create' | 'edit';
  onOpenChange: (visible: boolean) => void;
  onSuccess: () => void;
  width?: number;
}

export const CreateModal: React.FC<CreateModalProps> = ({
  open,
  type = 'create',
  values,
  onOpenChange,
  onSuccess,
  width = 600,
}) => {
  const { message } = App.useApp();
  const intl = useIntl();
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const [iconPath, setIconPath] = useState(`/images/agent/default_icon${Math.floor(Math.random() * 6) + 1}.png`);

  useEffect(() => {
    if (open) {
      if (type === 'edit' && values) {
        form.setFieldsValue(values);
        if (values.icon) {
          setIconPath(values.icon);
        }
      } else {
        form.resetFields();
        form.setFieldsValue({ icon: iconPath });
      }
    }
  }, [open, type, values, form, iconPath]);

  const handleSubmit = async (formValues: any) => {
    try {
      setLoading(true);

      if (type === 'create') {
        await createAgent(formValues);
      } else {
        await updateAgent(formValues);
      }

      message.success(intl.formatMessage({ id: 'actions.success' }));
      onSuccess();
      onOpenChange(false);
      return true;
    } catch (error) {
      console.error('Submit error:', error);
      message.error(intl.formatMessage({ id: 'actions.failed' }));
      return false;
    } finally {
      setLoading(false);
    }
  };

  const handleIconChange = (info: UploadChangeParam<UploadFile<any>>) => {
    if (info.file.status === 'done' && info.file.response) {
      setIconPath(info.file.response.url);
    }
  };

  const handleOpenChange = (visible: boolean) => {
    if (!visible) {
      onOpenChange(false);
    }
  };

  return (
    <ModalForm
      form={form}
      title={intl.formatMessage({
        id: type === 'create' ? 'agent.createModal.title' : 'agent.editModal.title'
      })}
      open={open}
      onOpenChange={handleOpenChange}
      onFinish={handleSubmit}
      loading={loading}
      width={width}
      modalProps={{
        destroyOnHidden: true,
      }}
    >
      <Form.Item name="id" hidden>
        <Input />
      </Form.Item>

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