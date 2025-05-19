import React from 'react';
import { Button, List } from 'antd';
import {
    ToolOutlined,
    DeleteOutlined,
    PlusOutlined,
    SettingOutlined,
} from '@ant-design/icons';
import { SidebarSection } from './sidebarSection';
import { Bot } from '../../../../types/bot';
import { UUID } from 'crypto';
export const KnowledgeComponent: React.FC<{
    bot: Bot;
    onChange: (updates: Partial<Bot>) => void;
}> = ({ bot, onChange }) => {
    const handleDeleteKnowledge = (knowledgeId: UUID) => {
        if (bot?.knowledges) {
            const updatedKnowledges = bot.knowledges.filter(
                (knowledge) => knowledge.id !== knowledgeId
            );
            onChange({ knowledges: updatedKnowledges });
        }
    };
    return (
        <SidebarSection
            title="知识"
            items={[
                {
                    key: 'konwsledge',
                    label: '文本',
                    children:
                        bot == null ||
                        bot?.knowledges == null ||
                        bot?.knowledges?.length === 0 ? (
                            <p style={{ paddingInlineStart: 24 }}>
                                将文档、URL、三方数据源上传为文本知识库后，用户发送消息时，智能体能够引用文本知识中的内容回答用户问题。
                            </p>
                        ) : (
                            <List
                                size="small"
                                itemLayout="horizontal"
                                dataSource={bot?.knowledges}
                                renderItem={(item, index) => (
                                    <List.Item>
                                        <List.Item.Meta
                                            avatar={
                                                !item.icon ? (
                                                    <ToolOutlined
                                                        width={24}
                                                        height={24}
                                                        className="rounded-lg object-cover flex items-center justify-center text-2xl"
                                                    />
                                                ) : (
                                                    <img
                                                        src={`/images/plugin/${item.icon}`}
                                                        className="w-6 h-6 rounded-lg object-cover flex items-center"
                                                    />
                                                )
                                            }
                                            title={<>{item.name}</>}
                                            description={
                                                <div
                                                    style={{
                                                        whiteSpace: 'nowrap',
                                                        overflow: 'hidden',
                                                        textOverflow:
                                                            'ellipsis',
                                                    }}
                                                >
                                                    {item.description}
                                                </div>
                                            }
                                        />
                                        <List.Item
                                            actions={[
                                                <Button
                                                    type="text"
                                                    size="small"
                                                    icon={<DeleteOutlined />}
                                                    onClick={() =>
                                                        handleDeleteKnowledge(
                                                            item.id!
                                                        )
                                                    }
                                                />,
                                            ]}
                                        ></List.Item>
                                    </List.Item>
                                )}
                            />
                        ),
                    extra: (
                        <>
                            <Button
                                type="text"
                                size="small"
                                icon={<PlusOutlined />}
                                onClick={(event) => {
                                    event.stopPropagation();
                                }}
                            />
                        </>
                    ),
                },
            ]}
            extra={
                <Button type="text" size="small" icon={<SettingOutlined />}>
                    自动调用
                </Button>
            }
        />
    );
};
export default KnowledgeComponent;
