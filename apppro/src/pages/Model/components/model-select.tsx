import { getModelList } from '@/services/aigc/model';
import { Model, ModelFilter, ModelProvider } from '@/types/model';
import { FormattedMessage } from '@umijs/max';
import { Select, Space } from 'antd';
import React, { useEffect, useState } from 'react';
import { getProviderIcon } from '../util';
interface ProviderSelectProps {
  value?: ModelProvider;
  onChange?: (value: ModelProvider) => void;
  filter?: Partial<ModelFilter>;
}

export const ModelSelect: React.FC<ProviderSelectProps> = ({ value, filter, onChange }) => {
  const [isLoading, setIsLoading] = useState(false);
  const [models, setModels] = useState<Model[]>([]);
  const [searchText, setSearchText] = useState(filter?.keyword || '');

  const handleLoadData = async () => {
    setIsLoading(true);
    try {
      const response = await getModelList(filter as ModelFilter);
      setModels(response);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    handleLoadData();
  }, [filter]);

  const filteredModels = models.filter((model) =>
    model.name.toLowerCase().includes(searchText.toLowerCase()),
  );
  return (
    <Select
      onChange={onChange}
      value={value}
      loading={isLoading}
      showSearch
      filterOption={false}
      searchValue={searchText}
      onSearch={setSearchText}
      placeholder={<FormattedMessage id="placeholder.search" />}
      allowClear
    >
      {filteredModels.map((model) => (
        <Select.Option key={model.id} value={model.id}>
          <Space className="flex items-center gap-2">
            <img
              src={getProviderIcon(model.provider as ModelProvider)}
              className="w-6 h-6 object-cover"
              alt={ModelProvider[model.provider as ModelProvider]}
            />
            {model.name}
          </Space>
        </Select.Option>
      ))}
    </Select>
  );
};

export default ModelSelect;
