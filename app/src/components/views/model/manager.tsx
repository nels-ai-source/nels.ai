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
// import { ModelCreateModal } from './create-modal';
import type { Model } from '../../types/model';
import { v4 as uuidv4 } from 'uuid';
import { UUID } from 'crypto';

export function ModelManager() {
    const [isLoading, setIsLoading] = useState(false);
    const [models, setModels] = useState<Model[]>([]);
    const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
    const { user } = useContext(appContext);
    const [messageApi, contextHolder] = message.useMessage();

    const fetchModels = useCallback(async () => {
        if (!user?.id) return;

        try {
            setIsLoading(true);
            //const data = await knowlegeAPI.listGalleries(user.id);
            // 伪造数据
            const data = Array.from({ length: 20 }, (_, index) => ({
                id: uuidv4() as UUID,
                provider: Math.floor(Math.random() * 3), // 随机选择 OpenAI/Azure/Anthropic
                type: Math.floor(Math.random() * 3), // 随机选择 Text/Image/Audio
                connector: Math.floor(Math.random() * 3), // 随机选择连接器
                name: `模型 ${index + 1}`,
                endpoint: `https://api.example.com/v1/model-${index + 1}`,
                accessKey: `access-key-${index + 1}`,
                secretKey: `secret-key-${index + 1}`,
                isEnabled: Math.random() > 0.3, // 70%概率启用
                deploymentName: `deployment-${index + 1}`,
                capabilities: JSON.stringify([Math.floor(Math.random() * 4)]), // 随机选择一个能力
                modelCapabilities: [Math.floor(Math.random() * 4)], // 随机选择一个模型能力
                creationTime: new Date(Date.now() - Math.random() * 30 * 24 * 60 * 60 * 1000),
                creatorId: user?.id || '',
                lastModificationTime: new Date(Date.now() - Math.random() * 7 * 24 * 60 * 60 * 1000),
                lastModifierId: user?.id || '',
                deleterId: null,
                deletionTime: null,
                isDeleted: false
            })) as Model[];
            setModels(data);
        } catch (error) {
            messageApi.error('Failed to fetch models');
        } finally {
            setIsLoading(false);
        }
    }, [messageApi]);

    useEffect(() => {
        fetchModels();
    }, [fetchModels]);

    const handleCreateModel = async (modelData: Model) => {
        try {
            // await knowlegeAPI.createKnowlege(modelData, user.id);
            fetchModels();
            setIsCreateModalOpen(false);
            messageApi.success('Model created successfully');
        } catch (error) {
            messageApi.error('Failed to create model');
        }
    };

    const handleDeleteModel = async (modelId: UUID) => {
        try {
            // await knowlegeAPI.deleteKnowlege(modelId, user.id);
            fetchModels();
            messageApi.success('Model deleted successfully');
        } catch (error) {
            messageApi.error('Failed to delete model');
        }
    };
    const columns: TableProps<Model>['columns'] = [
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
                        // handleIsEnableChange(modelId, checked);
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
            {/* <ModelCreateModal
                open={isCreateModalOpen}
                onCancel={() => setIsCreateModalOpen(false)}
                onCreatemodel={() => handleCreateKnowlege}
            /> */}

            {/* Main Content */}
            <div className={`flex-1 transition-all duration-200 "}`}>
                <div className="flex flex-col h-full p-4 pt-2">
                    {/* Search */}
                    <div className="flex-shrink-0 mb-4">
                        <div className="flex items-center justify-between">
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
                        ) : models == null || models.length === 0 ? (
                            <Empty />
                        ) : (
                            <>
                                <Table<Model>
                                    columns={columns}
                                    dataSource={models}
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

export default ModelManager;
