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
import CreateModal from '../components/create-modal';
import EditModal from '../components/edit-modal';
interface HeaderProps {
  data: Knowledge;
  onChange: () => void;
}

export const Header: React.FC<HeaderProps> = ({ data, onChange }) => {
  const access = useAccess();
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
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
      <EditModal
        open={isEditModalOpen}
        onOpenChange={setIsEditModalOpen}
        onFinish={async (value) => {
          const params: any = {
            knowledgeId: data.id,
            fileId: value.fileId,
            maxTokensPerParagraph: 500,
          };
          const success = await handleAddKnowledgeDocument(params as any);
          if (success) {
            setIsEditModalOpen(false);
            onChange();
          }
          return success;
        }}
        data-oid="j2fqfpv"
      />

      <CreateModal
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
        data-oid="jyeaq_."
      />

      <header
        className="border-b border-gray-200 z-10 flex items-center justify-between h-8 md:h-8"
        data-oid="yhrgvc0"
      >
        <div className="flex items-center space-x-2" data-oid="dg58fz4">
          <Button
            type="text"
            icon={<LeftOutlined data-oid=":uzxr2r" />}
            onClick={() => {
              window.history.back();
            }}
            data-oid="9zzbofd"
          />

          {data?.name}
          {access.checkAccess(Permissions.KnowledgeDocument.Update) && (
            <Button
              type="text"
              icon={<FormOutlined data-oid="jh41lht" />}
              title={data?.description || ''}
              onClick={() => {
                setIsCreateModalOpen(true);
              }}
              data-oid="9:w8jv1"
            />
          )}
          <Space style={{ fontSize: 10, color: '#8c8c8c' }} data-oid="fxb4x7x">
            <span data-oid="cb8uqwo">
              {data?.documentCount || 0} {intl.formatMessage({ id: 'knowledge.unit.count' })}
            </span>
            <span data-oid="4qfbxwa">·</span>
            <span data-oid="s2e17-m">
              {((data?.length || 0) / 1024).toFixed(2)}
              {'k '}
              {intl.formatMessage({ id: 'knowledge.unit.char' })}
            </span>
            <span data-oid="f659k2q">·</span>
            <span data-oid=".9.5::o">
              {data?.retrievalCount || 0} {intl.formatMessage({ id: 'knowledge.unit.times' })}
            </span>
          </Space>
        </div>

        <div className="flex items-center space-x-3" data-oid="72l3agt">
          {/* <Dropdown menu={{ items: items }}> */}
          {access.checkAccess(Permissions.KnowledgeDocument.Create) && (
            <Button type="primary" onClick={() => setIsEditModalOpen(true)} data-oid="dndzftn">
              <Space data-oid="w6fwbcg">
                <FormattedMessage id={'knowledge.detail.addDocument'} data-oid="lg0fuua" />
                <DownOutlined data-oid="be6lzt4" />
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
