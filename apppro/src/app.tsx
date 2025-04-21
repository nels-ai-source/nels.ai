import { AvatarDropdown, AvatarName, Footer, Question, SelectLang } from '@/components';
import { getConfiguration } from '@/services/aigc/api';
import { LinkOutlined } from '@ant-design/icons';
import type { Settings as LayoutSettings } from '@ant-design/pro-components';
import { ProBreadcrumb, SettingDrawer } from '@ant-design/pro-components';
import type { RequestConfig, RunTimeLayoutConfig } from '@umijs/max';
import { history, Link, useIntl } from '@umijs/max';
import { App as AntApp } from 'antd';
import defaultSettings from '../config/defaultSettings';
import { errorConfig } from './requestErrorConfig';

const isDev = process.env.NODE_ENV === 'development';
const loginPath = '/user/login';

/**
 * @see  https://umijs.org/zh-CN/plugins/plugin-initial-state
 * */
export async function getInitialState(): Promise<{
  settings?: Partial<LayoutSettings>;
  currentUser?: API.CurrentUser;
  loading?: boolean;
  fetchUserInfo?: () => Promise<API.CurrentUser | undefined>;
}> {
  const fetchUserInfo = async () => {
    try {
      const msg = await getConfiguration({
        skipErrorHandler: true,
      });
      msg.userInfo.permissions = msg.permissions;
      return msg.userInfo;
    } catch (error) {
      history.push(loginPath);
    }
    return undefined;
  };
  const { location } = history;
  if (location.pathname !== loginPath) {
    const currentUser = await fetchUserInfo();
    console.log('currentUser', currentUser);
    return {
      fetchUserInfo,
      currentUser,
      settings: defaultSettings as Partial<LayoutSettings>,
    };
  }
  return {
    fetchUserInfo,
    settings: defaultSettings as Partial<LayoutSettings>,
  };
}

export const layout: RunTimeLayoutConfig = ({ initialState, setInitialState }) => {
  const intl = useIntl();
  const home = intl.formatMessage({ id: 'menu.home' });
  return {
    actionsRender: () => [<Question key="doc" />, <SelectLang key="SelectLang" />],
    avatarProps: {
      src: initialState?.currentUser?.avatar,
      title: <AvatarName />,
      render: (_, avatarChildren) => {
        return <AvatarDropdown>{avatarChildren}</AvatarDropdown>;
      },
    },
    headerContentRender: () => <ProBreadcrumb style={{ paddingInline: '80px' }} />,
    breadcrumbRender: (routers = []) => {
      if (routers.length === 0) {
        return [];
      }
      const firstRoute = routers[0];
      if (firstRoute.title === home) {
        return [];
      }

      return [
        {
          path: '/',
          title: home,
        },
        ...routers,
      ];
    },
    waterMarkProps: {
      content: initialState?.currentUser?.name,
    },
    footerRender: () => <Footer />,
    onPageChange: () => {
      const { location } = history;
      const token = localStorage.getItem('access_token');

      if (!initialState?.currentUser || (!token && location.pathname !== loginPath)) {
        history.push(loginPath);
      }
    },
    bgLayoutImgList: [
      {
        src: 'https://mdn.alipayobjects.com/yuyan_qk0oxh/afts/img/D2LWSqNny4sAAAAAAAAAAAAAFl94AQBr',
        left: 85,
        bottom: 100,
        height: '303px',
      },
      {
        src: 'https://mdn.alipayobjects.com/yuyan_qk0oxh/afts/img/C2TWRpJpiC0AAAAAAAAAAAAAFl94AQBr',
        bottom: -68,
        right: -45,
        height: '303px',
      },
      {
        src: 'https://mdn.alipayobjects.com/yuyan_qk0oxh/afts/img/F6vSTbj8KpYAAAAAAAAAAAAAFl94AQBr',
        bottom: 0,
        left: 0,
        width: '331px',
      },
    ],
    links: isDev
      ? [
          <Link key="openapi" to="/umi/plugin/openapi" target="_blank">
            <LinkOutlined />
            <span>OpenAPI 文档</span>
          </Link>,
        ]
      : [],
    menuHeaderRender: undefined,

    childrenRender: (children) => {
      return (
        <AntApp>
          <div
            style={{
              minHeight: 'calc(100vh - 160px)',
            }}
          >
            {children}
            {isDev && (
              <SettingDrawer
                disableUrlParams
                enableDarkTheme
                settings={initialState?.settings}
                onSettingChange={(settings) => {
                  setInitialState((preInitialState) => ({
                    ...preInitialState,
                    settings,
                  }));
                }}
              />
            )}
          </div>
        </AntApp>
      );
    },
    ...initialState?.settings,
  };
};

const authHeaderInterceptor = (url: string, options: RequestConfig) => {
  const token = localStorage.getItem('access_token');
  const authHeader = { Authorization: 'Bearer ' + token };
  return {
    url: `${url}`,
    options: { ...options, interceptors: true, headers: { ...options.headers, ...authHeader } },
  };
};
/**
 * @en Request configuration with error handling capabilities
 * @en Based on axios and ahooks' useRequest, providing a unified network request and error handling solution
 * @zh 请求配置，可以配置错误处理
 * @zh 它基于 axios 和 ahooks 的 useRequest 提供了一套统一的网络请求和错误处理方案
 * @doc https://umijs.org/docs/max/request#配置
 */
export const request = {
  ...errorConfig,
  requestInterceptors: [authHeaderInterceptor],
};
