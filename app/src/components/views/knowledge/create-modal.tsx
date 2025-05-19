import React, { useState, useEffect } from 'react';
import { Modal, Input, Button, Upload, Form } from 'antd';
import type { UploadFile } from 'antd';
import type { UploadChangeParam } from 'antd/lib/upload/interface';
import { Knowledge } from '../../types/knowledge';
import { useTranslation } from 'react-i18next';
import { v4 as uuidv4 } from 'uuid';
import { UUID } from 'crypto';
interface knowledgeCreateModalProps {
    open: boolean;
    onCancel: () => void;
    onCreateknowledge: (data: Knowledge) => void;
}

export const KnowledgeCreateModal: React.FC<knowledgeCreateModalProps> = ({
    open,
    onCancel,
    onCreateknowledge,
}) => {
    const { t } = useTranslation();
    const [form] = Form.useForm();
    const [iconPath, setIconPath] = useState(
        `/images/knowledge/default_knowledge_icon${
            Math.floor(Math.random() * 6) + 1
        }.png`
    );
    const [knowledge, setknowledge] = useState<Knowledge>({
        id: uuidv4() as UUID,
        name: '',
        type: 'text',
        isEnabled: true,
        isDeleted: false,
        description: '',
        creationTime: new Date(),
        icon: '',
    });

    useEffect(() => {
        if (open) {
            form.resetFields();
        }
    }, [open, iconPath]);
    const handleCreate = () => {
        form.validateFields()
            .then((values) => {
                onCreateknowledge({ ...values, icon: iconPath });
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
            title={t('knowledge.createModal.title')}
            open={open}
            onCancel={onCancel}
            footer={[
                <Button key="cancel" onClick={onCancel}>
                    {t('knowledge.actions.cancel')}
                </Button>,
                <Button key="submit" type="primary" onClick={handleCreate}>
                    {t('knowledge.actions.create')}
                </Button>,
            ]}
            width={480}
        >
            <Form form={form} layout="vertical" initialValues={knowledge}>
                <Form.Item
                    name="name"
                    label={t('knowledge.createModal.nameLabel')}
                    rules={[
                        {
                            required: true,
                            message: t('knowledge.createModal.nameRequired'),
                        },
                    ]}
                >
                    <Input maxLength={20} showCount />
                </Form.Item>
                <Form.Item
                    name="description"
                    label={t('knowledge.createModal.descriptionLabel')}
                >
                    <Input.TextArea rows={4} maxLength={500} showCount />
                </Form.Item>
                <Form.Item
                    name="icon"
                    label={t('knowledge.createModal.iconLabel')}
                    rules={[
                        {
                            required: true,
                            message: t('knowledge.createModal.iconRequired'),
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
                            src={knowledge.icon}
                            alt="avatar"
                            style={{ width: '100%' }}
                        />
                    </Upload>
                </Form.Item>
            </Form>
        </Modal>
    );
};

export default KnowledgeCreateModal;
