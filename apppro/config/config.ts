import { defineConfig } from '@umijs/max';
import { join } from 'path';
import defaultSettings from './defaultSettings';
import proxy from './proxy';
import routes from './routes';

const { REACT_APP_ENV = 'dev' } = process.env;

export default defineConfig({
  /**
   * @name Enable hash mode
   * @description Add hash suffix to build output files. Used for incremental publishing and avoiding browser cache.
   * @doc https://umijs.org/docs/api/config#hash
   */
  hash: true,

  /**
   * @name Compatibility settings
   * @description IE11 compatibility is not guaranteed, need to check all dependencies
   * @doc https://umijs.org/docs/api/config#targets
   */
  // targets: {
  //   ie: 11,
  // },

  /**
   * @name Route configuration
   * @description Only supports path, component, routes, redirect, wrappers, title configuration
   * @doc https://umijs.org/docs/guides/routes
   */
  routes,
  /**
   * @name Theme configuration
   * @description Although called theme, it's actually less variable settings
   * @doc Antd theme settings: https://ant.design/docs/react/customize-theme-cn
   * @doc Umi theme config: https://umijs.org/docs/api/config#theme
   */
  theme: {
    // Set to 'default' if you don't want configProvide dynamic theme setting
    // Only 'variable' allows dynamic primary color setting via configProvide
    'root-entry-name': 'variable',
  },
  /**
   * @name Moment.js localization configuration
   * @description Can reduce JS bundle size if internationalization is not required
   * @doc https://umijs.org/docs/api/config#ignoremomentlocale
   */
  ignoreMomentLocale: true,
  /**
   * @name Proxy configuration
   * @description Allows local server to proxy to your server for accessing server data
   * @see Note: Proxy only works in local development, not available after build
   * @doc Proxy intro: https://umijs.org/docs/guides/proxy
   * @doc Proxy config: https://umijs.org/docs/api/config#proxy
   */
  proxy: {
    '/api': {
      target: 'https://localhost:44338/',
      changeOrigin: true,
      pathRewrite: { '^/api': '/api' },
      secure: false,
    },
    '/connect': {
      target: 'https://localhost:44338/',
      changeOrigin: true,
      pathRewrite: { '^/connect': '/connect' },
      secure: false,
    },
  },

  /**
   * @name Fast refresh configuration
   * @description A good hot reload component that preserves state during updates
   */
  fastRefresh: true,
  //============== Max Plugin Configurations Below ===============
  /**
   * @name Data flow plugin
   * @doc https://umijs.org/docs/max/data-flow
   */
  model: {},
  /**
   * Global initial data flow for sharing data between plugins
   * @description Can store global data like user info or global states. Created at the very beginning of the Umi project.
   * @doc https://umijs.org/docs/max/data-flow#global-initial-state
   */
  initialState: {},
  /**
   * @name Layout plugin
   * @doc https://umijs.org/docs/max/layout-menu
   */
  title: 'Ant Design Pro',
  layout: {
    locale: true,
    ...defaultSettings,
  },
  /**
   * @name moment2dayjs plugin
   * @description Replace moment with dayjs in the project
   * @doc https://umijs.org/docs/max/moment2dayjs
   */
  moment2dayjs: {
    preset: 'antd',
    plugins: ['duration'],
  },
  /**
   * @name Internationalization plugin
   * @doc https://umijs.org/docs/max/i18n
   */
  locale: {
    // default zh-CN
    default: 'zh-CN',
    antd: true,
    // default true, when it is true, will use `navigator.language` overwrite default
    baseNavigator: true,
  },
  /**
   * @name Antd plugin
   * @description Built-in babel import plugin
   * @doc https://umijs.org/docs/max/antd#antd
   */
  antd: {},
  /**
   * @name Network request configuration
   * @description Provides unified network request and error handling based on axios and ahooks useRequest
   * @doc https://umijs.org/docs/max/request
   */
  request: {},
  /**
   * @name Access plugin
   * @description Permission plugin based on initialState, requires initialState to be enabled first
   * @doc https://umijs.org/docs/max/access
   */
  access: {},
  /**
   * @name Additional scripts in <head>
   * @description Configure additional scripts in <head>
   */
  headScripts: [
    // Fix white screen issue on first load
    { src: '/scripts/loading.js', async: true },
  ],
  //================ Pro Plugin Configuration =================
  presets: ['umi-presets-pro'],
  /**
   * @name OpenAPI plugin configuration
   * @description Generate service and mock based on OpenAPI specification, reduces boilerplate code
   * @doc https://pro.ant.design/zh-cn/docs/openapi/
   */
  openAPI: [
    {
      requestLibPath: "import { request } from '@umijs/max'",
      // Or use online version
      // schemaPath: "https://gw.alipayobjects.com/os/antfincdn/M%24jrzTTYJN/oneapi.json"
      schemaPath: join(__dirname, 'oneapi.json'),
      mock: false,
    },
    {
      requestLibPath: "import { request } from '@umijs/max'",
      schemaPath: 'https://gw.alipayobjects.com/os/antfincdn/CA1dOm%2631B/openapi.json',
      projectName: 'swagger',
    },
  ],
  mfsu: {
    strategy: 'normal',
  },
  esbuildMinifyIIFE: true,
  requestRecord: {},
});
