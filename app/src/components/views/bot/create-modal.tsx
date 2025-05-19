import React, { useState, useEffect } from 'react';
import { Modal, Input, Button, Upload, Form } from 'antd';
import type { UploadFile } from 'antd';
import type { UploadChangeParam } from 'antd/lib/upload/interface';
import { Bot } from '../../types/bot';
import { useTranslation } from 'react-i18next';
import { v4 as uuidv4 } from 'uuid';
import { UUID } from 'crypto';
interface BotCreateModalProps {
    open: boolean;
    onCancel: () => void;
    onCreateBot: (gallery: Bot) => void;
}

export const BotCreateModal: React.FC<BotCreateModalProps> = ({
    open,
    onCancel,
    onCreateBot,
}) => {
    const { t } = useTranslation();
    const [form] = Form.useForm();
    const [iconPath, setIconPath] = useState(
        `/images/bot/default_bot_icon${Math.floor(Math.random() * 6) + 1}.png`
    );
    const [bot, setBot] = useState<Bot>({
        id: uuidv4() as UUID,
        name: '',
        description: '',
        icon: iconPath,
        instructions: '',
        prologue: '',
        suggestedQuestions: [],
        tools: [],
        knowledges: [],
        isDeleted: false,
        creationTime: new Date(),
    });

    useEffect(() => {
        if (open) {
            form.resetFields();
        }
    }, [open, iconPath]);
    const handleCreate = () => {
        form.validateFields()
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
            title={t('bot.createModal.title')}
            open={open}
            onCancel={onCancel}
            footer={[
                <Button key="cancel" onClick={onCancel}>
                    {t('bot.actions.cancel')}
                </Button>,
                <Button key="submit" type="primary" onClick={handleCreate}>
                    {t('bot.actions.create')}
                </Button>,
            ]}
            width={480}
        >
            <Form form={form} layout="vertical" initialValues={bot}>
                <Form.Item
                    name="name"
                    label={t('bot.createModal.nameLabel')}
                    rules={[
                        {
                            required: true,
                            message: t('bot.createModal.nameRequired'),
                        },
                    ]}
                >
                    <Input maxLength={20} showCount />
                </Form.Item>
                <Form.Item
                    name="description"
                    label={t('bot.createModal.descriptionLabel')}
                >
                    <Input.TextArea rows={4} maxLength={500} showCount />
                </Form.Item>
                <Form.Item
                    name="icon"
                    label={t('bot.createModal.iconLabel')}
                    rules={[
                        {
                            required: true,
                            message: t('bot.createModal.iconRequired'),
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
                        <img
                            src={bot.icon}
                            alt="avatar"
                            style={{ width: '100%' }}
                        />
                    </Upload>
                </Form.Item>
            </Form>
        </Modal>
    );
};

export default BotCreateModal;
