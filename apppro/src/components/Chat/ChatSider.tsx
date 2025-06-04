import {
  DeleteOutlined,
  EditOutlined,
  PlusOutlined,
  QuestionCircleOutlined,
} from '@ant-design/icons';
import { Conversations } from '@ant-design/x';
import { Avatar, Button } from 'antd';
import { createStyles } from 'antd-style';
import React from 'react';

const useStyle = createStyles(({ token }) => ({
  sider: {
    background: `${token.colorBgLayout}80`,
    width: 280,
    height: '100%',
    display: 'flex',
    flexDirection: 'column',
    padding: '0 12px',
    boxSizing: 'border-box',
  },
  logo: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'start',
    padding: '0 24px',
    boxSizing: 'border-box',
    gap: 8,
    margin: '24px 0',

    span: {
      fontWeight: 'bold',
      color: token.colorText,
      fontSize: 16,
    },
  },
  addBtn: {
    background: '#1677ff0f',
    border: '1px solid #1677ff34',
    height: 40,
  },
  conversations: {
    flex: 1,
    overflowY: 'auto',
    marginTop: 12,
    padding: 0,

    '.ant-conversations-list': {
      paddingInlineStart: 0,
    },
  },
  siderFooter: {
    borderTop: `1px solid ${token.colorBorderSecondary}`,
    height: 40,
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'space-between',
  },
}));

interface ChatSiderProps {
  conversations: any[];
  curConversation: string;
  onConversationChange: (key: string) => void;
  onNewConversation: () => void;
  onDeleteConversation: (key: string) => void;
}

export const ChatSider: React.FC<ChatSiderProps> = ({
  conversations,
  curConversation,
  onConversationChange,
  onNewConversation,
  onDeleteConversation,
}) => {
  const { styles } = useStyle();

  return (
    <div className={styles.sider} data-oid="gom_oon">
      <div className={styles.logo} data-oid="_tekv04">
        <img
          src="https://mdn.alipayobjects.com/huamei_iwk9zp/afts/img/A*eco6RrQhxbMAAAAAAAAAAAAADgCCAQ/original"
          draggable={false}
          alt="logo"
          width={24}
          height={24}
          data-oid="4dr.o8-"
        />

        <span data-oid="f66u1t.">Ant Design X</span>
      </div>

      <Button
        onClick={onNewConversation}
        type="link"
        className={styles.addBtn}
        icon={<PlusOutlined data-oid="ag4yk74" />}
        data-oid="5_8k5_8"
      >
        New Conversation
      </Button>

      <Conversations
        items={conversations}
        className={styles.conversations}
        activeKey={curConversation}
        onActiveChange={onConversationChange}
        groupable
        styles={{ item: { padding: '0 8px' } }}
        menu={(conversation) => ({
          items: [
            {
              label: 'Rename',
              key: 'rename',
              icon: <EditOutlined data-oid="s1.t1pq" />,
            },
            {
              label: 'Delete',
              key: 'delete',
              icon: <DeleteOutlined data-oid="vcdlrl7" />,
              danger: true,
              onClick: () => onDeleteConversation(conversation.key),
            },
          ],
        })}
        data-oid="qljbox4"
      />

      <div className={styles.siderFooter} data-oid="zk.io46">
        <Avatar size={24} data-oid="85-9g82" />
        <Button
          type="text"
          icon={<QuestionCircleOutlined data-oid="qhmybxz" />}
          data-oid="h-sbpd7"
        />
      </div>
    </div>
  );
};
