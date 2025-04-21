/**
 * @name Umi Route Configuration
 * @description Only supports configuration of path, component, routes, redirect, wrappers, name, icon
 * @param path Supports two types of placeholder configurations:
 *            1. Dynamic parameter in the form of :id
 *            2. Wildcard * which can only appear at the end of the route string
 * @param component React component path to render after location and path match. Can be absolute or relative path.
 *                  If relative, it will start looking from src/pages
 * @param routes Configure sub-routes, typically used when adding layout components to multiple paths
 * @param redirect Configure route redirections
 * @param wrappers Configure wrapper components for route components. Wrapper components can combine more functionality
 *                 into the current route component. For example, can be used for route-level permission verification
 * @param name Configure route title. By default, reads menu.xxxx value from i18n file menu.ts
 *             Example: if name is 'login', it will read menu.login value from menu.ts as the title
 * @param icon Configure route icon, refer to https://ant.design/components/icon-cn
 *             Note: Remove style suffix and case sensitivity
 *             Example: For <StepBackwardOutlined />, use 'stepBackward' or 'StepBackward'
 *             For <UserOutlined />, use 'user' or 'User'
 * @doc https://umijs.org/docs/guides/routes
 */
export default [
  {
    path: '/user',
    layout: false,
    routes: [
      {
        name: 'login',
        path: '/user/login',
        component: './User/Login',
      },
    ],
  },
  {
    path: '/home',
    name: 'home',
    icon: 'HomeOutlined',
    component: './Home',
  },
  {
    path: '/develop',
    name: 'develop',
    icon: 'AppstoreAddOutlined',
    component: './Develop',
  },
  {
    path: '/library',
    name: 'library',
    icon: 'ProfileOutlined',
    routes: [
      {
        path: '/library',
        redirect: '/library/plugin',
      },
      {
        path: '/library/plugin',
        name: 'plugin',
        component: './Library/Plugin',
      },
      {
        path: '/library/workflow',
        name: 'workflow',
        component: './Library/Workflow',
      },
      {
        path: '/library/knowledge',
        name: 'knowledge',
        component: './Library/Knowledge',
      },
      {
        path: '/library/knowledge/detail/:id',
        hideInMenu: true,
        component: './Library/Knowledge/detail',
      },
    ],
  },
  {
    path: '/model',
    name: 'model',
    icon: 'ChromeOutlined',
    component: './Model',
  },
  {
    path: '/',
    redirect: '/home',
  },
  {
    path: '*',
    layout: false,
    component: './404',
  },
];
