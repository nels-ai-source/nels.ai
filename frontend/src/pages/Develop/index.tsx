import { getAgentList } from '@/services/aigc/agent';
import type { Agent, AgentFilter, AgentType } from '@/types/agent';
import { PlusOutlined } from '@ant-design/icons';
import { PageContainer } from '@ant-design/pro-components';
import { Button, Empty, Input, message, Pagination, Skeleton } from 'antd';
import React, { useCallback, useEffect, useMemo, useState } from 'react';
import './agent.css';
import { AgentCard } from './components/agent-card';
import { CreateModal } from './components/create-modal';
import { AgentTypeSelect } from './components/agent-type-select';
import { useIntl } from '@umijs/max';

const DEFAULT_PAGE_SIZE = 20;
const SEARCH_INPUT_WIDTH = 200;
const TYPE_SELECT_WIDTH = 200;

interface SearchState {
  keyword: string;
  selectedType: AgentType | 'all';
  currentPage: number;
  pageSize: number;
}

const Develop: React.FC = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [agents, setAgents] = useState<Agent[]>([]);
  const [totalCount, setTotalCount] = useState(0);
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const [searchState, setSearchState] = useState<SearchState>({
    keyword: '',
    selectedType: 'all',
    currentPage: 1,
    pageSize: DEFAULT_PAGE_SIZE,
  });

  const [messageApi] = message.useMessage();
  const intl = useIntl();

  const buildFilter = useCallback((keyword?: string, type?: AgentType | 'all'): AgentFilter => {
    const filter: AgentFilter = {};

    const searchKeyword = keyword ?? searchState.keyword;
    const searchType = type ?? searchState.selectedType;

    if (searchKeyword.trim()) {
      filter.keyword = searchKeyword.trim();
    }

    if (searchType !== 'all') {
      filter.type = searchType as AgentType;
    }

    // 添加分页参数
    filter.skipCount = (searchState.currentPage - 1) * searchState.pageSize;
    filter.maxResultCount = searchState.pageSize;

    return filter;
  }, [searchState.keyword, searchState.selectedType, searchState.currentPage, searchState.pageSize]);

  const handleGetAgents = useCallback(async (filter?: AgentFilter) => {
    try {
      setIsLoading(true);
      const data = await getAgentList(filter || {});
      setAgents(data.items);
      setTotalCount(data.totalCount);
    } catch (error) {
      messageApi.error(intl.formatMessage({ id: 'agent.search.failedToFetch' }));
    } finally {
      setIsLoading(false);
    }
  }, [messageApi, intl]);

  const handlePageChange = useCallback((page: number, pageSize?: number) => {
    const newPageSize = pageSize || searchState.pageSize;
    setSearchState(prev => ({
      ...prev,
      currentPage: page,
      pageSize: newPageSize,
    }));

    setTimeout(() => {
      const filter = buildFilter();
      filter.skipCount = (page - 1) * newPageSize;
      filter.maxResultCount = newPageSize;
      handleGetAgents(filter);
    }, 0);
  }, [buildFilter, handleGetAgents, searchState.pageSize]);

  const handleSearch = useCallback((value: string) => {
    const newKeyword = value;
    setSearchState(prev => ({ ...prev, keyword: newKeyword }));

    if (!newKeyword.trim()) {
      const filter = buildFilter(newKeyword);
      handleGetAgents(filter);
    }
  }, [buildFilter, handleGetAgents]);

  const performSearch = useCallback(() => {
    const filter = buildFilter();
    handleGetAgents(filter);
    setSearchState(prev => ({ ...prev, currentPage: 1 }));
  }, [buildFilter, handleGetAgents]);

  const handleTypeChange = useCallback((value: AgentType | 'all') => {
    setSearchState(prev => ({ ...prev, selectedType: value, currentPage: 1 }));
    const filter = buildFilter(undefined, value);
    handleGetAgents(filter);
  }, [buildFilter, handleGetAgents]);

  const refreshAgents = useCallback(() => {
    const filter = buildFilter();
    handleGetAgents(filter);
  }, [buildFilter, handleGetAgents]);

  useEffect(() => {
    const initialFilter: AgentFilter = {
      skipCount: 0,
      maxResultCount: DEFAULT_PAGE_SIZE,
    };
    handleGetAgents(initialFilter);
  }, []);

  const handleCreateModalClose = useCallback(() => {
    refreshAgents();
    setIsCreateModalOpen(false);
  }, [refreshAgents]);

  const renderSearchBar = useMemo(() => (
    <div className="flex-shrink-0 w-full h-[32px] flex items-center justify-between mb-4">
      <div className="flex items-center gap-2">
        <AgentTypeSelect
          placeholder={intl.formatMessage({ id: 'agent.search.typeSelectPlaceholder' })}
          style={{ width: TYPE_SELECT_WIDTH }}
          value={searchState.selectedType}
          onChange={handleTypeChange}
          showAllOption={true}
        />
        <Input.Search
          placeholder={intl.formatMessage({ id: 'agent.search.placeholder' })}
          style={{ width: SEARCH_INPUT_WIDTH }}
          allowClear
          value={searchState.keyword}
          onChange={(e) => handleSearch(e.target.value)}
          onSearch={performSearch}
        />
      </div>
      <Button
        icon={<PlusOutlined />}
        type="primary"
        onClick={() => setIsCreateModalOpen(true)}
      >
        {intl.formatMessage({ id: 'agent.actions.create' })}
      </Button>
    </div>
  ), [searchState.selectedType, searchState.keyword, handleTypeChange, handleSearch, performSearch, intl]);

  const renderContent = useMemo(() => {
    if (isLoading) {
      return (
        <div className="flex items-center justify-center text-secondary">
          <Skeleton active />
        </div>
      );
    }

    if (!agents || agents.length === 0) {
      return <Empty />;
    }

    return (
      <>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-5 gap-4">
          {agents.map((agent) => (
            <AgentCard key={agent.id} agent={agent} onChange={refreshAgents} />
          ))}
        </div>
        <Pagination
          current={searchState.currentPage}
          pageSize={searchState.pageSize}
          showSizeChanger={false}
          total={totalCount}
          showTotal={(total, range) =>
            intl.formatMessage(
              { id: 'pagination.showTotal' },
              { start: range[0], end: range[1], total }
            )
          }
          onChange={handlePageChange}
          className="flex justify-end mt-4"
        />
      </>
    );
  }, [isLoading, agents, searchState.currentPage, searchState.pageSize, refreshAgents, handlePageChange]);

  return (
    <PageContainer header={{ title: '' }} breadcrumb={{}}>
      {/* Create Modal */}
      <CreateModal
        open={isCreateModalOpen}
        onOpenChange={setIsCreateModalOpen}
        onChange={handleCreateModalClose}
      />

      {/* Main Content */}
      <div className="flex-1 transition-all duration-200">
        {/* Search Bar */}
        {renderSearchBar}

        {/* Content Area */}
        {renderContent}
      </div>
    </PageContainer>
  );
};

export default Develop;
