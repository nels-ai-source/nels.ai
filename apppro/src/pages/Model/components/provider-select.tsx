import { useIntl } from '@umijs/max';
import { Select } from 'antd';
import React from 'react';
import { ModelProvider } from '../../../types/model';
import { getEnumLabel, getProviderIcon } from '../util';
interface ProviderSelectProps {
  value?: ModelProvider;
  onChange?: (value: ModelProvider) => void;
}

export const ProviderSelect: React.FC<ProviderSelectProps> = ({ value, onChange }) => {
  const intl = useIntl();

  return (
    <Select onChange={onChange} value={value || ModelProvider.OpenAI}>
      {Object.entries(ModelProvider)
        .filter(([key]) => isNaN(Number(key)))
        .map(([, value]) => (
          <Select.Option key={value} value={value}>
            <div className="flex items-center gap-2">
              <img
                src={getProviderIcon(value as ModelProvider)}
                className="w-6 h-6 object-cover"
                alt={ModelProvider[value as ModelProvider]}
              />
              {intl.formatMessage(
                getEnumLabel(ModelProvider, 'ModelProvider', value as ModelProvider),
              )}
            </div>
          </Select.Option>
        ))}
    </Select>
  );
};

export default ProviderSelect;
