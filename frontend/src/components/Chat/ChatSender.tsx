import { ClearOutlined, CloudUploadOutlined, PaperClipOutlined } from '@ant-design/icons';
import { Attachments, Sender } from '@ant-design/x';
import { Button, Flex } from 'antd';
import { createStyles } from 'antd-style';
import React, { useState } from 'react';
import { useIntl } from 'umi';

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
  loading,
}) => {
  const { styles } = useStyle();
  const intl = useIntl();
  const [attachmentsOpen, setAttachmentsOpen] = useState(false);
  const senderHeader = (
    <Sender.Header
      title={intl.formatMessage({ id: 'component.chatSender.uploadFile' })}
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
            ? { title: intl.formatMessage({ id: 'component.chatSender.dropFileHere' }) }
            : {
              icon: <CloudUploadOutlined />,
              title: intl.formatMessage({ id: 'component.chatSender.uploadFiles' }),
              description: intl.formatMessage({ id: 'component.chatSender.uploadDescription' }),
            }
        }

      />
    </Sender.Header>
  );

  return (
    <>
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
              icon={<PaperClipOutlined style={{ fontSize: 18 }} />}
              onClick={() => setAttachmentsOpen(!attachmentsOpen)}
              disabled={loading}

            />
          </>
        }
        loading={loading}
        className={styles.sender}
        allowSpeech
        actions={(_, info) => {
          const { SendButton, LoadingButton, SpeechButton } = info.components;
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
