import { ClearOutlined, CloudUploadOutlined, PaperClipOutlined } from '@ant-design/icons';
import { Attachments, Prompts, Sender } from '@ant-design/x';
import { Button, Flex } from 'antd';
import { createStyles } from 'antd-style';
import React, { useState } from 'react';

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
      data-oid="7clbtp2"
    >
      <Attachments
        beforeUpload={() => false}
        items={attachedFiles}
        onChange={(info) => onAttachedFilesChange(info.fileList)}
        placeholder={(type) =>
          type === 'drop'
            ? { title: 'Drop file here' }
            : {
                icon: <CloudUploadOutlined data-oid="xah0x8v" />,
                title: 'Upload files',
                description: 'Click or drag files to this area to upload',
              }
        }
        data-oid="dwz1gnu"
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
        data-oid="0ki0cgw"
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
              icon={<ClearOutlined style={{ fontSize: 18 }} data-oid="4euvxx9" />}
              onClick={() => onClear()}
              disabled={loading}
              data-oid="uv0q37l"
            />

            <Button
              type="text"
              icon={<PaperClipOutlined style={{ fontSize: 18 }} data-oid="gscucus" />}
              onClick={() => setAttachmentsOpen(!attachmentsOpen)}
              disabled={loading}
              data-oid="2:2kq-s"
            />
          </>
        }
        loading={loading}
        className={styles.sender}
        allowSpeech
        actions={(_, info) => {
          const { SendButton, LoadingButton, SpeechButton } = info.components;
          return (
            <Flex gap={4} data-oid="-vcc1qi">
              <SpeechButton className={styles.speechButton} data-oid="c2mmlaa" />
              {loading ? (
                <LoadingButton type="default" data-oid="1xsk9-x" />
              ) : (
                <SendButton type="primary" data-oid=".z:f6j:" />
              )}
            </Flex>
          );
        }}
        data-oid="61fz00m"
      />
    </>
  );
};
