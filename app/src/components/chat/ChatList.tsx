import React from 'react';
import { Button, Space, Flex, Spin } from 'antd';
import {
    ReloadOutlined,
    CopyOutlined,
    LikeOutlined,
    DislikeOutlined,
    ShareAltOutlined,
    EllipsisOutlined,
} from '@ant-design/icons';
import { Bubble, Welcome, Prompts } from '@ant-design/x';
import { createStyles } from 'antd-style';

const useStyle = createStyles(({ token }) => ({
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
        <div className={styles.chatList}>
            {messages?.length ? (
                <Bubble.List
                    items={messages?.map((i) => ({
                        ...i.message,
                        classNames: {
                            content: i.status === 'loading' ? styles.loadingMessage : '',
                        },
                        typing: i.status === 'loading'
                            ? { step: 5, interval: 20, suffix: <>💗</> }
                            : false,
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
                <Space direction="vertical" size={16} className={styles.placeholder}>
                    <Welcome
                        variant="borderless"
                        icon="https://mdn.alipayobjects.com/huamei_iwk9zp/afts/img/A*s5sNRo5LjfQAAAAAAAAAAAAADgCCAQ/fmt.webp"
                        title="Hello, I'm Ant Design X"
                        description="Base on Ant Design, AGI product interface solution, create a better intelligent vision~"
                        extra={
                            <Space>
                                <Button icon={<ShareAltOutlined />} />
                                <Button icon={<EllipsisOutlined />} />
                            </Space>
                        }
                    />
                    <Flex gap={16}>
                        <Prompts
                            items={[hotTopics]}
                            styles={{
                                list: { height: '100%' },
                                item: {
                                    flex: 1,
                                    backgroundImage:
                                        'linear-gradient(123deg, #e5f4ff 0%, #efe7ff 100%)',
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
                        />

                        <Prompts
                            items={[designGuide]}
                            styles={{
                                item: {
                                    flex: 1,
                                    backgroundImage:
                                        'linear-gradient(123deg, #e5f4ff 0%, #efe7ff 100%)',
                                    borderRadius: 12,
                                    border: 'none',
                                },
                                subItem: { background: '#ffffffa6' },
                            }}
                            onItemClick={(info) => {
                                onPromptClick(info.data.description as string);
                            }}
                            className={styles.chatPrompt}
                        />
                    </Flex>
                </Space>
            )}
        </div>
    );
};