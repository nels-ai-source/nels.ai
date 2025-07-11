import {
  CopyOutlined,
  DislikeOutlined,
  LikeOutlined,
  ReloadOutlined,
} from '@ant-design/icons';
import { Bubble, Prompts, Welcome } from '@ant-design/x';
import { Button, Flex, Space, Spin } from 'antd';
import { createStyles } from 'antd-style';
import { Agent } from '@/types/agent';
import React from 'react';

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
  },
  chatPrompt: {
    '.ant-prompts-label': {
      color: '#000000e0 !important',
    },
    '.ant-prompts-desc': {
      color: '#000000a6 !important',
      width: '100%',
    },
    '.ant-prompts-icon': {
      color: '#000000a6 !important',
    },
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

  return (
    <div className={styles.chatList}>
      {messages?.length ? (
        <Bubble.List
          items={messages?.map((i) => ({
            ...i.message,
            classNames: {
              content: i.status === 'loading' ? styles.loadingMessage : '',
            },
            typing: i.status === 'loading' ? { step: 5, interval: 20, suffix: <>💗</> } : false,
          }))}
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
        agent && <Space direction="vertical" size={16} className={styles.placeholder}>
          <Welcome
            variant="borderless"
            icon={<img src={agent.icon} className="rounded-lg object-cover" />}
            title={agent.name}
            description={agent.prologue}
          />
          <Prompts vertical
            items={agent.questions.map((question) => ({
              label: question.content,
              value: question.content,
              key: question.id
            }))}
          />
        </Space>
      )}
    </div>
  );
};
