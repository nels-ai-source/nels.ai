import agent from './zh-CN/agent';
import commonality from './zh-CN/commonality';
import component from './zh-CN/component';
import globalHeader from './zh-CN/globalHeader';
import knowledge from './zh-CN/knowledge';
import menu from './zh-CN/menu';
import model from './zh-CN/model';
import settingDrawer from './zh-CN/settingDrawer';
import user from './zh-CN/user';

export default {
  'navBar.lang': '语言',
  'layout.user.link.help': '帮助',
  'layout.user.link.privacy': '隐私',
  'layout.user.link.terms': '条款',
  'app.preview.down.block': '下载此页面到本地项目',
  'app.welcome.link.fetch-blocks': '获取全部区块',
  'app.welcome.link.block-list': '基于 block 开发，快速构建标准页面',
  ...globalHeader,
  ...menu,
  ...settingDrawer,
  ...component,
  ...knowledge,
  ...commonality,
  ...model,
  ...agent,
  ...user
};
