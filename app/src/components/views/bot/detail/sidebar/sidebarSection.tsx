import React from 'react';
import { Collapse, Button } from 'antd';
import type { CollapseProps } from 'antd';
import { OneToOneOutlined } from '@ant-design/icons';

interface SidebarSectionProps {
    title: string;
    items: CollapseProps['items'];
    extra?: React.ReactNode;
}

export const SidebarSection: React.FC<SidebarSectionProps> = ({
    title,
    items,
    extra,
}) => (
    <>
        <div className="flex items-center justify-between flex-1 text-[14px] font-semibold leading-[20px] text-gray-500">
            <div className="flex items-center space-x-2">{title}</div>
            {extra && (
                <div className="flex items-center space-x-3">{extra}</div>
            )}
        </div>
        <Collapse
            ghost
            size="small"
            items={items}
            bordered={false}
            defaultActiveKey={['1']}
            className="mb-2.5"
        />
    </>
);

export default SidebarSection;
