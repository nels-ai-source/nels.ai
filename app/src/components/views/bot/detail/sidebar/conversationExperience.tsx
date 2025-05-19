import React from 'react';
import { Input } from 'antd';
import { HolderOutlined } from '@ant-design/icons';
import { Bot, BotSuggestedQuestion } from '../../../../types/bot';
import { MdEditor } from 'md-editor-rt';
import { SidebarSection } from './sidebarSection';
import type { DragEndEvent } from '@dnd-kit/core';
import { DndContext } from '@dnd-kit/core';
import { restrictToVerticalAxis } from '@dnd-kit/modifiers';
import {
    arrayMove,
    SortableContext,
    useSortable,
    verticalListSortingStrategy,
} from '@dnd-kit/sortable';

const SortableItem = ({ id, question }: { id: string; question: string }) => {
    const { attributes, listeners, setNodeRef, transform, transition } =
        useSortable({ id });

    return (
        <div
            ref={setNodeRef}
            className={`transform transition flex items-center mb-2`}
            {...attributes}
            {...listeners}
        >
            <HolderOutlined className="mr-2" />
            <Input value={question} />
        </div>
    );
};

export const ConversationExperience: React.FC<{
    bot: Bot;
    onChange: (updates: Partial<Bot>) => void;
}> = ({ bot, onChange }) => {
    const [dataSource, setDataSource] = React.useState<BotSuggestedQuestion[]>(
        bot.suggestedQuestions
    );
    const onDragEnd = ({ active, over }: DragEndEvent) => {
        if (active.id !== over?.id) {
            setDataSource((prevState) => {
                const activeIndex = prevState.findIndex(
                    (record) => record.id === active?.id
                );
                const overIndex = prevState.findIndex(
                    (record) => record.id === over?.id
                );
                const newDataSource = arrayMove(
                    prevState,
                    activeIndex,
                    overIndex
                );
                onChange({ suggestedQuestions: newDataSource } as Bot);
                return newDataSource;
            });
        }
    };
    return (
        <SidebarSection
            title="对话体验"
            items={[
                {
                    key: 'prologue',
                    label: '开场白文案',
                    children: (
                        /*  https://imzbf.github.io/md-editor-rt/en-US/ */
                        <MdEditor
                            value={bot.prologue || ''}
                            onChange={(value) => {
                                onChange({ prologue: value } as Bot);
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
                    label: '预设问题',
                    children: (
                        <DndContext
                            modifiers={[restrictToVerticalAxis]}
                            onDragEnd={onDragEnd}
                        >
                            <SortableContext
                                items={dataSource.map((i) => i.id)}
                                strategy={verticalListSortingStrategy}
                            >
                                {dataSource.map((item) => (
                                    <SortableItem
                                        key={item.id}
                                        id={item.id}
                                        question={item.question}
                                    />
                                ))}
                            </SortableContext>
                        </DndContext>
                    ),
                },
            ]}
        />
    );
};
export default ConversationExperience;
