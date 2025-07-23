import { Knowledge } from '@/types/knowledge';
import { Modal } from 'antd';
import React from 'react';
import { useIntl } from '@umijs/max';
import KnowledgeManager from '../index';

interface KnowledgeSelectorProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  onSelect: (knowledge: Knowledge) => void;
  onRemove?: (knowledge: Knowledge) => void;
  title?: string;
  addedKnowledgeIds?: string[];
}

const KnowledgeSelector: React.FC<KnowledgeSelectorProps> = ({
  open,
  onOpenChange,
  onSelect,
  onRemove,
  title,
  addedKnowledgeIds = [],
}) => {
  const intl = useIntl();
  const defaultTitle = intl.formatMessage({ id: 'knowledge.selector.title' });

  const handleSelect = (knowledge: Knowledge) => {
    onSelect(knowledge);
    onOpenChange(false);
  };

  const handleRemove = (knowledge: Knowledge) => {
    onRemove?.(knowledge);
  };

  return (
    <Modal
      title={title || defaultTitle}
      open={open}
      onCancel={() => onOpenChange(false)}
      footer={null}
      width={1000}
    >
      <KnowledgeManager
        selectMode={true}
        onSelect={handleSelect}
        onRemove={handleRemove}
        hidePageContainer={true}
        addedKnowledgeIds={addedKnowledgeIds}
      />
    </Modal>
  );
};

export default KnowledgeSelector;