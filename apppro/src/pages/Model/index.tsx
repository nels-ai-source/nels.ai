import { getModelList } from '@/services/aigc/model';
import type { Model, ModelFilter } from '@/types/model';
import { ClearOutlined, KeyOutlined, PlusOutlined } from '@ant-design/icons';
import { PageContainer } from '@ant-design/pro-components';
import { FormattedMessage } from '@umijs/max';
import { Button, Empty, Skeleton } from 'antd';
import { useEffect, useState } from 'react';
import { CreateModal } from './components/create-modal';
import { FilterPanel } from './components/filter-panel';
import { KeySettingModal } from './components/key-setting-modal';
import { ModelCard } from './components/model-card';

export default () => {
  const [isLoading, setIsLoading] = useState(false);
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const [isKeySettingModalOpen, setIsKeySettingModalOpen] = useState(false);
  const [models, setModels] = useState<Model[]>([]);

  const filter: ModelFilter = {
    keyword: null,
    type: null,
    provider: null,
    maxTokens: null,
    modelCapabilities: null,
  };
  const [searchFilters, setSearchFilters] = useState<ModelFilter>(filter);

  const handleLoadData = async () => {
    setIsLoading(true);
    try {
      const response = await getModelList(searchFilters as ModelFilter);
      setModels(response);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    handleLoadData();
  }, [searchFilters]);

  const handleFiltersUpdate = (updates: Partial<ModelFilter>) => {
    if (searchFilters) {
      setSearchFilters((prevFilters) => {
        const newFilters = { ...prevFilters, ...updates };
        return newFilters;
      });
    }
  };

  const handleClearFilters = () => {
    setSearchFilters(filter);
  };

  const handleCreateModel = async (result: boolean) => {
    if (!result) {
      return;
    }
    setIsCreateModalOpen(false);
    await handleLoadData();
  };

  return (
    <PageContainer header={{ title: '' }} breadcrumb={{}}>
      {/* Create Modal */}
      <CreateModal
        open={isCreateModalOpen}
        onCancel={() => setIsCreateModalOpen(false)}
        onCreate={handleCreateModel}
      />
      <KeySettingModal
        open={isKeySettingModalOpen}
        mode="provider"
        onCancel={() => setIsKeySettingModalOpen(false)}
        onChange={(result: boolean) => {
          if (!result) {
            return;
          }
          setIsKeySettingModalOpen(false);
          handleLoadData();
        }}
      />
      <div className="flex flex-row h-full w-full ">
        <div className="flex-1 overflow-y-auto" style={{ height: 'calc(100vh - 130px)' }}>
          <div className="flex-shrink-0 w-full h-[32px] flex items-center justify-between mb-4">
            <div className="flex items-center gap-2"></div>
            <div className="flex flex-1 justify-end items-center gap-2">
              {models.length > 0 && (
                <FormattedMessage id="model.list.result.count" values={{ count: models.length }} />
              )}
              <Button icon={<ClearOutlined />} onClick={() => handleClearFilters()}>
                <FormattedMessage id={'model.operation.clearFilter'} />
              </Button>
              <Button icon={<KeyOutlined />} onClick={() => setIsKeySettingModalOpen(true)}>
                <FormattedMessage id={'model.operation.setKey'} />
              </Button>
              <Button
                icon={<PlusOutlined />}
                type="primary"
                onClick={() => setIsCreateModalOpen(true)}
              >
                <FormattedMessage id={'model.operation.addModel'} />
              </Button>
            </div>
          </div>
          {isLoading ? (
            <Skeleton active />
          ) : models === null || models.length === 0 ? (
            <Empty />
          ) : (
            models.map((model) => (
              <ModelCard key={model.id} model={model} onChange={handleLoadData} />
            ))
          )}
        </div>
        <FilterPanel onFilterChange={handleFiltersUpdate} filter={searchFilters} />
      </div>
    </PageContainer>
  );
};
