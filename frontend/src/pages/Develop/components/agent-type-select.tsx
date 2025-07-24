import React from 'react';
import { Select } from 'antd';
import { useIntl } from '@umijs/max';
import { AgentType } from '@/types/agent';
import {
    OneToOneOutlined,
    PartitionOutlined,
    ClusterOutlined,
    HolderOutlined
} from '@ant-design/icons';

export interface AgentTypeItem {
    label: string;
    key: AgentType;
    icon: React.ReactNode;
    intlId: string;
}

export const getAgentTypeItems = (intl: any): AgentTypeItem[] => [
    {
        label: intl.formatMessage({ id: 'agent.types.chatCompletion' }),
        key: AgentType.chatCompletion,
        icon: <OneToOneOutlined className="w-6 h-6 object-cover" />,
        intlId: 'agent.types.chatCompletion',
    },
    {
        label: intl.formatMessage({ id: 'agent.types.workflow' }),
        key: AgentType.workflow,
        icon: <PartitionOutlined className="w-6 h-6 object-cover" />,
        intlId: 'agent.types.workflow',
    },
    {
        label: intl.formatMessage({ id: 'agent.types.multiAgent' }),
        key: AgentType.multiAgent,
        icon: <ClusterOutlined className="w-6 h-6 object-cover" />,
        intlId: 'agent.types.multiAgent',
    }
];

export const getAgentTypeSelectOptions = (intl: any) => {
    const items = getAgentTypeItems(intl);
    return [
        {
            value: 'all',
            label: intl.formatMessage({ id: 'agent.search.allTypes' }),
            icon: <HolderOutlined className="w-6 h-6 object-cover" />
        },
        ...items.map(item => ({
            value: item.key,
            label: item.label,
            icon: item.icon
        }))
    ];
};

interface AgentTypeSelectProps {
    value?: AgentType | 'all';
    onChange?: (value: AgentType | 'all') => void;
    placeholder?: string;
    style?: React.CSSProperties;
    showAllOption?: boolean;
    className?: string;
    disabled: boolean;
}

export const AgentTypeSelect: React.FC<AgentTypeSelectProps> = ({
    value,
    onChange,
    placeholder,
    style,
    showAllOption = false,
    className,
    disabled
}) => {
    const intl = useIntl();

    const options = showAllOption
        ? getAgentTypeSelectOptions(intl)
        : getAgentTypeItems(intl).map(item => ({
            value: item.key,
            label: item.label,
            icon: item.icon
        }));

    return (
        <Select
            value={value}
            onChange={onChange}
            placeholder={placeholder}
            style={style}
            className={className}
            disabled={disabled}
        >
            {options.map((option) => (
                <Select.Option key={option.value} value={option.value}>
                    <div className="flex items-center">
                        {!showAllOption && option.icon}
                        {option.label}
                    </div>
                </Select.Option>
            ))}
        </Select>
    );
};

export default AgentTypeSelect;