import React, { useCallback, useEffect, useState, useContext } from 'react';
import {
    message,
    Input,
    Select,
    Button,
    Space,
    Switch,
    Empty,
    Skeleton,
    Table,
    Tag,
} from 'antd';
import type { TableProps } from 'antd';

import { appContext } from '../../../hooks/provider';
import { KnowledgeCreateModal } from './create-modal';
import type { Knowledge } from '../../types/knowledge';
import { v4 as uuidv4 } from 'uuid';
import { UUID } from 'crypto';

export function KnowledgeManager() {
    const [isLoading, setIsLoading] = useState(false);
    const [knowledges, setKnowledges] = useState<Knowledge[]>([]);
    const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
    const { user } = useContext(appContext);
    const [messageApi, contextHolder] = message.useMessage();
    const [currentPage, setCurrentPage] = useState(1);
    const [pageSize, setPageSize] = useState(20);

    const handlePageChange = (page: number, pageSize?: number) => {
        setCurrentPage(page);
        if (pageSize) setPageSize(pageSize);
    };

    const fetchKnowledges = useCallback(async () => {
        if (!user?.id) return;

        try {
            setIsLoading(true);
            //const data = await knowlegeAPI.listGalleries(user.id);
            // 伪造数据
            const data = Array.from({ length: 20 }, (_, index) => ({
                id: uuidv4() as UUID,
                name: `知识库 ${index + 1}`,
                description: `这是一个用于测试的机器人，编号 ${
                    index + 1
                }，可以执行各种智能任务。`,
                type: 'text',
            })) as Knowledge[];
            setKnowledges(data);
        } catch (error) {
            messageApi.error('Failed to fetch knowledges');
        } finally {
            setIsLoading(false);
        }
    }, [messageApi]);

    useEffect(() => {
        fetchKnowledges();
    }, [fetchKnowledges]);

    const handleCreateKnowlege = async (knowledgeData: Knowledge) => {
        try {
            // await knowlegeAPI.createKnowlege(knowledgeData, user.id);
            fetchKnowledges();
            setIsCreateModalOpen(false);
            messageApi.success('Knowledge created successfully');
        } catch (error) {
            messageApi.error('Failed to create knowledge');
        }
    };

    const handleDeleteKnowlege = async (knowledgeId: UUID) => {
        try {
            // await knowlegeAPI.deleteKnowlege(knowledgeId, user.id);
            fetchKnowledges();
            messageApi.success('Knowledge deleted successfully');
        } catch (error) {
            messageApi.error('Failed to delete knowledge');
        }
    };
    const columns: TableProps<Knowledge>['columns'] = [
        {
            title: 'Name',
            dataIndex: 'name',
            render: (text: string) => <a>{text}</a>,
        },
        {
            title: 'Type',
            dataIndex: 'type',
            render: (type: string) => <Tag color="green">{type}</Tag>,
        },
        {
            title: 'IsEnable',
            dataIndex: 'isEnable',
            render: (isEnable: boolean) => (
                <Switch
                    checkedChildren="启用"
                    unCheckedChildren="禁用"
                    checked={isEnable}
                    onChange={(checked: boolean) => {
                        // handleIsEnableChange(knowledgeId, checked);
                    }}
                />
            ),
        },
        {
            title: 'LastModificationTime',
            dataIndex: 'lastModificationTime',
        },
        {
            title: 'Description',
            dataIndex: 'description',
        },
        {
            title: 'Action',
            key: 'action',
            render: (_, record) => (
                <Space size="middle">
                    <a>Delete</a>
                </Space>
            ),
        },
    ];
    return (
        <div className="relative flex h-full w-full overflow-hidden">
            {contextHolder}

            {/* Create Modal */}
            <KnowledgeCreateModal
                open={isCreateModalOpen}
                onCancel={() => setIsCreateModalOpen(false)}
                onCreateknowledge={() => handleCreateKnowlege}
            />

            {/* Main Content */}
            <div className={`flex-1 transition-all duration-200"}`}>
                <div className="flex flex-col h-full p-4 pt-2">
                    {/* Search */}
                    <div className="flex-shrink-0">
                        <div className="flex items-center justify-between mb-4">
                            <div className="flex items-center gap-2">
                                <Select
                                    placeholder="选择类型"
                                    style={{ width: 120 }}
                                    value="all"
                                    options={[
                                        { value: 'all', label: '全部' },
                                        { value: 'text', label: '文本' },
                                        { value: 'table', label: '表格' },
                                    ]}
                                />
                                <Input.Search
                                    placeholder="搜索知识库"
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
                    </div>
                    {/* Content Area */}
                    <div className="flex-1 overflow-y-auto">
                        {isLoading ? (
                            <div className="flex items-center justify-center text-secondary">
                                <Skeleton active />
                            </div>
                        ) : knowledges == null || knowledges.length === 0 ? (
                            <Empty />
                        ) : (
                            <>
                                <Table<Knowledge>
                                    columns={columns}
                                    dataSource={knowledges}
                                    bordered
                                    className="w-full h-full"
                                />
                            </>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
}

export default KnowledgeManager;
