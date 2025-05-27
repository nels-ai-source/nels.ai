import { Permissions } from '@/access';
import { addKnowledgeDocument, updateKnowledge } from '@/services/aigc/knowledge';
import { Knowledge } from '@/types/knowledge';
import {
  DownOutlined,
  // FileAddOutlined,
  // FileTextOutlined,
  FormOutlined,
  LeftOutlined,
} from '@ant-design/icons';
import { FormattedMessage, useAccess, useIntl } from '@umijs/max';
// import type { MenuProps } from 'antd';
import { Button, Space } from 'antd';
import React, { useState } from 'react';
import CreateForm from '../components/CreateForm';
import UploadForm from '../components/UploadForm';
interface HeaderProps {
  data: Knowledge;
  onChange: () => void;
}

export const Header: React.FC<HeaderProps> = ({ data, onChange }) => {
  const access = useAccess();
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const [isUploadModalOpen, setIsUploadModalOpen] = useState(false);
  // const items: MenuProps['items'] = [
  //   {
  //     label: <FormattedMessage id={'knowledge.importType.local.title'} />,
  //     key: '1',
  //     icon: <FileTextOutlined />,
  //   },
  //   {
  //     label: <FormattedMessage id={'knowledge.importType.online.title'} />,
  //     key: '2',
  //     icon: <FileAddOutlined />,
  //   },
  // ];
  const intl = useIntl();
  const handleAddKnowledgeDocument = async (fields: any) => {
    try {
      await addKnowledgeDocument({ ...fields });
      return true;
    } catch (error) {
      console.error(error);
      return false;
    }
  };

  const handleUpdate = async (fields: Knowledge) => {
    try {
      await updateKnowledge({ ...fields });
      return true;
    } catch (error) {
      console.error(error);
      return false;
    }
  };

  return (
    <>
      <UploadForm
        open={isUploadModalOpen}
        onOpenChange={setIsUploadModalOpen}
        onFinish={async (value) => {
          const params: any = {
            knowledgeId: data.id,
            fileId: value.fileId,
            maxTokensPerParagraph: 500,
          };
          const success = await handleAddKnowledgeDocument(params as any);
          if (success) {
            setIsUploadModalOpen(false);
            onChange();
          }
          return success;
        }}
      />
      <CreateForm
        open={isCreateModalOpen}
        onOpenChange={setIsCreateModalOpen}
        onFinish={async (value) => {
          const success = await handleUpdate(value as Knowledge);
          if (success) {
            setIsCreateModalOpen(false);
            onChange();
          }
          return success;
        }}
        type="edit"
        values={data || {}}
      />

      <header className="border-b border-gray-200 z-10 flex items-center justify-between h-8 md:h-8">
        <div className="flex items-center space-x-2">
          <Button
            type="text"
            icon={<LeftOutlined />}
            onClick={() => {
              window.history.back();
            }}
          />
          {data?.name}
          {access.checkAccess(Permissions.KnowledgeDocument.Update) && (
            <Button
              type="text"
              icon={<FormOutlined />}
              title={data?.description || ''}
              onClick={() => {
                setIsCreateModalOpen(true);
              }}
            />
          )}
          <Space style={{ fontSize: 10, color: '#8c8c8c' }}>
            <span>
              {data?.documentCount || 0} {intl.formatMessage({ id: 'knowledge.unit.count' })}
            </span>
            <span>·</span>
            <span>
              {((data?.length || 0) / 1024).toFixed(2)}
              {'k '}
              {intl.formatMessage({ id: 'knowledge.unit.char' })}
            </span>
            <span>·</span>
            <span>
              {data?.retrievalCount || 0} {intl.formatMessage({ id: 'knowledge.unit.times' })}
            </span>
          </Space>
        </div>

        <div className="flex items-center space-x-3">
          {/* <Dropdown menu={{ items: items }}> */}
          {access.checkAccess(Permissions.KnowledgeDocument.Create) && (
            <Button type="primary" onClick={() => setIsUploadModalOpen(true)}>
              <Space>
                <FormattedMessage id={'knowledge.detail.addDocument'} />
                <DownOutlined />
              </Space>
            </Button>
          )}{' '}
          {/* </Dropdown> */}
        </div>
      </header>
    </>
  );
};

export default Header;
