import {
  DownOutlined,
  FormOutlined,
  LeftOutlined,
  OneToOneOutlined,
  PartitionOutlined,
} from '@ant-design/icons';
import type { MenuProps } from 'antd';
import { Button, Dropdown, Space } from 'antd';
import React from 'react';

import { Agent } from '@/types/agent';

interface HeaderProps {
  agent: Agent | null;
  onMenuClick: MenuProps['onClick'];
  onPublish: () => void;
}

export const Header: React.FC<HeaderProps> = ({ onPublish, onMenuClick, agent }) => {
  const items: MenuProps['items'] = [
    {
      label: '单 Agent（自主规划模式）',
      key: '1',
      icon: <OneToOneOutlined data-oid=".-tn4i7" />,
    },
    {
      label: '单 Agent（对话流模式）',
      key: '2',
      icon: <PartitionOutlined data-oid="ciz1kl8" />,
    },
  ];

  const menuProps = {
    items,
    onClick: onMenuClick,
  };

  return (
    <header
      className="bg-gray-50 border-b border-gray-200 z-10 flex items-center justify-between h-14 px-2 md:h-16 md:px-4 shadow-sm"
      data-oid="hmq-_vb"
    >
      <div className="flex items-center space-x-2" data-oid="gkczvol">
        <Button type="text" icon={<LeftOutlined data-oid="dcopoo1" />} data-oid="hbvn0fb" />
        {agent?.name}
        <Button
          type="text"
          icon={<FormOutlined data-oid="e8.73sh" />}
          title={agent?.description || ''}
          onClick={() => {}}
          data-oid=":x6n.kg"
        />

        <Dropdown menu={menuProps} data-oid="86f18kv">
          <Button icon={<OneToOneOutlined data-oid="_p_ox03" />} data-oid="y3_qy43">
            <Space data-oid="w4t42ir">
              单 Agent（自主规划模式）
              <DownOutlined data-oid="471uh9j" />
            </Space>
          </Button>
        </Dropdown>
      </div>

      <div className="flex items-center space-x-3" data-oid=":i-:4y4">
        <Button type="primary" onClick={onPublish} data-oid="rccph_d">
          发布
        </Button>
      </div>
    </header>
  );
};

export default Header;
