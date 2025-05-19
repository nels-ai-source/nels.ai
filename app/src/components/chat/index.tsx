import React, { useRef, useState } from 'react';
import { message } from 'antd';
import {
    HeartOutlined,
    SmileOutlined,
    CommentOutlined,
    PaperClipOutlined,
    ScheduleOutlined,
    ProductOutlined,
    FileSearchOutlined,
    AppstoreAddOutlined,
} from '@ant-design/icons';
import { useXAgent, useXChat } from '@ant-design/x';
import type { BubbleDataType } from '@ant-design/x/es/bubble/BubbleList';
import { createStyles } from 'antd-style';
import dayjs from 'dayjs';

import { ChatSider } from './ChatSider';
import { ChatList } from './ChatList';
import { ChatSender } from './ChatSender';

const DEFAULT_CONVERSATIONS_ITEMS = [
    {
        key: 'default-0',
        label: 'What is Ant Design X?',
        group: 'Today',
    },
    {
        key: 'default-1',
        label: 'How to quickly install and import components?',
        group: 'Today',
    },
    {
        key: 'default-2',
        label: 'New AGI Hybrid Interface',
        group: 'Yesterday',
    },
];

const HOT_TOPICS = {
    key: '1',
    label: 'Hot Topics',
    children: [
        {
            key: '1-1',
            description: 'What has Ant Design X upgraded?',
            icon: <span style={{ color: '#f93a4a', fontWeight: 700 }}>1</span>,
        },
        {
            key: '1-2',
            description: 'New AGI Hybrid Interface',
            icon: <span style={{ color: '#ff6565', fontWeight: 700 }}>2</span>,
        },
        {
            key: '1-3',
            description: 'What components are in Ant Design X?',
            icon: <span style={{ color: '#ff8f1f', fontWeight: 700 }}>3</span>,
        },
        {
            key: '1-4',
            description:
                'Come and discover the new design paradigm of the AI era.',
            icon: (
                <span style={{ color: '#00000040', fontWeight: 700 }}>4</span>
            ),
        },
        {
            key: '1-5',
            description: 'How to quickly install and import components?',
            icon: (
                <span style={{ color: '#00000040', fontWeight: 700 }}>5</span>
            ),
        },
    ],
};

const DESIGN_GUIDE = {
    key: '2',
    label: 'Design Guide',
    children: [
        {
            key: '2-1',
            icon: <HeartOutlined />,
            label: 'Intention',
            description: 'AI understands user needs and provides solutions.',
        },
        {
            key: '2-2',
            icon: <SmileOutlined />,
            label: 'Role',
            description: "AI's public persona and image",
        },
        {
            key: '2-3',
            icon: <CommentOutlined />,
            label: 'Chat',
            description: 'How AI Can Express Itself in a Way Users Understand',
        },
        {
            key: '2-4',
            icon: <PaperClipOutlined />,
            label: 'Interface',
            description: 'AI balances "chat" & "do" behaviors.',
        },
    ],
};

const SENDER_PROMPTS = [
    {
        key: '1',
        description: 'Upgrades',
        icon: <ScheduleOutlined />,
    },
    {
        key: '2',
        description: 'Components',
        icon: <ProductOutlined />,
    },
    {
        key: '3',
        description: 'RICH Guide',
        icon: <FileSearchOutlined />,
    },
    {
        key: '4',
        description: 'Installation Introduction',
        icon: <AppstoreAddOutlined />,
    },
];

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

