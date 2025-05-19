import React from 'react';
import { Input, List } from 'antd';
import { FileTextOutlined } from '@ant-design/icons';
import { KnowledgeDocument } from '../../../types/knowledge';
import { UUID } from 'crypto';

interface DocumentListProps {
    documents?: KnowledgeDocument[];
    selectedDocId: UUID | null;
    onDocumentSelect: (doc: KnowledgeDocument) => void;
}

export const DocumentList: React.FC<DocumentListProps> = ({
    documents,
    selectedDocId,
    onDocumentSelect,
}) => {
    return (
        <div className="flex flex-col h-full gap-4">
            <Input.Search
                placeholder="搜索"
                allowClear
                className="flex-shrink-0"
            />
            <List
                header={'文档列表'}
                className="flex-1 overflow-y-auto"
                dataSource={documents}
                split={false}
                renderItem={(item) => (
                    <List.Item
                        className={`cursor-pointer rounded-lg p-2 transition-colors mb-1 ${
                            selectedDocId === item.id
                                ? 'bg-gray-200'
                                : 'hover:bg-gray-200'
                        }`}
                        onClick={() => onDocumentSelect(item!)}
                    >
                        <List.Item.Meta
                            className="p-1 pl-2"
                            avatar={
                                <FileTextOutlined className="text-lg text-gray-500" />
                            }
                            title={
                                <span className="text-sm truncate block max-w-[230px]">
                                    {item.name}
                                </span>
                            }
                        ></List.Item.Meta>
                    </List.Item>
                )}
            />
        </div>
    );
};

export default DocumentList;
