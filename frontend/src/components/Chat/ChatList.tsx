import {
  CopyOutlined,
  DislikeOutlined,
  LikeOutlined,
  ReloadOutlined,
} from '@ant-design/icons';
import { Bubble } from '@ant-design/x';
import { Button, Spin } from 'antd';
import { createStyles } from 'antd-style';
import { Agent } from '@/types/agent';
import React from 'react';
import Markdown from '../Markdown';
import { AgentWelcome } from './AgentWelcome';

const useStyle = createStyles(({ }) => ({
  chatList: {
    flex: 1,
    overflow: 'auto',
    paddingRight: 10,
  },
  loadingMessage: {
    backgroundImage: 'linear-gradient(90deg, #ff6b23 0%, #af3cb8 31%, #53b6ff 89%)',
    backgroundSize: '100% 2px',
    backgroundRepeat: 'no-repeat',
    backgroundPosition: 'bottom',
  },
  placeholder: {
    paddingTop: 32,
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    overflow: 'hidden',
    height: '100%',
  },
}));

interface ChatListProps {
  agent: Agent | null;
  messages: any[];
  onPromptClick: (text: string) => void;
}

export const ChatList: React.FC<ChatListProps> = ({
  agent,
  messages,
  onPromptClick,
}) => {
  const { styles } = useStyle();
  const renderMessageContent = (content: string, role: string, messageId: string) => {
    if (role === 'assistant') {
      return (
        <Markdown content={content} id={messageId} />
      );
    }
    return content;
  };

  return (
    <div className={styles.chatList}>
      {messages?.length ? (
        <Bubble.List
          items={messages?.map((i, index) => {
            const messageId = `message-${index}-${i.role}`;
            return {
              key: messageId,
              role: i.role,
              content: renderMessageContent(i.content, i.role, messageId),
              classNames: {
                content: i.status === 'loading' ? styles.loadingMessage : '',
              },
              typing: i.status === 'loading' ? { step: 5, interval: 20, suffix: <>💗</> } : false,
            };
          })}
          style={{ height: '100%' }}
          roles={{
            assistant: {
              placement: 'start',
              footer: (
                <div style={{ display: 'flex' }}>
                  <Button
                    type="text"
                    size="small"
                    icon={<ReloadOutlined />}
                  />

                  <Button
                    type="text"
                    size="small"
                    icon={<CopyOutlined />}
                  />

                  <Button
                    type="text"
                    size="small"
                    icon={<LikeOutlined />}
                  />

                  <Button
                    type="text"
                    size="small"
                    icon={<DislikeOutlined />}
                  />
                </div>
              ),

              loadingRender: () => <Spin size="small" />,
            },
            user: { placement: 'end' },
          }}

        />
      ) : (
        agent && <div className={styles.placeholder}>
          <AgentWelcome agent={agent} onPromptClick={onPromptClick} />
        </div>
      )}
    </div>
  );
};
