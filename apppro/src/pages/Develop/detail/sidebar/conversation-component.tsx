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

const SortableItem = ({ id, question }: { id: string; question: string }) => {
  const { attributes, listeners, setNodeRef } = useSortable({ id });

  return (
    <div
      ref={setNodeRef}
      className={`transform transition flex items-center mb-2`}
      {...attributes}
      {...listeners}
      data-oid="jko4d6a"
    >
      <HolderOutlined className="mr-2" data-oid="y9wzftg" />
      <Input value={question} data-oid="oc0joti" />
    </div>
  );
};

export const ConversationComponent: React.FC<{
  agent: Agent;
  onChange: (updates: Partial<Agent>) => void;
}> = ({ agent, onChange }) => {
  const [dataSource, setDataSource] = React.useState<AgentPresetQuestions[]>(
    agent.questions || [{}],
  );
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
  return (
    <SidebarSection
      title="对话体验"
      defaultActiveKey={['prologue', 'chat']}
      items={[
        {
          key: 'prologue',
          label: '开场白文案',
          children: (
            /*  https://imzbf.github.io/md-editor-rt/en-US/ */
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
              data-oid="u1:7j7g"
            />
          ),
        },
        {
          key: 'chat',
          label: '预设问题',
          children: (
            <DndContext
              modifiers={[restrictToVerticalAxis]}
              onDragEnd={onDragEnd}
              data-oid="b.eg7ij"
            >
              <SortableContext
                items={dataSource.map((i) => i.id)}
                strategy={verticalListSortingStrategy}
                data-oid="j50mnhk"
              >
                {dataSource.map((item) => (
                  <SortableItem
                    key={item.id}
                    id={item.id}
                    question={item.content}
                    data-oid="25:8n-v"
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
                icon={<PlusOutlined data-oid="pzglvjz" />}
                onClick={(event) => {
                  event.stopPropagation();
                }}
                data-oid="31767wu"
              ></Button>
            </>
          ),
        },
      ]}
      data-oid="lzgy71h"
    />
  );
};
export default ConversationComponent;
