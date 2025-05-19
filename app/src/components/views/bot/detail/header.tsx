import React from 'react';
import { Button, Dropdown, Space } from 'antd';
import type { MenuProps } from 'antd';
import {
    LeftOutlined,
    FormOutlined,
    DownOutlined,
    OneToOneOutlined,
    PartitionOutlined,
} from '@ant-design/icons';

import { Bot } from '../../../types/bot';

interface HeaderProps {
    bot: Bot | null;
    onMenuClick: MenuProps['onClick'];
    onPublish: () => void;
}

export const Header: React.FC<HeaderProps> = ({
    onPublish,
    onMenuClick,
    bot,
}) => {
    const items: MenuProps['items'] = [
        {
            label: '单 Agent（自主规划模式）',
            key: '1',
            icon: <OneToOneOutlined />,
        },
        {
            label: '单 Agent（对话流模式）',
            key: '2',
            icon: <PartitionOutlined />,
        },
    ];

    const menuProps = {
        items,
        onClick: onMenuClick,
    };

    return (
        <header className="bg-gray-50 border-b border-gray-200 z-10 flex items-center justify-between h-14 px-2 md:h-16 md:px-4 shadow-sm">
            <div className="flex items-center space-x-2">
                <Button type="text" icon={<LeftOutlined />} />
                {bot?.name}
                <Button
                    type="text"
                    icon={<FormOutlined />}
                    title={bot?.description || ''}
                    onClick={() => {}}
                />
                <Dropdown menu={menuProps}>
                    <Button icon={<OneToOneOutlined />}>
                        <Space>
                            单 Agent（自主规划模式）
                            <DownOutlined />
                        </Space>
                    </Button>
                </Dropdown>
            </div>

            <div className="flex items-center space-x-3">
                <Button type="primary" onClick={onPublish}>
                    发布
                </Button>
            </div>
        </header>
    );
};

export default Header;
