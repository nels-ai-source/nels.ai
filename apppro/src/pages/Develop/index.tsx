import type { Agent } from '@/types/agent';
import { PlusOutlined } from '@ant-design/icons';
import { PageContainer } from '@ant-design/pro-components';
import { Button, Empty, Input, message, Pagination, Select, Skeleton } from 'antd';
import { UUID } from 'crypto';
import React, { useCallback, useEffect, useState } from 'react';
import './agent.css';
import { AgentCard } from './components/agent-card';
import { CreateModal } from './components/create-modal';

const Develop: React.FC = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [agents, setAgents] = useState<Agent[]>([]);
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const [messageApi] = message.useMessage();
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(20);

  const handlePageChange = (page: number, pageSize?: number) => {
    setCurrentPage(page);
    if (pageSize) setPageSize(pageSize);
  };

  const fetchAgents = useCallback(async () => {
    try {
      setIsLoading(true);
      //const data = await galleryAPI.listGalleries(user.id);
      // 伪造数据
      const data = Array.from({ length: 20 }, (_, index) => ({
        id: '' as UUID,
        name: `测试机器人 ${index + 1}`,
        description: `这是一个用于测试的机器人，编号 ${index + 1}，可以执行各种智能任务。`,
        icon: `default_icon${Math.floor(Math.random() * 6) + 1}.png`,
        creationTime: new Date(Date.now() - Math.random() * 30 * 24 * 60 * 60 * 1000),
        lastModificationTime: new Date(Date.now() - Math.random() * 7 * 24 * 60 * 60 * 1000),
      })) as Agent[];
      setAgents(data);
    } catch (error) {
      messageApi.error('Failed to fetch agents');
    } finally {
      setIsLoading(false);
    }
  }, [messageApi]);

  useEffect(() => {
    fetchAgents();
  }, [fetchAgents]);

  const handleCreateGallery = async (agentData: Agent) => {
    try {
      // await galleryAPI.createGallery(agentData, user.id);
      console.log(agentData);
      fetchAgents();
      setIsCreateModalOpen(false);
      messageApi.success('Agent created successfully');
    } catch (error) {
      messageApi.error('Failed to create agent');
    }
  };

  const handleDeleteGallery = async (agentId: UUID) => {
    try {
      // await galleryAPI.deleteGallery(agentId, user.id);
      console.log(agentId);
      fetchAgents();
      messageApi.success('Agent deleted successfully');
    } catch (error) {
      messageApi.error('Failed to delete agent');
    }
  };
  return (
    <PageContainer header={{ title: '' }} breadcrumb={{}} data-oid="gieahod">
      {/* Create Modal */}
      <CreateModal
        open={isCreateModalOpen}
        onCancel={() => setIsCreateModalOpen(false)}
        onCreateAgent={() => handleCreateGallery}
        data-oid="-_vpfw9"
      />

      {/* Main Content */}
      <div className={`flex-1 transition-all duration-200"}`} data-oid="su9dbuf">
        {/* Search */}
        <div
          className="flex-shrink-0 w-full h-[32px] flex items-center justify-between mb-4"
          data-oid="bppn4da"
        >
          <div className="flex items-center gap-2" data-oid="ye4eu8g">
            <Select
              placeholder="选择类型"
              style={{ width: 120 }}
              value="all"
              options={[
                { value: 'all', label: '全部' },
                { value: 'chat', label: '对话' },
                { value: 'task', label: '任务' },
              ]}
              data-oid="bunsvk0"
            />

            <Input.Search
              placeholder="搜索智能体"
              style={{ width: 200 }}
              allowClear
              data-oid="4z824yj"
            />
          </div>
          <Button
            icon={<PlusOutlined data-oid="180r16:" />}
            type="primary"
            onClick={() => setIsCreateModalOpen(true)}
            data-oid="vg26lia"
          >
            创建
          </Button>
        </div>
        {/* Content Area */}
        {isLoading ? (
          <div className="flex items-center justify-center text-secondary" data-oid=".zc82.x">
            <Skeleton active data-oid="yibhi0c" />
          </div>
        ) : !agents || agents.length === 0 ? (
          <Empty data-oid="k4ucs-d" />
        ) : (
          <>
            <div
              className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-5 gap-4"
              data-oid="_m1v7.0"
            >
              {agents.map((agent) => (
                <AgentCard
                  key={agent.id}
                  agent={agent}
                  onCreateGallery={handleCreateGallery}
                  onDeleteGallery={handleDeleteGallery}
                  data-oid="mrwc1lm"
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
              data-oid="0m.sxpl"
            />
          </>
        )}
      </div>
    </PageContainer>
  );
};

export default Develop;
