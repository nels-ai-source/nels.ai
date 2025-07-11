import { PageContainer } from '@ant-design/pro-components';
import App from 'antd/es/app/App';

const plugin: React.FC = () => {
  return (
    <PageContainer header={{ title: '' }} breadcrumb={{}}>
      <App> Coming Soon</App>
    </PageContainer>
  );
};

export default plugin;
