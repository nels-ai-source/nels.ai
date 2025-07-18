import { ProLayoutProps } from '@ant-design/pro-components';

/**
 * @name Default Layout Settings
 */
const Settings: ProLayoutProps & {
  pwa?: boolean;
  logo?: string;
} = {
  navTheme: 'light',
  // Daybreak Blue
  colorPrimary: '#1890ff',
  layout: 'mix',
  contentWidth: 'Fluid',
  fixedHeader: false,
  fixSiderbar: true,
  colorWeak: false,
  title: 'Nels.AI',
  pwa: true,
  logo: '/nels_logo2.svg',
  iconfontUrl: '',
  token: {
    bgLayout:'#fff',
    // See TypeScript declaration and documentation for demo
    // Modify styles through token: https://procomponents.ant.design/components/layout#%E9%80%9A%E8%BF%87-token-%E4%BF%AE%E6%94%B9%E6%A0%B7%E5%BC%8F
  },
};

export default Settings;