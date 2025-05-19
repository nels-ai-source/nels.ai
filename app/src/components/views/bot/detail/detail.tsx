import React, { useState, useEffect } from 'react';
import { Button, message, Skeleton } from 'antd';
import type { MenuProps, CollapseProps } from 'antd';
import { useSearchParams } from 'react-router-dom';
import { PromptEditor } from './prompt';
import { Sidebar } from './sidebar/sidebar';
import { Header } from './header';
import { Bot } from '../../../types/bot';
import { v4 as uuidv4 } from 'uuid';
import { UUID } from 'crypto';
import Independent from '../../../chat/index';

export function BotDetail() {
    const [messageApi, contextHolder] = message.useMessage();
    const [bot, setBot] = useState<Bot | null>(null);
    const [loading, setLoading] = useState(true);
    const [searchParams] = useSearchParams();
    const id = searchParams.get('id');

    useEffect(() => {
        const fetchBot = async () => {
            setBot({
                name: 'AutoGen Studio',
                description: 'AutoGen Studio Bot',
                suggestedQuestions: [
                    {
                        id: uuidv4() as UUID,
                        question: '你好',
                        order: 1,
                    },
                    {
                        id: uuidv4() as UUID,
                        question: '你叫什么名字',
                        order: 2,
                    },
                ],
                tools: [
                    {
                        id: uuidv4() as UUID,
                        pluginName: 'google-search',
                        name: 'Google Search',
                        description: 'Search the web using Google',
                        inputParameters: [
                            {
                                name: 'query',
                                description: 'The search query',
                                type: 'string',
                            },
                        ],
                    },
                    {
                        id: uuidv4() as UUID,
                        pluginName: 'wolfram-alpha',
                        name: 'Wolfram Alpha',
                        description: 'Perform calculations and queries',
                        inputParameters: [
                            {
                                name: 'query',
                                description: 'The calculation or query',
                                type: 'string',
                            },
                        ],
                    },
                ],
                knowledges: [
                    {
                        id: uuidv4() as UUID,
                        name: 'Knowledge 1',
                        icon: 'dataset_text.png',
                        description: 'Knowledge 1 description',
                    },
                ],
            });
            setLoading(false);
            // try {
            //     const response = await fetch(`/api/bots/${id}`);
            //     const data = await response.json();
            //     setBot(data);
            // } catch (error) {
            //     message.error('获取Bot数据失败');
            // } finally {
            //     setLoading(false);
            // }
        };

        if (id) {
            fetchBot();
        }
    }, [id]);

    const handleBotUpdate = (updates: Partial<Bot>) => {
        if (bot) {
            setBot({ ...bot, ...updates });
        }
    };

    const handleMenuClick: MenuProps['onClick'] = (e) => {
        console.log('click', e);
    };

    return loading ? (
        <div className="flex items-center justify-center text-secondary">
            <Skeleton active />
        </div>
    ) : (
        <div className="flex flex-col h-screen ">
            {contextHolder}
            {/* Header */}
            <Header
                onMenuClick={handleMenuClick}
                bot={bot}
                onPublish={() => {
                    console.log(bot);
                }}
            />
            {/* Main Content*/}
            <main className="flex-1 overflow-auto flex flex-row">
                <aside className="flex flex-col w-[65%] bg-white border-r border-gray-200 shadow-sm">
                    <header className="border-b border-gray-200 px-2 h-12 flex items-center justify-between">
                        <div className="flex items-center space-x-2">编排</div>
                        <div className="flex items-center space-x-3">
                            <Button type="text" size="small" title="模型">
                                模型
                            </Button>
                        </div>
                    </header>

                    <main className="flex flex-row flex-1">
                        <aside className="flex flex-col w-[50%] p-2 overflow-auto whitespace-pre-wrap break-words">
                            <PromptEditor
                                bot={bot}
                                onChange={handleBotUpdate}
                            />
                        </aside>

                        {bot && (
                            <Sidebar bot={bot} onChange={handleBotUpdate} />
                        )}
                    </main>
                </aside>

                {/* Right Sidebar */}
                <aside className="w-[35%] p-4 border-l border-gray-200">
                    {/* Right Sidebar Content */}
                    <Independent></Independent>
                </aside>
            </main>
        </div>
    );
}

export default BotDetail;
