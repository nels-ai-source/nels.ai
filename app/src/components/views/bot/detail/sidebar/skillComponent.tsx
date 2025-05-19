import React from 'react';
import { Avatar, Button, List, Popover } from 'antd';
import {
    PlusOutlined,
    DeleteOutlined,
    ToolOutlined,
    ExclamationCircleOutlined,
} from '@ant-design/icons';
import { SidebarSection } from './sidebarSection';
import { Bot } from '../../../../types/bot';
import { Tool } from '../../../../types/plugin';
import { UUID } from 'crypto';

interface ToolPopoverContentProps {
    tool: Tool;
}

export const ToolPopoverContent: React.FC<ToolPopoverContentProps> = ({
    tool,
}) => {
    return (
        <div className="max-w-[300px] p-1">
            <div className="mb-2">
                <div className="font-bold mb-1">{tool.name}</div>
                <div className="text-gray-600">{tool.description}</div>
            </div>

            <div>
                {tool.inputParameters.map((parameter) => (
                    <div key={parameter.name} className="mb-1">
                        <div className="font-bold mb-0.5">{parameter.name}</div>
                        <div className="text-gray-600">
                            {parameter.description}
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export const SkillComponent: React.FC<{
    bot: Bot;
    onChange: (updates: Partial<Bot>) => void;
}> = ({ bot, onChange }) => {
    const handleDeleteTool = (toolId: UUID) => {
        if (bot?.tools) {
            const updatedTools = bot.tools.filter((tool) => tool.id !== toolId);
            onChange({ tools: updatedTools });
        }
    };
    return (
        <SidebarSection
            title="技能"
            items={[
                {
                    key: 'plugin',
                    label: '插件',
                    children:
                        bot == null ||
                        bot?.tools == null ||
                        bot?.tools?.length === 0 ? (
                            <p style={{ paddingInlineStart: 24 }}>
                                插件能够让智能体调用外部
                                API，例如搜索信息、浏览网页、生成图片等，扩展智能体的能力和使用场景。
                            </p>
                        ) : (
                            <List
                                size="small"
                                itemLayout="horizontal"
                                dataSource={bot?.tools}
                                renderItem={(item, index) => (
                                    <List.Item>
                                        <List.Item.Meta
                                            avatar={
                                                !item.pluginIcon ? (
                                                    <ToolOutlined
                                                        width={24}
                                                        height={24}
                                                        className="rounded-lg object-cover flex items-center justify-center text-2xl"
                                                    />
                                                ) : (
                                                    <img
                                                        src={`/images/bot/${item.pluginIcon}`}
                                                        className="w-6 h-6 rounded-lg object-cover flex items-center"
                                                    />
                                                )
                                            }
                                            title={
                                                <>
                                                    {item.pluginName}/
                                                    {item.name}
                                                </>
                                            }
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
                                                <Popover
                                                    placement="bottom"
                                                    content={
                                                        <ToolPopoverContent
                                                            tool={item}
                                                        />
                                                    }
                                                >
                                                    <Button
                                                        type="text"
                                                        size="small"
                                                        icon={
                                                            <ExclamationCircleOutlined />
                                                        }
                                                    ></Button>
                                                </Popover>,
                                                <Button
                                                    type="text"
                                                    size="small"
                                                    icon={<DeleteOutlined />}
                                                    onClick={() =>
                                                        handleDeleteTool(
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
                            ></Button>
                        </>
                    ),
                },
            ]}
        />
    );
};
export default SkillComponent;
