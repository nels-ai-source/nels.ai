import { Bot } from '@/types/bot';
import { useIntl } from '@umijs/max';
import type { UploadFile } from 'antd';
import { Button, Form, Input, Modal, Upload } from 'antd';
import type { UploadChangeParam } from 'antd/lib/upload/interface';
import React, { useEffect, useState } from 'react';

interface BotCreateModalProps {
  open: boolean;
  onCancel: () => void;
  onCreateBot: (gallery: Bot) => void;
}

export const BotCreateModal: React.FC<BotCreateModalProps> = ({ open, onCancel, onCreateBot }) => {
  const intl = useIntl();
  const [form] = Form.useForm();
  const [iconPath, setIconPath] = useState(
    `/images/bot/default_bot_icon${Math.floor(Math.random() * 6) + 1}.png`,
  );
  const [bot] = useState<Bot>();

  useEffect(() => {
    if (open) {
      form.resetFields();
    }
  }, [open, iconPath]);
  const handleCreate = () => {
    form
      .validateFields()
      .then((values) => {
        onCreateBot({ ...values, icon: iconPath });
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
      title={intl.formatMessage({ id: 'bot.createModal.title' })}
      open={open}
      onCancel={onCancel}
      footer={[
        <Button key="cancel" onClick={onCancel}>
          {intl.formatMessage({ id: 'bot.actions.cancel' })}
        </Button>,
        <Button key="submit" type="primary" onClick={handleCreate}>
          {intl.formatMessage({ id: 'bot.actions.create' })}
        </Button>,
      ]}
      width={480}
    >
      <Form form={form} layout="vertical" initialValues={bot}>
        <Form.Item
          name="name"
          label={intl.formatMessage({ id: 'bot.createModal.nameLabel' })}
          rules={[
            {
              required: true,
              message: intl.formatMessage({ id: 'bot.createModal.nameRequired' }),
            },
          ]}
        >
          <Input maxLength={20} showCount />
        </Form.Item>
        <Form.Item
          name="description"
          label={intl.formatMessage({ id: 'bot.createModal.descriptionLabel' })}
        >
          <Input.TextArea rows={4} maxLength={500} showCount />
        </Form.Item>
        <Form.Item
          name="icon"
          label={intl.formatMessage({ id: 'bot.createModal.iconLabel' })}
          rules={[
            {
              required: true,
              message: intl.formatMessage({ id: 'bot.createModal.iconRequired' }),
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
            <img src={bot?.icon||"/images/bot/default_bot_icon3.png"} alt="avatar" style={{ width: '100%' }} />
          </Upload>
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default BotCreateModal;
