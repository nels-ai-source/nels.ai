import { ModelCapability, ModelFilter, ModelProvider, ModelType } from '@/types/model';
import { FormattedMessage, useIntl } from '@umijs/max';
import type { SliderSingleProps } from 'antd';
import { Checkbox, Collapse, Input, Radio, Slider } from 'antd';
import React from 'react';

interface FilterPanelProps {
  filter: ModelFilter;
  onFilterChange: (updates: Partial<ModelFilter>) => void;
}

export const FilterPanel: React.FC<FilterPanelProps> = ({ filter, onFilterChange }) => {
  const intl = useIntl();
  const style: React.CSSProperties = {
    display: 'flex',
    flexDirection: 'column',
    gap: 8,
  };

  const marks: SliderSingleProps['marks'] = {
    0: '0',
    128000: '128k',
    256000: '256k',
    512000: 'max',
  };

  const generateEnumOptions = (enumObj: any, enumName: string) => {
    return Object.entries(enumObj)
      .filter(([key]) => isNaN(Number(key)))
      .map(([key]) => ({
        label: intl.formatMessage({ id: `${enumName}.${key}` }),
        value: enumObj[key],
      }));
  };
  return (
    <div className="w-200px p-2 pt-0" data-oid="xo0mr8x">
      <Input.Search
        placeholder={intl.formatMessage({ id: 'model.search' })}
        style={{ width: 200 }}
        allowClear
        onChange={(e) => {
          onFilterChange({ keyword: e.target.value });
        }}
        value={filter.keyword || ''}
        className="mb-4"
        data-oid="c0uz7c2"
      />

      <Collapse
        defaultActiveKey={['type', 'provider', 'maxTokens', 'modelCapabilities']}
        ghost
        items={[
          {
            key: 'type',
            label: <FormattedMessage id="model.type" data-oid="e6ht9:y" />,
            children: (
              <Radio.Group
                style={style}
                onChange={(e) => {
                  onFilterChange({ type: e.target.value });
                }}
                value={filter.type || undefined}
                data-oid="b9wnroz"
              >
                {generateEnumOptions(ModelType, 'ModelType').map((option) => (
                  <Radio key={option.value} value={option.value} data-oid="2s.37y6">
                    {option.label}
                  </Radio>
                ))}
              </Radio.Group>
            ),
          },
          {
            key: 'provider',
            label: <FormattedMessage id="model.provider" data-oid="buyh_66" />,
            children: (
              <Radio.Group
                style={style}
                onChange={(e) => {
                  onFilterChange({ provider: e.target.value });
                }}
                value={filter.provider || undefined}
                data-oid="y3rnfcn"
              >
                {generateEnumOptions(ModelProvider, 'ModelProvider').map((option) => (
                  <Radio key={option.value} value={option.value} data-oid="bzu5fow">
                    {option.label}
                  </Radio>
                ))}
              </Radio.Group>
            ),
          },
          {
            key: 'maxTokens',
            label: <FormattedMessage id="model.maxTokens" data-oid="tnmkaoj" />,
            children: (
              <Slider
                marks={marks}
                min={0}
                max={512000}
                step={16000}
                onChange={(value) => {
                  onFilterChange({ maxTokens: value });
                }}
                value={filter.maxTokens || 128000}
                data-oid="ihlc4uv"
              />
            ),
          },
          {
            key: 'modelCapabilities',
            label: <FormattedMessage id="model.modelCapabilities" data-oid="oav-ths" />,
            children: (
              <Checkbox.Group
                style={style}
                onChange={(values) => {
                  onFilterChange({ modelCapabilities: values });
                }}
                value={filter.modelCapabilities || []}
                options={generateEnumOptions(ModelCapability, 'ModelCapability').map((option) => ({
                  label: option.label,
                  value: option.value,
                }))}
                data-oid="lgtett:"
              />
            ),
          },
        ]}
        data-oid="wod.2jo"
      />
    </div>
  );
};
