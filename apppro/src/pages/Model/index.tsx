import { modelSetting, getModelList } from '@/services/aigc/model';
import {
  ProList,
  PageContainer,
  LightFilter,
  ProFormSwitch,
  ProFormSelect,
} from '@ant-design/pro-components';
import openAIIcon from '/public/modelIcons/GPT-3.5_v2.png';
import { Button, Tag, Switch, message, Flex, Badge } from 'antd';
import { useState, useEffect } from 'react';
import SetingForm from './components/SetingForm';
import { modelIcons, modelProvider, modelCapability } from './constants';
import { useIntl } from '@umijs/max';

export default () => {
  const intl = useIntl();

  const [createModalOpen, handleModalOpen] = useState<boolean>(false);
  const [currentRow, setCurrentRow] = useState<API.ModelItem>();
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState<API.ModelItem[]>([]);

  const handleGetModelList = async (params: API.PageParams) => {
    setLoading(true);
    try {
      const { current = 1, pageSize = 10 } = params;
      const data = await getModelList({
        skipCount: (current - 1) * pageSize,
        maxResultCount: pageSize,
        sorting: 'creationTime desc',
      });
      setDataSource(data.items);
    } catch (error) {
      message.error(intl.formatMessage({ id: 'operation.get.failed' }));
    } finally {
      setLoading(false);
    }
  };
  const handleModelSetting = async (params: API.ModelSettingDto) => {
    setLoading(true);
    try {
      await modelSetting(params);
      return true;
    } catch (error) {
      message.error(intl.formatMessage({ id: 'operation.failed' }));
      return false;
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    handleGetModelList({});
  }, []);

  return (
    <PageContainer header={{ title: '' }} breadcrumb={{}}>
      <ProList<API.ModelItem>
        loading={loading}
        metas={{
          title: {
            dataIndex: 'name',
            render: (dom) => (
              <div style={{ width: '220px', overflow: 'hidden', textOverflow: 'ellipsis' }}>
                {dom}
              </div>
            ),
          },
          subTitle: {
            render: (_, record) => (
              <Flex align="center" gap="small">
                <Button
                  type="link"
                  onClick={async () => {
                    setCurrentRow(record);
                    await handleModalOpen(true);
                  }}
                >
                  设置
                </Button>
              </Flex>
            ),
          },
          avatar: {
            render: (_, record) => (
              <Badge count={record.children.length}>
                <img
                  src={modelIcons[record.provider] || openAIIcon}
                  alt="avatar"
                  style={{ width: 40, height: 40 }}
                />
              </Badge>
            ),
          },
          description: {
            render: (_, record) => (
              <div>
                <div style={{ marginBottom: 8 }}>{record.description}</div>
                {record.children && (
                  <Flex vertical gap="small">
                    {record.children.map((item) => (
                      <Flex
                        key={item.id}
                        align="center"
                        justify="space-between"
                        style={{
                          padding: '8px 12px',
                          borderRadius: 6,
                        }}
                      >
                        <Flex align="center" gap="small">
                          <img
                            src={modelIcons[item.provider] || openAIIcon}
                            alt={item.name}
                            style={{ width: 24, height: 24 }}
                          />
                          <span style={{ width: 200 }}>{item.name}</span>

                          {item.modelCapabilities?.map((capability) => (
                            <Tag key={capability} color={modelCapability[capability]?.color}>
                              {intl.formatMessage({ id: modelCapability[capability]?.labelKey })}
                            </Tag>
                          ))}
                        </Flex>
                        <Switch
                          checkedChildren={intl.formatMessage({ id: 'model.list.running' })}
                          unCheckedChildren={intl.formatMessage({ id: 'model.list.stopped' })}
                          checked={item.isEnabled}
                        />
                      </Flex>
                    ))}
                  </Flex>
                )}
              </div>
            ),
          },
        }}
        toolbar={{
          search: {
            onSearch: (value: string) => {
              console.log(value);
              handleGetModelList({});
            },
          },
          filter: (
            <LightFilter>
              <ProFormSelect
                name="provider"
                label={intl.formatMessage({ id: 'model.list.provider' })}
                showSearch
                valueEnum={modelProvider}
                placeholder={intl.formatMessage({ id: 'model.list.provider' })}
              />
              <ProFormSwitch
                name="open"
                label={intl.formatMessage({ id: 'model.list.switch' })}
                checkedChildren={intl.formatMessage({ id: 'model.list.running' })}
                unCheckedChildren={intl.formatMessage({ id: 'model.list.stopped' })}
              />
            </LightFilter>
          ),
        }}
        rowKey="id"
        headerTitle=""
        dataSource={dataSource}
      />
      <SetingForm
        open={createModalOpen}
        onOpenChange={handleModalOpen}
        onFinish={async (value) => {
          const success = await handleModelSetting(value as API.ModelSettingDto);
          if (success) {
            handleModalOpen(false);
            await handleGetModelList({});
          }
          return success;
        }}
        provider={currentRow?.provider}
      />
    </PageContainer>
  );
};
