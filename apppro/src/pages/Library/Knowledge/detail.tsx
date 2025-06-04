import { Permissions } from '@/access';
import {
  deleteKnowledgeDocument,
  getKnowledge,
  getParagraphList,
  updateKnowledgeDocument,
} from '@/services/aigc/knowledge';
import { Knowledge, KnowledgeDocument, KnowledgeDocumentParagraph } from '@/types/knowledge';
import { DeleteOutlined, FileTextOutlined, FormOutlined, SettingOutlined } from '@ant-design/icons';
import { ModalForm, ProFormText } from '@ant-design/pro-components';
import { FormattedMessage, useAccess, useIntl, useParams } from '@umijs/max';
import { App, Button, List, message, Skeleton, Space } from 'antd';
import React, { useEffect, useState } from 'react';
import './detail/detail.css';
import { DocumentList } from './detail/documentList';
import { Header } from './detail/header';
const DocumentHeader: React.FC<{
  intl: ReturnType<typeof useIntl>;
  access: ReturnType<typeof useAccess>;
  document: KnowledgeDocument;
  onUpdate: () => void;
  onDelete: (id: string) => void;
}> = ({ intl, access, document, onUpdate, onDelete }) => (
  <header
    className="border-b border-gray-200 z-10 flex items-center justify-between h-14 px-2 md:h-14 md:px-4 shadow-sm"
    data-oid=":.50yxj"
  >
    <Space className="flex items-center" data-oid="reyhmfq">
      <FileTextOutlined data-oid="ry.8xow" />
      {document?.name}
      {access.checkAccess(Permissions.KnowledgeDocument.Update) && (
        <Button
          type="text"
          icon={<FormOutlined data-oid="m0v.t5." />}
          onClick={onUpdate}
          data-oid="q:kv257"
        />
      )}
    </Space>

    <Space className="flex items-center" data-oid="dy_a:yh">
      {access.checkAccess(Permissions.KnowledgeDocument.Update) && (
        <Button
          type="text"
          size="small"
          icon={<SettingOutlined data-oid="gc-0dcm" />}
          title={intl.formatMessage({ id: 'knowledge.detail.updateSettings' })}
          onClick={() => {}}
          data-oid="3:m5fwu"
        />
      )}
      {access.checkAccess(Permissions.KnowledgeDocument.Delete) && (
        <Button
          type="text"
          size="small"
          icon={<DeleteOutlined data-oid="3nkh.md" />}
          title={intl.formatMessage({ id: 'knowledge.detail.deleteDocument' })}
          onClick={() => {
            onDelete(document.id);
          }}
          data-oid="908t9us"
        />
      )}
    </Space>
  </header>
);

const DocumentContent: React.FC<{
  intl: ReturnType<typeof useIntl>;
  access: ReturnType<typeof useAccess>;
  paragraphs: KnowledgeDocumentParagraph[];
}> = ({ intl, access, paragraphs }) => (
  <main className="flex-1 overflow-y-auto p-4 md:p-4" data-oid="ds6du9-">
    <List
      size="small"
      dataSource={paragraphs}
      split={false}
      renderItem={(item) => (
        <List.Item
          className={`
                        cursor-pointer 
                        rounded-lg 
                        p-2 
                        transition-colors 
                        mb-2
                        bg-gray-100 
                        relative 
                        group
                        hover:bg-gray-200
                    `}
          data-oid="9ua:-j3"
        >
          <div className="flex items-center p-2" data-oid="uak4r27">
            <div className="flex-1" data-oid="8g6f_g4">
              <div className="text-sm text-gray-600 leading-relaxed" data-oid="qfhkvc7">
                {item.content}
              </div>
            </div>
            {access.checkAccess(Permissions.KnowledgeDocument.Update) && (
              <div
                className="absolute top-2 right-2 flex items-center gap-1 opacity-0 group-hover:opacity-100 transition-all duration-200 ease-in-out bg-white rounded-md shadow-sm p-1"
                data-oid="omcunek"
              >
                <Button
                  type="text"
                  size="small"
                  icon={<FormOutlined data-oid="9ln3ua." />}
                  className="text-gray-500 hover:text-blue-500"
                  title={intl.formatMessage({ id: 'knowledge.detail.updateParagraph' })}
                  onClick={() => {
                    message.success(intl.formatMessage({ id: 'operation.update.success' }));
                  }}
                  data-oid="rcjzdr5"
                />

                <Button
                  type="text"
                  size="small"
                  icon={<DeleteOutlined data-oid="19xpenr" />}
                  className="text-gray-500 hover:text-red-500"
                  title={intl.formatMessage({ id: 'knowledge.detail.deleteParagraph' })}
                  data-oid="8j07w1h"
                />

                <Button
                  type="text"
                  size="small"
                  icon={<SettingOutlined data-oid="zc8a2my" />}
                  className="text-gray-500 hover:text-blue-500"
                  title={intl.formatMessage({ id: 'knowledge.detail.settingParagraph' })}
                  data-oid="-wtrs_3"
                />
              </div>
            )}
          </div>
        </List.Item>
      )}
      data-oid="6tmw372"
    />
  </main>
);