const Independent: React.FC = () => {
    const { styles } = useStyle();
    const abortController = useRef<AbortController | null>(null);

    const [messageHistory, setMessageHistory] = useState<Record<string, any>>(
        {}
    );
    const [conversations, setConversations] = useState(
        DEFAULT_CONVERSATIONS_ITEMS
    );
    const [curConversation, setCurConversation] = useState(
        DEFAULT_CONVERSATIONS_ITEMS[0].key
    );
    const [attachmentsOpen, setAttachmentsOpen] = useState(false);
    const [attachedFiles, setAttachedFiles] = useState([]);
    const [inputValue, setInputValue] = useState('');

    const [agent] = useXAgent<BubbleDataType>({
        baseURL: 'https://api.siliconflow.cn/v1/chat/completions',
        model: 'deepseek-ai/DeepSeek-R1-Distill-Qwen-7B',
        dangerouslyApiKey:
            'Bearer sk-ravoadhrquyrkvaqsgyeufqdgphwxfheifujmaoscudjgldr',
    });
    const loading = agent.isRequesting();

    const { onRequest, messages, setMessages } = useXChat({
        agent,
        requestFallback: (_, { error }) => {
            if (error.name === 'AbortError') {
                return { content: 'Request is aborted', role: 'assistant' };
            }
            return {
                content: 'Request failed, please try again!',
                role: 'assistant',
            };
        },
        transformMessage: (info) => {
            const { originMessage, chunk } = info || {};
            let currentText = '';
            try {
                if (chunk?.data && !chunk?.data.includes('DONE')) {
                    const message = JSON.parse(chunk?.data);
                    currentText = !message?.choices?.[0].delta
                        ?.reasoning_content
                        ? ''
                        : message?.choices?.[0].delta?.reasoning_content;
                }
            } catch (error) {
                console.error(error);
            }
            return {
                content: (originMessage?.content || '') + currentText,
                role: 'assistant',
            };
        },
        resolveAbortController: (controller) => {
            abortController.current = controller;
        },
    });

    const handleSubmit = (val: string) => {
        if (!val) return;

        if (loading) {
            message.error(
                'Request is in progress, please wait for the request to complete.'
            );
            return;
        }

        onRequest({
            stream: true,
            message: { role: 'user', content: val },
        });
    };

    const handleConversationChange = async (val: string) => {
        abortController.current?.abort();
        setTimeout(() => {
            setCurConversation(val);
            setMessages(messageHistory?.[val] || []);
        }, 100);
    };

    const handleNewConversation = () => {
        const now = dayjs().valueOf().toString();
        setConversations([
            {
                key: now,
                label: `New Conversation ${conversations.length + 1}`,
                group: 'Today',
            },
            ...conversations,
        ]);
        setCurConversation(now);
        setMessages([]);
    };

    const handleDeleteConversation = (key: string) => {
        const newList = conversations.filter((item) => item.key !== key);
        const newKey = newList?.[0]?.key;
        setConversations(newList);
        setTimeout(() => {
            if (key === curConversation) {
                setCurConversation(newKey);
                setMessages(messageHistory?.[newKey] || []);
            }
        }, 200);
    };

    return (
        <div className={styles.layout}>
            {/* <ChatSider
                conversations={conversations}
                curConversation={curConversation}
                onConversationChange={handleConversationChange}
                onNewConversation={handleNewConversation}
                onDeleteConversation={handleDeleteConversation}
            /> */}
            <div className={styles.chat}>
                <ChatList
                    messages={messages}
                    hotTopics={HOT_TOPICS}
                    designGuide={DESIGN_GUIDE}
                    onPromptClick={handleSubmit}
                />
                <ChatSender
                    value={inputValue}
                    onChange={setInputValue}
                    onClear={() => setMessages([])}
                    onSubmit={() => {
                        handleSubmit(inputValue);
                        setInputValue('');
                    }}
                    onCancel={() => abortController.current?.abort()}
                    onAttachmentsOpenChange={setAttachmentsOpen}
                    attachedFiles={attachedFiles}
                    onAttachedFilesChange={(files) =>
                        setAttachedFiles(files as never[])
                    }
                    senderPrompts={SENDER_PROMPTS}
                    onPromptClick={handleSubmit}
                    loading={loading}
                />
            </div>
        </div>
    );
};

export default Independent;
