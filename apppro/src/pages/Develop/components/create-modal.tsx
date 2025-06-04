import { Agent } from '@/types/agent';
import { useIntl } from '@umijs/max';
import type { UploadFile } from 'antd';
import { Button, Form, Input, Modal, Upload } from 'antd';
import type { UploadChangeParam } from 'antd/lib/upload/interface';
import React, { useEffect, useState } from 'react';

interface AgentCreateModalProps {
  open: boolean;
  onCancel: () => void;
  onCreateAgent: (gallery: Agent) => void;
}

export const CreateModal: React.FC<AgentCreateModalProps> = ({
  open,
  onCancel,
  onCreateAgent,
}) => {
  const intl = useIntl();
  const [form] = Form.useForm();
  const [iconPath, setIconPath] = useState(
    `/images/agent/default_icon${Math.floor(Math.random() * 6) + 1}.png`,
  );
  const [agent] = useState<Agent>();

  useEffect(() => {
    if (open) {
      form.resetFields();
    }
  }, [open, iconPath]);
  const handleCreate = () => {
    form
      .validateFields()
      .then((values) => {
        onCreateAgent({ ...values, icon: iconPath });
        form.resetFields();
      })
      .catch((info) => {
        console.log('Validate Failed:', info);
      });
  };
  const handleIconChange = (info: UploadChangeParam<UploadFile<any>>) => {
    if (info.file.status === 'done' && info.file.response) {
      setIconPath(info.file.response.url);
    }
  };

  return (
    <Modal
      title={intl.formatMessage({ id: 'agent.createModal.title' })}
      open={open}
      onCancel={onCancel}
      footer={[
        <Button key="cancel" onClick={onCancel} data-oid="ez-55.e">
          {intl.formatMessage({ id: 'agent.actions.cancel' })}
        </Button>,
        <Button key="submit" type="primary" onClick={handleCreate} data-oid="q95em4g">
          {intl.formatMessage({ id: 'agent.actions.create' })}
        </Button>,
      ]}
      width={480}
      data-oid="xh8s38u"
    >
      <Form form={form} layout="vertical" initialValues={agent} data-oid="jjm5qwz">
        <Form.Item
          name="name"
          label={intl.formatMessage({ id: 'agent.createModal.nameLabel' })}
          rules={[
            {
              required: true,
              message: intl.formatMessage({ id: 'agent.createModal.nameRequired' }),
            },
          ]}
          data-oid="xaeawek"
        >
          <Input maxLength={20} showCount data-oid="h9ygztc" />
        </Form.Item>
        <Form.Item
          name="description"
          label={intl.formatMessage({ id: 'agent.createModal.descriptionLabel' })}
          data-oid="3xf5ht."
        >
          <Input.TextArea rows={4} maxLength={500} showCount data-oid="0oedfc7" />
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
          data-oid="o0_x13p"
        >
          <Upload
            name="avatar"
            listType="picture-card"
            className="avatar-uploader"
            showUploadList={false}
            onChange={handleIconChange}
            data-oid="wkwllx4"
          >
            <img
              src={agent?.icon || '/images/agent/default_icon3.png'}
              alt="avatar"
              style={{ width: '100%' }}
              data-oid="c_b-v8c"
            />
          </Upload>
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default CreateModal;