export function KnowledgeDetail() {
  const [knowledge, setKnowledge] = useState<Knowledge>();
  const [selectedDocument, setSelectedDocument] = useState<KnowledgeDocument>();
  const [paragraphs, setParagraphs] = useState<KnowledgeDocumentParagraph[]>();
  const [isUploadDocumentModalOpen, setIsUploadDocumentModalOpen] = useState(false);
  const [loading, setLoading] = useState(true);
  const { id } = useParams<{ id: string }>();
  const { message, modal } = App.useApp();
  const intl = useIntl();
  const access = useAccess();
  const handleGetKnowledge = async (knowledgeId: string) => {
    if (!knowledgeId) return;
    setLoading(false);
    try {
      const res = await getKnowledge(knowledgeId);
      setKnowledge(res);
    } catch (error) {
      console.error(error);
    } finally {
      setLoading(false);
    }
  };
  const handleGetParagraphList = async (knowledgeDocumentId?: string) => {
    if (!knowledgeDocumentId) return;
    setLoading(false);
    try {
      const res = await getParagraphList(knowledgeDocumentId);
      setParagraphs(res);
    } catch (error) {
      console.error(error);
    } finally {
      setLoading(false);
    }
  };
  const handleDeleteDocument = async (documentId?: string) => {
    if (!documentId) return;
    try {
      setLoading(true);
      await deleteKnowledgeDocument(documentId);
      message.success(intl.formatMessage({ id: 'actions.success' }));
      await handleGetKnowledge(id as string);
    } catch (error) {
      message.error(intl.formatMessage({ id: 'actions.failed' }));
    } finally {
      setLoading(false);
    }
  };
  useEffect(() => {
    if (id) {
      handleGetKnowledge(id);
    }
  }, [id]);
  useEffect(() => {
    if (knowledge?.documents && knowledge.documents.length > 0) {
      setSelectedDocument(knowledge.documents[0]);
    }
  }, [knowledge]);
  useEffect(() => {
    if (selectedDocument?.id) {
      handleGetParagraphList(selectedDocument.id);
    }
  }, [selectedDocument]);

  return loading ? (
    <Skeleton active data-oid="ejtg468" />
  ) : (
    <>
      <ModalForm
        title={intl.formatMessage({ id: 'knowledge.document.title' })}
        width="480px"
        open={isUploadDocumentModalOpen}
        onOpenChange={setIsUploadDocumentModalOpen}
        onFinish={async (value) => {
          try {
            if (selectedDocument?.id) {
              await updateKnowledgeDocument(selectedDocument.id, value.name);
              setIsUploadDocumentModalOpen(false);
              await handleGetKnowledge(id as string);
              selectedDocument.name = value.name;
              message.success(intl.formatMessage({ id: 'actions.success' }));
              return true;
            }
            return false;
          } catch (error) {
            message.error(intl.formatMessage({ id: 'actions.failed' }));
            return false;
          }
        }}
        initialValues={selectedDocument}
        data-oid="062hh2s"
      >
        <ProFormText
          rules={[
            {
              required: true,
              message: intl.formatMessage({ id: 'knowledge.document.required.name' }),
            },
          ]}
          name="name"
          label={intl.formatMessage({ id: 'knowledge.document.name' })}
          placeholder={intl.formatMessage({ id: 'knowledge.document.placeholder.name' })}
          data-oid=":vbda9."
        />
      </ModalForm>
      {/* Header */}
      <div className="flex-shrink-0 mb-4" data-oid="gx8dbxr">
        <Header
          data={knowledge as Knowledge}
          onChange={() => {
            handleGetKnowledge(id as string);
          }}
          data-oid=":8.71r3"
        />
      </div>
      {/* Main Content*/}
      <main
        className="flex-1 min-h-0 flex "
        style={{ border: '1px solid #e5e7eb' }}
        data-oid="q0x5i-_"
      >
        {/* Left Sidebar */}
        <aside
          className="flex-shrink-0 w-[300px] overflow-y-auto bg-white shadow-sm p-4"
          data-oid="d384ond"
        >
          <DocumentList
            documents={knowledge?.documents}
            selectedDocId={selectedDocument?.id}
            onDocumentSelect={setSelectedDocument}
            data-oid="1pn:kcf"
          />
        </aside>

        {/* Main Content Area */}
        <div className="flex-1 overflow-y-auto" data-oid="4v3p91w">
          <DocumentHeader
            document={selectedDocument as KnowledgeDocument}
            intl={intl}
            access={access}
            onUpdate={() => {
              setIsUploadDocumentModalOpen(true);
            }}
            onDelete={(id: string) => {
              modal.confirm({
                title: <FormattedMessage id="modal.delete.confirm" data-oid="yea-:eq" />,
                content: <FormattedMessage id="modal.delete.content" data-oid=":w_hsn3" />,
                okText: <FormattedMessage id="modal.delete.ok" data-oid="z1x8as." />,
                cancelText: <FormattedMessage id="modal.delete.cancel" data-oid="03ms01_" />,
                onOk: async () => {
                  await handleDeleteDocument(id);
                },
              });
            }}
            data-oid="alodecq"
          />

          <DocumentContent
            paragraphs={paragraphs as KnowledgeDocumentParagraph[]}
            intl={intl}
            access={access}
            data-oid="0o-._jd"
          />
        </div>
      </main>
    </>
  );
}
export default KnowledgeDetail;
