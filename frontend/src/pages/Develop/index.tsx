import { getAgentList } from '@/services/aigc/agent';
import type { Agent } from '@/types/agent';
import { PlusOutlined } from '@ant-design/icons';
import { PageContainer } from '@ant-design/pro-components';
import { Button, Empty, Input, message, Pagination, Select, Skeleton } from 'antd';
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

  const handleGetAgents = useCallback(async () => {
    try {
      setIsLoading(true);
      const data = await getAgentList({});

      setAgents(data);
    } catch (error) {
      messageApi.error('Failed to fetch agents');
    } finally {
      setIsLoading(false);
    }
  }, [messageApi]);

  useEffect(() => {
    handleGetAgents();
  }, [handleGetAgents]);

  return (
    <PageContainer header={{ title: '' }} breadcrumb={{}}>
      {/* Create Modal */}
      <CreateModal
        open={isCreateModalOpen}
        onOpenChange={setIsCreateModalOpen}
        onChange={() => {
          handleGetAgents();
          setIsCreateModalOpen(false);
        }}
      />

      {/* Main Content */}
      <div className={`flex-1 transition-all duration-200"}`}>
        {/* Search */}
        <div
          className="flex-shrink-0 w-full h-[32px] flex items-center justify-between mb-4"

        >
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
            icon={<PlusOutlined />}
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
        ) : !agents || agents.length === 0 ? (
          <Empty />
        ) : (
          <>
            <div
              className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-5 gap-4"

            >
              {agents.map((agent) => (
                <AgentCard key={agent.id} agent={agent} onChange={handleGetAgents} />
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
    </PageContainer>
  );
};

export default Develop;
