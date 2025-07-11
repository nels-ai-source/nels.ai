import { Agent, AgentPresetQuestions } from '@/types/agent';
import { HolderOutlined, PlusOutlined } from '@ant-design/icons';
import type { DragEndEvent } from '@dnd-kit/core';
import { DndContext } from '@dnd-kit/core';
import { restrictToVerticalAxis } from '@dnd-kit/modifiers';
import {
  arrayMove,
  SortableContext,
  useSortable,
  verticalListSortingStrategy,
} from '@dnd-kit/sortable';
import { Button, Input } from 'antd';
import { MdEditor } from 'md-editor-rt';
import React from 'react';
import { SidebarSection } from './sidebar-section';
import { useIntl } from '@umijs/max';
import { v4 as uuidv4 } from 'uuid';
import { UUID } from 'crypto';

const SortableItem = ({ id, question, onChange }: { id: string; question: string, onChange: (question: string) => void }) => {
  const { attributes, listeners, setNodeRef, transform } = useSortable({ id });
  const [localQuestion, setLocalQuestion] = React.useState(question);

  React.useEffect(() => {
    setLocalQuestion(question);
  }, [question]);

  return (
    <div
      ref={setNodeRef}
      className={`transform transition flex items-center mb-2`}
      style={{
        transform: transform ? `translate3d(${transform.x}px, ${transform.y}px, 0)` : undefined,
      }}
    >
      <HolderOutlined
        className="mr-2 cursor-pointer"
        {...attributes}
        {...listeners}
      />
      <Input
        value={localQuestion}
        onChange={(e) => {
          const newQuestion = e.target.value;
          setLocalQuestion(newQuestion);
          onChange(newQuestion);
        }}
      />
    </div>
  );
};

export const ConversationComponent: React.FC<{
  agent: Agent;
  onChange: (updates: Partial<Agent>) => void;
}> = ({ agent, onChange }) => {
  const intl = useIntl();
  const [dataSource, setDataSource] = React.useState<AgentPresetQuestions[]>(agent.questions || []);

  const onDragEnd = ({ active, over }: DragEndEvent) => {
    if (active.id !== over?.id) {
      setDataSource((prevState) => {
        const activeIndex = prevState.findIndex((record) => record.id === active?.id);
        const overIndex = prevState.findIndex((record) => record.id === over?.id);
        const newDataSource = arrayMove(prevState, activeIndex, overIndex);
        onChange({ questions: newDataSource } as Agent);
        return newDataSource;
      });
    }
  };

  const handleQuestionChange = (id: string, newContent: string) => {
    setDataSource((prevDataSource) => {
      const updatedDataSource = prevDataSource.map((item) =>
        item.id === id ? { ...item, content: newContent } : item
      );
      onChange({ questions: updatedDataSource } as Agent);
      return updatedDataSource;
    });
  };

  return (
    <SidebarSection
      title={intl.formatMessage({ id: 'agent.detail.conversationTitle' })}
      defaultActiveKey={['prologue', 'chat']}
      items={[
        {
          key: 'prologue',
          label: intl.formatMessage({ id: 'agent.detail.prologue' }),
          children: (
            <MdEditor
              value={agent.prologue || ''}
              onChange={(value) => {
                onChange({ prologue: value } as Agent);
              }}
              toolbars={[
                'title',
                '-',
                'bold',
                'italic',
                'strikeThrough',
                '-',
                'unorderedList',
                'orderedList',
                'quote',
                '-',
                'link',
                'image',
                'code',
                'pageFullscreen',
              ]}
              preview={false}
              footers={[]}
              style={{
                height: '120px',
              }}
            />
          ),
        },
        {
          key: 'chat',
          label: intl.formatMessage({ id: 'agent.detail.presetQuestions' }),
          children: (
            <DndContext modifiers={[restrictToVerticalAxis]} onDragEnd={onDragEnd}>
              <SortableContext items={dataSource.map((i) => i.id)} strategy={verticalListSortingStrategy}>
                {dataSource.map((item) => (
                  <SortableItem
                    key={item.id}
                    id={item.id}
                    question={item.content}
                    onChange={(question) => handleQuestionChange(item.id, question)}
                  />
                ))}
              </SortableContext>
            </DndContext>
          ),
          extra: (
            <>
              <Button
                type="text"
                size="small"
                icon={<PlusOutlined />}
                onClick={(event) => {
                  event.stopPropagation();
                  setDataSource((prevDataSource) => {
                    const newQuestion: AgentPresetQuestions = {
                      id: uuidv4() as UUID,
                      content: '',
                      index: prevDataSource.length,
                    };
                    const newDataSource = [...prevDataSource, newQuestion];
                    onChange({ questions: newDataSource } as Agent);
                    return newDataSource;
                  });
                }}
              ></Button>
            </>
          ),
        },
      ]}
    />
  );
};

export default ConversationComponent;