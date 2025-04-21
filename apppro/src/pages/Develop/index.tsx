import { getAgentList } from '@/services/aigc/agent';
import { Card, List, Typography, message } from 'antd';
import { useState, useEffect } from 'react';
import { PageContainer, ProList } from '@ant-design/pro-components';
import { useIntl } from '@umijs/max';

const { Paragraph } = Typography;

const Develop: React.FC = () => {
  const intl = useIntl();
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState<API.Agent[]>([]);

  const handleGetList = async (params: API.PageParams) => {
    setLoading(true);
    try {
      const { current = 1, pageSize = 10 } = params;
      const data = await getAgentList({
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

  useEffect(() => {
    handleGetList({});
  }, []);

  return (
    <PageContainer header={{ title: '' }} breadcrumb={{}}>
      <ProList<API.Agent>
        rowKey="id"
        loading={loading}
        dataSource={dataSource}
        renderItem={(item) => (
          <List.Item>
            <Card hoverable>
              <Card.Meta
                title={<a>{item.name}</a>}
                description={
                  <Paragraph
                    ellipsis={{
                      rows: 2,
                    }}
                  >
                    {item.description}
                  </Paragraph>
                }
              />
            </Card>
          </List.Item>
        )}
      ></ProList>
    </PageContainer>
  );
};

export default Develop;
