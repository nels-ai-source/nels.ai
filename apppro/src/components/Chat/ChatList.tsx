import {
  CopyOutlined,
  DislikeOutlined,
  EllipsisOutlined,
  LikeOutlined,
  ReloadOutlined,
  ShareAltOutlined,
} from '@ant-design/icons';
import { Bubble, Prompts, Welcome } from '@ant-design/x';
import { Button, Flex, Space, Spin } from 'antd';
import { createStyles } from 'antd-style';
import React from 'react';

const useStyle = createStyles(({}) => ({
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
  messages: any[];
  hotTopics: any;
  designGuide: any;
  onPromptClick: (text: string) => void;
}

export const ChatList: React.FC<ChatListProps> = ({
  messages,
  hotTopics,
  designGuide,
  onPromptClick,
}) => {
  const { styles } = useStyle();

  return (
    <div className={styles.chatList} data-oid="vulb6nf">
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
                <div style={{ display: 'flex' }} data-oid="1ln-2rc">
                  <Button
                    type="text"
                    size="small"
                    icon={<ReloadOutlined data-oid="p6pqden" />}
                    data-oid="s6b-:ky"
                  />

                  <Button
                    type="text"
                    size="small"
                    icon={<CopyOutlined data-oid="ar_dzx5" />}
                    data-oid="9k4fn83"
                  />

                  <Button
                    type="text"
                    size="small"
                    icon={<LikeOutlined data-oid="rfzniku" />}
                    data-oid="oxaz1v:"
                  />

                  <Button
                    type="text"
                    size="small"
                    icon={<DislikeOutlined data-oid=":37jsw4" />}
                    data-oid="530:dau"
                  />
                </div>
              ),

              loadingRender: () => <Spin size="small" data-oid="_um:0t_" />,
            },
            user: { placement: 'end' },
          }}
          data-oid="7_b__vh"
        />
      ) : (
        <Space direction="vertical" size={16} className={styles.placeholder} data-oid="sd.aml3">
          <Welcome
            variant="borderless"
            icon="https://mdn.alipayobjects.com/huamei_iwk9zp/afts/img/A*s5sNRo5LjfQAAAAAAAAAAAAADgCCAQ/fmt.webp"
            title="Hello, I'm Ant Design X"
            description="Base on Ant Design, AGI product interface solution, create a better intelligent vision~"
            extra={
              <Space data-oid="4eygvso">
                <Button icon={<ShareAltOutlined data-oid="h6y44kt" />} data-oid="a9.e-jr" />
                <Button icon={<EllipsisOutlined data-oid="j1wpfvt" />} data-oid="3afw:0y" />
              </Space>
            }
            data-oid="v-cn8e8"
          />

          <Flex gap={16} data-oid="dg_w6gd">
            <Prompts
              items={[hotTopics]}
              styles={{
                list: { height: '100%' },
                item: {
                  flex: 1,
                  backgroundImage: 'linear-gradient(123deg, #e5f4ff 0%, #efe7ff 100%)',
                  borderRadius: 12,
                  border: 'none',
                },
                subItem: {
                  padding: 0,
                  background: 'transparent',
                },
              }}
              onItemClick={(info) => {
                onPromptClick(info.data.description as string);
              }}
              className={styles.chatPrompt}
              data-oid="q:fmqq5"
            />

            <Prompts
              items={[designGuide]}
              styles={{
                item: {
                  flex: 1,
                  backgroundImage: 'linear-gradient(123deg, #e5f4ff 0%, #efe7ff 100%)',
                  borderRadius: 12,
                  border: 'none',
                },
                subItem: { background: '#ffffffa6' },
              }}
              onItemClick={(info) => {
                onPromptClick(info.data.description as string);
              }}
              className={styles.chatPrompt}
              data-oid="pzj:ed3"
            />
          </Flex>
        </Space>
      )}
    </div>
  );
};
