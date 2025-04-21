import {
  addKnowledgeDocument,
  deleteKnowledgeDocument,
  getKnowledge,
  getKnowledgeDocumentList,
  getParagraphList,
  updateKnowledge,
  updateKnowledgeDocument,
} from '@/services/aigc/knowledge';
import { DeleteOutlined, EditOutlined, FileTextOutlined, LeftOutlined } from '@ant-design/icons';
import { ModalForm, PageContainer, ProCard, ProFormText } from '@ant-design/pro-components';
import { FormattedMessage, useIntl, useParams, history } from '@umijs/max';
import { App, Button, Flex, Input, List } from 'antd';
import React, { useEffect, useMemo, useState } from 'react';
import CreateForm from './components/CreateForm';
import UploadForm from './components/UploadForm';

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
  listContainer: {
    marginTop: '16px',
  },
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

const DocumentPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [knowledgeData, setKnowledgeData] = useState<API.KnowledgeItem>();
  const [knowledgeDocuments, setKnowledgeDocuments] = useState<API.KnowledgeDocument[]>();
  const [selectedDocumnt, setSelectedDocument] = useState<API.KnowledgeDocument>();
  const [KnowledgeDocumentParagraphs, setKnowledgeDocumentParagraphs] =
    useState<API.KnowledgeDocumentParagraph[]>();
  const [updateModalOpen, handleUpdateModalOpen] = useState<boolean>(false);
  const [uploadVisible, setUploadVisible] = useState<boolean>(false);

  const [editDocumentModalOpen, handleEditDocumentModalOpen] = useState<boolean>(false);

  const intl = useIntl();
  const { message, modal } = App.useApp();

  const handleAddKnowledgeDocument = async (fields: any) => {
    const hide = message.loading(intl.formatMessage({ id: 'operation.add.loading' }));
    try {
      await addKnowledgeDocument({ ...fields });
      hide();
      message.success(intl.formatMessage({ id: 'operation.add.success' }));
      return true;
    } catch (error) {
      hide();
      message.error(intl.formatMessage({ id: 'operation.add.failed' }));
      return false;
    }
  };

  const handleGetKnowledge = async (knowledgeId?: string) => {
    if (!knowledgeId) return;
    try {
      const res = await getKnowledge(knowledgeId);
      setKnowledgeData(res);
      const docs = await getKnowledgeDocumentList(knowledgeId);
      setKnowledgeDocuments(docs);
    } catch (error) {
      message.error(intl.formatMessage({ id: 'operation.get.failed' }));
    }
  };

  const handleGetParagraphList = async (knowledgeDocumentId?: string) => {
    if (!knowledgeDocumentId) return;
    try {
      const res = await getParagraphList(knowledgeDocumentId);
      setKnowledgeDocumentParagraphs(res);
    } catch (error) {
      message.error(intl.formatMessage({ id: 'operation.get.failed' }));
    }
  };

  const handleDeleteDocument = async (documentId?: string) => {
    if (!documentId) return;
    const hide = message.loading(intl.formatMessage({ id: 'operation.delete.loading' }));
    try {
      await deleteKnowledgeDocument(documentId);
      hide();
      message.success(intl.formatMessage({ id: 'operation.delete.success' }));
      setSelectedDocument(undefined);
      setKnowledgeDocumentParagraphs(undefined);
      await handleGetKnowledge(id);
    } catch (error) {
      hide();
      message.error(intl.formatMessage({ id: 'operation.delete.failed' }));
    }
  };

  const handleUpdate = async (fields: API.RuleListItem) => {
    const hide = message.loading(intl.formatMessage({ id: 'operation.edit.loading' }));
    try {
      await updateKnowledge({ ...fields });
      hide();
      message.success(intl.formatMessage({ id: 'operation.edit.success' }));
      return true;
    } catch (error) {
      hide();
      message.error(intl.formatMessage({ id: 'operation.edit.failed' }));
      return false;
    }
  };

  const [searchText, setSearchText] = useState<string>('');

  const filteredDocuments = useMemo(() => {
    if (!searchText) return knowledgeDocuments;
    return knowledgeDocuments?.filter((doc) =>
      doc.name.toLowerCase().includes(searchText.toLowerCase()),
    );
  }, [knowledgeDocuments, searchText]);

  useEffect(() => {
    handleGetKnowledge(id);
  }, [id]);

  return (
    <PageContainer
      header={{
        title: (
          <App>
            <Flex align="center">
              <LeftOutlined onClick={() => history.push('/library/knowledge')} />
              <Flex gap={8} style={{ marginLeft: 8 }}>
                <FileTextOutlined style={styles.fileIcon} />
                <Flex vertical>
                  <Flex align="center" gap={8}>
                    <span style={styles.title}>{knowledgeData?.name}</span>
                    <EditOutlined
                      style={styles.iconButton}
                      onClick={() => handleUpdateModalOpen(true)}
                    />
                  </Flex>
                  <Flex gap={8} style={styles.secondaryText}>
                    <span>
                      {knowledgeData?.documentCount || 0}{' '}
                      {intl.formatMessage({ id: 'knowledge.table.unit.count' })}
                    </span>
                    <span>·</span>
                    <span>
                      {((knowledgeData?.length || 0) / 1024).toFixed(2)}
                      {'k '}
                      {intl.formatMessage({ id: 'knowledge.table.unit.char' })}
                    </span>
                    <span>·</span>
                    <span>
                      {knowledgeData?.retrievalCount || 0}{' '}
                      {intl.formatMessage({ id: 'knowledge.table.unit.times' })}
                    </span>
                  </Flex>
                </Flex>
              </Flex>
            </Flex>
          </App>
        ),
      }}
      breadcrumb={{}}
      extra={
        <Button type="link" onClick={() => setUploadVisible(true)}>
          <FormattedMessage id="knowledge.detail.addDocument" />
        </Button>
      }
    >
      <ProCard split="vertical" style={{ minHeight: 'calc(100vh - 235px)' }}>
        <ProCard title="" colSpan="300px">
          <Input.Search
            placeholder={intl.formatMessage({ id: 'search.placeholder' })}
            onSearch={(value) => setSearchText(value)}
            onChange={(e) => setSearchText(e.target.value)}
            allowClear
          />
          <List
            style={styles.listContainer}
            dataSource={filteredDocuments || []}
            renderItem={(item) => (
              <List.Item
                onClick={() => {
                  setSelectedDocument(item);
                  handleGetParagraphList(item.id);
                }}
                style={{
                  ...styles.listItem,
                  backgroundColor: selectedDocumnt === item ? '#f0f0f0' : 'transparent',
                }}
              >
                <div style={styles.listItemText}>{item.name}</div>
              </List.Item>
            )}
          />
        </ProCard>
        <ProCard
          title={
            selectedDocumnt && (
              <Flex align="center" gap={8}>
                <span style={styles.title}>{selectedDocumnt?.name}</span>
                <EditOutlined
                  style={styles.iconButton}
                  onClick={() => handleEditDocumentModalOpen(true)}
                />
              </Flex>
            )
          }
          extra={
            selectedDocumnt && (
              <DeleteOutlined
                style={styles.iconButton}
                onClick={() =>
                  modal.confirm({
                    title: <FormattedMessage id="operation.delete.confirm" />,
                    content: <FormattedMessage id="operation.delete.content" />,
                    okText: <FormattedMessage id="operation.delete.ok" />,
                    cancelText: <FormattedMessage id="operation.delete.cancel" />,
                    onOk: async () => {
                      handleDeleteDocument(selectedDocumnt?.id);
                    },
                  })
                }
              />
            )
          }
          headerBordered
        >
          <List
            dataSource={KnowledgeDocumentParagraphs || []}
            renderItem={(item) => <List.Item>{item.content}</List.Item>}
          />
        </ProCard>
      </ProCard>
      <UploadForm
        open={uploadVisible}
        onOpenChange={setUploadVisible}
        onFinish={async (value) => {
          const dara = {
            knowledgeId: id,
            fileId: value.fileId,
            maxTokensPerParagraph: 500,
          };
          const success = await handleAddKnowledgeDocument(dara as any);
          if (success) {
            setUploadVisible(false);
            await handleGetKnowledge(id);
          }
          return success;
        }}
      />
      <CreateForm
        open={updateModalOpen}
        onOpenChange={handleUpdateModalOpen}
        onFinish={async (value) => {
          const success = await handleUpdate(value as API.KnowledgeItem);
          if (success) {
            handleUpdateModalOpen(false);
            await handleGetKnowledge(id);
          }
          return success;
        }}
        type="edit"
        values={knowledgeData || {}}
      />
      <ModalForm
        title={intl.formatMessage({ id: 'knowledge.document.edit.title' })}
        width="400px"
        open={editDocumentModalOpen}
        onOpenChange={handleEditDocumentModalOpen}
        onFinish={async (value) => {
          try {
            if (selectedDocumnt?.id) {
              await updateKnowledgeDocument(selectedDocumnt.id, value.name);
              handleEditDocumentModalOpen(false);
              await handleGetKnowledge(id);
              selectedDocumnt.name = value.name;
              message.success(intl.formatMessage({ id: 'operation.edit.success' }));
              return true;
            }
            return false;
          } catch (error) {
            message.error(intl.formatMessage({ id: 'operation.edit.failed' }));
            return false;
          }
        }}
        initialValues={selectedDocumnt}
      >
        <ProFormText
          rules={[
            {
              required: true,
              message: intl.formatMessage({ id: 'knowledge.document.name.required' }),
            },
          ]}
          width="md"
          name="name"
          label={intl.formatMessage({ id: 'knowledge.document.name.label' })}
        />
      </ModalForm>
    </PageContainer>
  );
};

export default DocumentPage;
