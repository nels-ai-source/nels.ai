import React, { useState, useEffect } from 'react';
import { Button, List, Popover, message, Tooltip } from 'antd';
import { UUID } from 'crypto';
import { useIntl } from 'umi';
import { DeleteOutlined, PlusOutlined, SettingOutlined, CopyOutlined } from '@ant-design/icons';

import { Agent, AgentKnowledge, KnowledgeOption } from '@/types/agent';
import { Knowledge } from '@/types/knowledge';
import { SidebarSection } from './sidebar-section';
import { KnowledgeSettings } from '@/pages/Library/Knowledge/components/knowledge-settings';
import KnowledgeSelector from '@/pages/Library/Knowledge/components/knowledge-selector';

interface KnowledgeComponentProps {
  agent: Agent;
  onChange: (updates: Partial<Agent>) => void;
}


export const KnowledgeComponent: React.FC<KnowledgeComponentProps> = ({ agent, onChange }) => {
  const intl = useIntl();
  const [settingsVisible, setSettingsVisible] = useState(false);
  const [selectorVisible, setSelectorVisible] = useState(false);


  const [knowledgeOption, setKnowledgeOption] = useState({});


  useEffect(() => {
    if (agent.knowledgeOption) {
      setKnowledgeOption(agent.knowledgeOption);
    }
  }, [agent.knowledgeOption]);

  const handleSettingsChange = (option: KnowledgeOption) => {
    setKnowledgeOption(option);
    onChange({ knowledgeOption: option });
  };

  const handleSettingsVisibleChange = (visible: boolean) => {
    setSettingsVisible(visible);
  };
  const handleDeleteKnowledge = (knowledgeId: UUID) => {
    if (agent?.knowledges) {
      const updatedKnowledges = agent.knowledges.filter(
        (knowledge) => knowledge.id !== knowledgeId,
      );
      onChange({ knowledges: updatedKnowledges });
    }
  };

  const handleAddKnowledge = (event: React.MouseEvent) => {
    event.stopPropagation();
    setSelectorVisible(true);
  };

  const handleSelectKnowledge = (knowledge: Knowledge) => {
    const existingKnowledges = agent?.knowledges || [];
    const isAlreadyAdded = existingKnowledges.some(k => k.knowledgeId === knowledge.id);

    if (isAlreadyAdded) {
      message.warning(intl.formatMessage({ id: 'agent.detail.knowledgeComponent.alreadyAdded' }));
      return;
    }

    const agentKnowledge: AgentKnowledge = {
      id: crypto.randomUUID(),
      icon: knowledge.icon,
      name: knowledge.name,
      description: knowledge.description,
      agentId: agent.id,
      knowledgeId: knowledge.id,
    };

    const updatedKnowledges = [...existingKnowledges, agentKnowledge];
    onChange({ knowledges: updatedKnowledges });
    message.success(`${intl.formatMessage({ id: 'agent.detail.knowledgeComponent.addSuccess' })}: ${knowledge.name}`);
  };

  const handleRemoveKnowledge = (knowledge: Knowledge) => {
    const existingKnowledges = agent?.knowledges || [];
    const updatedKnowledges = existingKnowledges.filter(k => k.knowledgeId !== knowledge.id);
    onChange({ knowledges: updatedKnowledges });
    message.success(`${intl.formatMessage({ id: 'agent.detail.knowledgeComponent.removeSuccess' })}: ${knowledge.name}`);
  };

  const handleCopyKnowledge = (knowledge: AgentKnowledge) => {
    navigator.clipboard.writeText(knowledge.name).then(() => {
      message.success(intl.formatMessage({ id: 'agent.detail.knowledgeComponent.copySuccess' }));
    }).catch(() => {
      message.error(intl.formatMessage({ id: 'agent.detail.knowledgeComponent.copyFailed' }));
    });
  };

  const renderEmptyState = () => (
    <p className="pl-6 text-gray-500">
      {intl.formatMessage({ id: 'agent.detail.knowledgeComponent.emptyText' })}
    </p>
  );

  const renderKnowledgeList = () => (
    <List
      size="small"
      itemLayout="horizontal"
      dataSource={agent?.knowledges}
      renderItem={(item) => (
        <List.Item>
          <List.Item.Meta
            avatar={
              !item.icon ? (
                <img
                  src="/images/knowledge/dataset_text.png"
                  className="w-8 h-8 rounded-lg object-cover"
                  alt={intl.formatMessage({ id: 'agent.detail.knowledgeComponent.knowledgeAlt' })}
                />
              ) : (
                <img
                  src={`/images/plugin/${item.icon}`}
                  className="w-8 h-8 rounded-lg object-cover"
                  alt={item.name}
                />
              )
            }
            title={
              <div className="font-medium text-gray-900 text-sm">
                {item.name}
              </div>
            }
            description={
              <div className="text-xs text-gray-500 mt-1 line-clamp-2">
                {item.description || intl.formatMessage({ id: 'agent.detail.knowledgeComponent.noDescription' })}
              </div>
            }
          />

          <div className="flex items-center space-x-1">
            <Tooltip title={intl.formatMessage({ id: 'agent.detail.knowledgeComponent.copyTooltip' })}>
              <Button
                type="text"
                size="small"
                icon={<CopyOutlined />}
                onClick={() => handleCopyKnowledge(item)}
                className="text-gray-400 hover:text-blue-500"
              />
            </Tooltip>
            <Tooltip title={intl.formatMessage({ id: 'agent.detail.knowledgeComponent.removeTooltip' })}>
              <Button
                type="text"
                size="small"
                icon={<DeleteOutlined />}
                onClick={() => handleDeleteKnowledge(item.id!)}
                className="text-gray-400 hover:text-red-500"
              />
            </Tooltip>
          </div>
        </List.Item>
      )}
    />
  );

  const hasKnowledge = agent?.knowledges && agent.knowledges.length > 0;

  return (
    <>
      <SidebarSection
        title={intl.formatMessage({ id: 'agent.detail.sidebar.knowledge' })}
        defaultActiveKey={['konwsledge']}
        items={[
          {
            key: 'konwsledge',
            label: intl.formatMessage({ id: 'agent.detail.sidebar.text' }),
            children: hasKnowledge ? renderKnowledgeList() : renderEmptyState(),
            extra: (
              <Button
                type="text"
                size="small"
                icon={<PlusOutlined />}
                onClick={handleAddKnowledge}
              />
            ),
          },
        ]}
        extra={
          <Popover
            content={
              <KnowledgeSettings
                option={knowledgeOption as KnowledgeOption}
                onSettingsChange={handleSettingsChange}
              />
            }
            title={null}
            trigger="click"
            open={settingsVisible}
            onOpenChange={handleSettingsVisibleChange}
            placement="bottomRight"
            arrow={false}
          >
            <Button type="text" size="small" icon={<SettingOutlined />}>
              {intl.formatMessage({ id: 'agent.detail.sidebar.autoCall' })}
            </Button>
          </Popover>
        }
      />

      <KnowledgeSelector
        open={selectorVisible}
        onOpenChange={setSelectorVisible}
        onSelect={handleSelectKnowledge}
        onRemove={handleRemoveKnowledge}
        title={intl.formatMessage({ id: 'agent.detail.knowledgeComponent.selectTitle' })}
        addedKnowledgeIds={agent?.knowledges?.map(k => k.knowledgeId!) || []}
      />
    </>
  );
};

export default KnowledgeComponent;