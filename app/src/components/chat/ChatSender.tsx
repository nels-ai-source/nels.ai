import React, { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Button, Flex } from 'antd';
import { Sender, Attachments, Prompts } from '@ant-design/x';
import {
    CloudUploadOutlined,
    ClearOutlined,
    PaperClipOutlined,
} from '@ant-design/icons';
import { createStyles } from 'antd-style';

const useStyle = createStyles(({ token }) => ({
    sender: {
        boxShadow: token.boxShadow,
        color: token.colorText,
    },
    speechButton: {
        fontSize: 18,
        color: `${token.colorText} !important`,
    },
    senderPrompt: {
        color: token.colorText,
    },
}));

interface ChatSenderProps {
    value: string;
    onChange: (value: string) => void;
    onClear: () => void;
    onSubmit: () => void;
    onCancel: () => void;
    onAttachmentsOpenChange: (open: boolean) => void;
    attachedFiles: any[];
    onAttachedFilesChange: (files: any[]) => void;
    senderPrompts: any[];
    onPromptClick: (text: string) => void;
    loading: boolean;
}

export const ChatSender: React.FC<ChatSenderProps> = ({
    value,
    onChange,
    onClear,
    onSubmit,
    onCancel,
    onAttachmentsOpenChange,
    attachedFiles,
    onAttachedFilesChange,
    senderPrompts,
    onPromptClick,
    loading,
}) => {
    const { styles } = useStyle();
    const [attachmentsOpen, setAttachmentsOpen] = useState(false);
    const senderHeader = (
        <Sender.Header
            title="Upload File"
            open={attachmentsOpen}
            onOpenChange={onAttachmentsOpenChange}
            styles={{ content: { padding: 0 } }}
        >
            <Attachments
                beforeUpload={() => false}
                items={attachedFiles}
                onChange={(info) => onAttachedFilesChange(info.fileList)}
                placeholder={(type) =>
                    type === 'drop'
                        ? { title: 'Drop file here' }
                        : {
                              icon: <CloudUploadOutlined />,
                              title: 'Upload files',
                              description:
                                  'Click or drag files to this area to upload',
                          }
                }
            />
        </Sender.Header>
    );

    return (
        <>
            <Prompts
                items={senderPrompts}
                onItemClick={(info) => {
                    onPromptClick(info.data.description as string);
                }}
                styles={{ item: { padding: '6px 12px' } }}
                className={styles.senderPrompt}
            />
            <Sender
                value={value}
                header={senderHeader}
                onSubmit={onSubmit}
                onChange={onChange}
                onCancel={onCancel}
                prefix={
                    <>
                        <Button
                            type="text"
                            icon={<ClearOutlined style={{ fontSize: 18 }} />}
                            onClick={() => onClear()}
                            disabled={loading}
                        />
                        <Button
                            type="text"
                            icon={
                                <PaperClipOutlined style={{ fontSize: 18 }} />
                            }
                            onClick={() => setAttachmentsOpen(!attachmentsOpen)}
                            disabled={loading}
                        />
                    </>
                }
                loading={loading}
                className={styles.sender}
                allowSpeech
                actions={(_, info) => {
                    const { SendButton, LoadingButton, SpeechButton } =
                        info.components;
                    return (
                        <Flex gap={4}>
                            <SpeechButton className={styles.speechButton} />
                            {loading ? (
                                <LoadingButton type="default" />
                            ) : (
                                <SendButton type="primary" />
                            )}
                        </Flex>
                    );
                }}
            />
        </>
    );
};
