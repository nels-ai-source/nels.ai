import React from 'react';
import { Button, Dropdown, Space } from 'antd';
import type { MenuProps } from 'antd';
import {
    LeftOutlined,
    FormOutlined,
    DownOutlined,
    FileTextOutlined,
    FileAddOutlined,
} from '@ant-design/icons';

import { Knowledge } from '../../../types/knowledge';

interface HeaderProps {
    data: Knowledge | null;
    onPublish: () => void;
}

export const Header: React.FC<HeaderProps> = ({ onPublish, data }) => {
    const items: MenuProps['items'] = [
        {
            label: '本地文档',
            key: '1',
            icon: <FileTextOutlined />,
        },
        {
            label: '自定义',
            key: '2',
            icon: <FileAddOutlined />,
        },
    ];

    return (
        <header className="bg-gray-50 border-b border-gray-200 z-10 flex items-center justify-between h-14 px-2 md:h-16 md:px-4 shadow-sm">
            <div className="flex items-center space-x-2">
                <Button type="text" icon={<LeftOutlined />} />
                {data?.name}
                <Button
                    type="text"
                    icon={<FormOutlined />}
                    title={data?.description || ''}
                    onClick={() => {}}
                />
            </div>

            <div className="flex items-center space-x-3">
                <Dropdown menu={{ items: items }}>
                    <Button type="primary">
                        <Space>
                            添加内容 <DownOutlined />
                        </Space>
                    </Button>
                </Dropdown>
            </div>
        </header>
    );
};

export default Header;
