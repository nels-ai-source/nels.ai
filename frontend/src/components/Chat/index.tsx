import { message } from 'antd';
import { createStyles } from 'antd-style';
import React, { useRef, useState } from 'react';
import { Agent, ChatMessage, ChatCreatedContent, MessageContent, ChatFailedContent } from "@/types/agent";

import { ChatList } from './ChatList';
import { ChatSender } from './ChatSender';
import { useInvokeStreamingAsync } from '@/services/aigc/agentActuator';
import './markdown.less';

const useStyle = createStyles(({ token }) => ({
  layout: {
    width: '100%',
    height: 'calc(100vh - 80px)',
    display: 'flex',
    background: token.colorBgContainer,
    fontFamily: `AlibabaPuHuiTi, ${token.fontFamily}, sans-serif`,
  },
  chat: {
    width: '100%',
    margin: '0 auto',
    boxSizing: 'border-box',
    display: 'flex',
    flexDirection: 'column',
    paddingBottom: token.padding,
    gap: 16,
  },
}));

const Independent: React.FC<{ agentData: Agent }> = ({ agentData }) => {
  const { styles } = useStyle();
  const abortController = useRef<AbortController | null>(null);

  const [attachmentsOpen, setAttachmentsOpen] = useState(false);
  const [attachedFiles, setAttachedFiles] = useState([]);
  const [inputValue, setInputValue] = useState('');
  const [conversationId, setConversationId] = useState<string>('');
  const { agent, invokeChat } = useInvokeStreamingAsync();
  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const loading = agent.isRequesting();

  async function handleSubmit(val: string) {
    if (!val) return;

    if (loading) {
      message.error('Request is in progress, please wait for the request to complete.');
      return;
    }

    const userMessage: ChatMessage = { role: 'user', content: val };
    setMessages((prev) => [...prev, userMessage]);

    let currentMessageId: string | undefined;

    await invokeChat(
      {
        messages: [userMessage],
        agentId: agentData.id,
        conversationId: conversationId || null,
      },
      {
        onChatCreated: (data: ChatCreatedContent) => {
          setConversationId(data.conversationId);
          currentMessageId = data.chatId;
        },
        onMessageDelta: (data: MessageContent) => {
          if (!currentMessageId) {
            console.warn('messageId is null, skipping message update');
            return;
          }

          setMessages(prevMessages => {
            const existingMessageIndex = prevMessages.findIndex(message => message.id === currentMessageId);

            if (existingMessageIndex !== -1) {
              const updatedMessages = [...prevMessages];
              updatedMessages[existingMessageIndex] = {
                ...updatedMessages[existingMessageIndex],
                content: (updatedMessages[existingMessageIndex].content || '') + data.content
              };
              return updatedMessages;
            } else {
              return [...prevMessages, {
                id: currentMessageId,
                role: data.role as 'user' | 'assistant' | 'system',
                content: data.content
              }];
            }
          });
        },
        onChatFailed: (data: ChatFailedContent) => {
          console.error('Chat failed:', data);
          message.error(`Chat failed: ${data.msg}`);
        },
        onError: (error) => {
          console.error('Chat error:', error);
          message.error('Chat request failed');
        },
      }
    );
  }

  const handleCancel = () => {
    abortController.current?.abort()
  };

  const handleClear = () => {
    console.log(messages);
    setMessages([]);
    setConversationId('');
  };

  return (
    <div className={styles.layout}>
      <div className={styles.chat}>
        <ChatList
          agent={agentData}
          messages={messages}
          onPromptClick={handleSubmit}
        />

        <ChatSender
          value={inputValue}
          onChange={setInputValue}
          onClear={handleClear}
          onSubmit={() => {
            handleSubmit(inputValue);
            setInputValue('');
          }}
          onCancel={handleCancel}
          attachedFiles={attachedFiles}
          onAttachedFilesChange={(files) => setAttachedFiles(files as never[])}
          onAttachmentsOpenChange={(open) => setAttachmentsOpen(open)}
          onPromptClick={handleSubmit}
          loading={loading}
        />
      </div>
    </div>
  );
};

export default Independent;
