import React from 'react';
import type { CollapseProps } from 'antd';
import { Collapse } from 'antd';

interface SidebarSectionProps {
  title: string;
  items: CollapseProps['items'];
  extra?: React.ReactNode;
  defaultActiveKey?: string[];
}

export const SidebarSection: React.FC<SidebarSectionProps> = ({
  title,
  items,
  extra,
  defaultActiveKey,
}) => (
  <>
    <div className="flex items-center justify-between flex-1 font-semibold leading-[20px] text-gray-500">
      <div className="sidebar-section-title flex items-center space-x-2">{title}</div>
      {extra && <div className="flex items-center space-x-3">{extra}</div>}
    </div>
    <Collapse
      ghost
      size="small"
      items={items}
      bordered={false}
      defaultActiveKey={defaultActiveKey}
      className="mb-2 [&_.ant-collapse-item]:border-b [&_.ant-collapse-item]:border-gray-200"
    />
  </>
);

export default SidebarSection;
