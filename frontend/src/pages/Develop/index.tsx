import React, { useState } from 'react';
import { PageContainer } from '@ant-design/pro-components';
import { Button, Input, Pagination, Empty, Skeleton } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import { useIntl, useAccess } from '@umijs/max';
import { AgentTypeSelect } from './components/agent-type-select';
import { useAgentList } from '@/hooks/useAgentList';
import { AgentCard } from './components/agent-card';
import { CreateModal } from './components/create-modal';
import type { Agent } from '@/types/agent';
import { Permissions } from '@/access';

const AgentList: React.FC = () => {
  const intl = useIntl();
  const access = useAccess();
  const {
    loading,
    data,
    total,
    filter,
    search,
    filterData,
    paginate,
    refresh,
  } = useAgentList();

  const [createModalOpen, setCreateModalOpen] = useState(false);
  const [editingAgent, setEditingAgent] = useState<Agent | undefined>(undefined);
  const [modalType, setModalType] = useState<'create' | 'edit'>('create');

  const handleCreate = () => {
    setModalType('create');
    setEditingAgent(undefined);
    setCreateModalOpen(true);
  };

  const handleEdit = (agent: Agent) => {
    setModalType('edit');
    setEditingAgent(agent);
    setCreateModalOpen(true);
  };

  const handleModalClose = (visible: boolean) => {
    if (!visible) {
      setCreateModalOpen(false);
      setEditingAgent(undefined);
      setModalType('create');
    }
  };

  return (
    <PageContainer header={{ title: '' }} breadcrumb={{}}>
      {/* Search and Filter Bar */}
      <div className="flex items-center justify-between mb-4">
        <div className="flex items-center gap-2">
          <AgentTypeSelect
            placeholder={intl.formatMessage({ id: 'agent.search.typeSelectPlaceholder' })}
            style={{ width: 200 }}
            value={filter.type}
            onChange={(value) => filterData({ type: value === 'all' ? undefined : value })}
            showAllOption={true}
            disabled={loading}
          />
          <Input.Search
            placeholder={intl.formatMessage({ id: 'placeholder.search' })}
            style={{ width: 200 }}
            allowClear
            onSearch={search}
          />
        </div>
        {access.checkAccess(Permissions.Agent.Create) && (
          <Button
            icon={<PlusOutlined />}
            type="primary"
            onClick={handleCreate}
          >
            {intl.formatMessage({ id: 'actions.create' })}
          </Button>
        )}
      </div>

      {/* Content Area */}
      {loading ? (
        <Skeleton active />
      ) : data.length === 0 ? (
        <Empty description="暂无数据" />
      ) : (
        <>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 xl:grid-cols-5 gap-4">
            {data.map((item) => (
              <AgentCard
                onEdit={handleEdit}
                key={item.id}
                data={item}
                onChanged={refresh}
              />
            ))}
          </div>
          <Pagination
            current={Math.floor(filter.skipCount! / filter.maxResultCount!) + 1}
            pageSize={filter.maxResultCount}
            total={total}
            onChange={paginate}
            className="flex justify-end mt-4"
            showSizeChanger
            showQuickJumper
            showTotal={(total, range) =>
              intl.formatMessage(
                { id: 'pagination.showTotal' },
                { total, start: range[0], end: range[1] }
              )
            }
          />
        </>
      )}

      <CreateModal
        open={createModalOpen}
        type={modalType}
        values={editingAgent}
        onOpenChange={handleModalClose}
        onSuccess={refresh}
      />
    </PageContainer>
  );
};

export default AgentList;