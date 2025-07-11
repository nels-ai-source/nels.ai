import { KnowledgeDocument } from '@/types/knowledge';
import { FileTextOutlined } from '@ant-design/icons';
import { useIntl } from '@umijs/max';
import { Input, List, Space } from 'antd';
import { UUID } from 'crypto';
import React, { useMemo, useState } from 'react';

interface DocumentListProps {
  documents?: KnowledgeDocument[];
  selectedDocId: UUID | undefined;
  onDocumentSelect: (doc: KnowledgeDocument) => void;
}
const styles = {
  iconButton: {
    fontSize: 12,
    color: '#8c8c8c',
    cursor: 'pointer',
  },
  secondaryText: {
    fontSize: 10,
    color: '#8c8c8c',
  },
  title: {
    fontSize: 12,
  },
  fileIcon: {
    fontSize: 24,
  },
  listContainer: {},
  listItem: {
    cursor: 'pointer',
    padding: '8px 12px',
    borderRadius: 4,
    transition: 'background-color 0.3s',
  },
  listItemText: {
    overflow: 'hidden',
    textOverflow: 'ellipsis',
    whiteSpace: 'nowrap',
    width: '100%',
  },
} as const;
export const DocumentList: React.FC<DocumentListProps> = ({
  documents,
  selectedDocId,
  onDocumentSelect,
}) => {
  const intl = useIntl();

  const [searchText, setSearchText] = useState<string>('');

  const filteredDocuments = useMemo(() => {
    if (!searchText) return documents;
    return documents?.filter((doc) => doc.name.toLowerCase().includes(searchText.toLowerCase()));
  }, [documents, searchText]);

  return (
    <div className="flex flex-col h-full gap-4" style={{ width: '300px' }}>
      <Input.Search
        placeholder={intl.formatMessage({ id: 'placeholder.search' })}
        onSearch={(value) => setSearchText(value)}
        onChange={(e) => setSearchText(e.target.value)}
        allowClear

      />

      <List
        style={styles.listContainer}
        dataSource={filteredDocuments || []}
        renderItem={(item) => (
          <List.Item
            onClick={() => onDocumentSelect(item)}
            style={{
              ...styles.listItem,
              backgroundColor: selectedDocId === item.id ? '#f0f0f0' : 'transparent',
            }}

          >
            <div className="w-full overflow-hidden">
              <Space className="w-full" size={8}>
                <FileTextOutlined
                  className="text-lg text-gray-500 flex-shrink-0"

                />

                <span
                  className="text-sm truncate inline-block"
                  style={{ width: '250px' }}

                >
                  {' '}
                  {item.name}
                </span>
              </Space>
            </div>
          </List.Item>
        )}

      />
    </div>
  );
};

export default DocumentList;
