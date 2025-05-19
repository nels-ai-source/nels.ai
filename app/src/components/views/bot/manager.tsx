import React, { useCallback, useEffect, useState, useContext } from 'react';
import {
    message,
    Input,
    Select,
    Button,
    Pagination,
    Empty,
    Skeleton,
} from 'antd';
import { BotCard } from './bot-card';

import { appContext } from '../../../hooks/provider';
import { galleryAPI } from './api';
import { BotCreateModal } from './create-modal';
import type { Bot } from '../../types/bot';
import './bot.css';
import { v4 as uuidv4 } from 'uuid';
import { UUID } from 'crypto';

export function BotManager() {
    const [isLoading, setIsLoading] = useState(false);
    const [bots, setBots] = useState<Bot[]>([]);
    const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
    const { user } = useContext(appContext);
    const [messageApi, contextHolder] = message.useMessage();
    const [currentPage, setCurrentPage] = useState(1);
    const [pageSize, setPageSize] = useState(20);

    const handlePageChange = (page: number, pageSize?: number) => {
        setCurrentPage(page);
        if (pageSize) setPageSize(pageSize);
    };

    const fetchBots = useCallback(async () => {
        if (!user?.id) return;

        try {
            setIsLoading(true);
            //const data = await galleryAPI.listGalleries(user.id);
            // 伪造数据
            const data = Array.from({ length: 20 }, (_, index) => ({
                id: uuidv4() as UUID,
                name: `测试机器人 ${index + 1}`,
                description: `这是一个用于测试的机器人，编号 ${
                    index + 1
                }，可以执行各种智能任务。`,
                icon: `default_bot_icon${
                    Math.floor(Math.random() * 6) + 1
                }.png`,
                creationTime: new Date(
                    Date.now() - Math.random() * 30 * 24 * 60 * 60 * 1000
                ),
                lastModificationTime: new Date(
                    Date.now() - Math.random() * 7 * 24 * 60 * 60 * 1000
                ),
            })) as Bot[];
            setBots(data);
        } catch (error) {
            messageApi.error('Failed to fetch bots');
        } finally {
            setIsLoading(false);
        }
    }, [messageApi]);

    useEffect(() => {
        fetchBots();
    }, [fetchBots]);

    const handleCreateGallery = async (botData: Bot) => {
        try {
            // await galleryAPI.createGallery(botData, user.id);
            fetchBots();
            setIsCreateModalOpen(false);
            messageApi.success('Bot created successfully');
        } catch (error) {
            messageApi.error('Failed to create bot');
        }
    };

    const handleDeleteGallery = async (botId: UUID) => {
        try {
            // await galleryAPI.deleteGallery(botId, user.id);
            fetchBots();
            messageApi.success('Bot deleted successfully');
        } catch (error) {
            messageApi.error('Failed to delete bot');
        }
    };
    return (
        <div className="relative flex h-full w-full overflow-hidden">
            {contextHolder}

            {/* Create Modal */}
            <BotCreateModal
                open={isCreateModalOpen}
                onCancel={() => setIsCreateModalOpen(false)}
                onCreateBot={() => handleCreateGallery}
            />

            {/* Main Content */}
            <div className={`flex-1 transition-all duration-200"}`}>
                <div className="p-4 pt-2 overflow-y-auto  ">
                    {/* Search */}
                    <div className="flex-shrink-0 w-full h-[32px] flex items-center justify-between mb-4">
                        <div className="flex items-center gap-2">
                            <Select
                                placeholder="选择类型"
                                style={{ width: 120 }}
                                value="all"
                                options={[
                                    { value: 'all', label: '全部' },
                                    { value: 'chat', label: '对话' },
                                    { value: 'task', label: '任务' },
                                ]}
                            />
                            <Input.Search
                                placeholder="搜索智能体"
                                style={{ width: 200 }}
                                allowClear
                            />
                        </div>
                        <Button
                            type="primary"
                            onClick={() => setIsCreateModalOpen(true)}
                        >
                            创建
                        </Button>
                    </div>
                    {/* Content Area */}
                    {isLoading ? (
                        <div className="flex items-center justify-center text-secondary">
                            <Skeleton active />
                        </div>
                    ) : !bots || bots.length === 0 ? (
                        <Empty />
                    ) : (
                        <>
                            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-5 gap-4">
                                {bots.map((bot) => (
                                    <BotCard
                                        key={bot.id}
                                        bot={bot}
                                        onCreateGallery={handleCreateGallery}
                                        onDeleteGallery={handleDeleteGallery}
                                    />
                                ))}
                            </div>
                            <Pagination
                                current={currentPage}
                                pageSize={pageSize}
                                showSizeChanger={false}
                                total={1000}
                                onChange={handlePageChange}
                                className="flex justify-end mt-4"
                            />
                        </>
                    )}
                </div>
            </div>
        </div>
    );
}

export default BotManager;
