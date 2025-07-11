import { Permissions } from '@/access';
import {
  createKnowledge,
  deleteKnowledge,
  getKnowledgeList,
  updateKnowledge,
} from '@/services/aigc/knowledge';
import { Knowledge } from '@/types/knowledge';
import { PlusOutlined } from '@ant-design/icons';
import type { ActionType, ProColumns } from '@ant-design/pro-components';
import { PageContainer, ProTable } from '@ant-design/pro-components';
import { FormattedMessage, history, useAccess, useIntl } from '@umijs/max';
import { App, Button } from 'antd';
import React, { useRef, useState } from 'react';
import CreateModal from './components/create-modal';

const KnowledgeManager: React.FC = () => {
  const access = useAccess();
  const { message, modal } = App.useApp();
  const intl = useIntl();

  const handleAdd = async (fields: Knowledge) => {
    const hide = message.loading(intl.formatMessage({ id: 'operation.add.loading' }));
    try {
      await createKnowledge({ ...fields });
      hide();
      message.success(intl.formatMessage({ id: 'operation.add.success' }));
      return true;
    } catch (error) {
      hide();
      message.error(intl.formatMessage({ id: 'operation.add.failed' }));
      return false;
    }
  };

  const handleUpdate = async (fields: Knowledge) => {
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

  const handleRemove = async (selectedRows: Knowledge[]) => {
    const hide = message.loading(intl.formatMessage({ id: 'operation.delete.loading' }));
    if (!selectedRows) return true;
    try {
      await deleteKnowledge({
        id: selectedRows.map((row) => row.id),
      });
      hide();
      message.success(intl.formatMessage({ id: 'operation.delete.success' }));
      return true;
    } catch (error) {
      hide();
      message.error(intl.formatMessage({ id: 'operation.delete.failed' }));
      return false;
    }
  };
  const [createModalOpen, handleModalOpen] = useState<boolean>(false);
  const [updateModalOpen, handleUpdateModalOpen] = useState<boolean>(false);
  const actionRef = useRef<ActionType>();
  const [currentRow, setCurrentRow] = useState<Knowledge>();

  const columns: ProColumns<Knowledge>[] = [
    {
      dataIndex: 'index',
      valueType: 'index',
      width: 48,
    },
    {
      title: <FormattedMessage id="knowledge.name" />,
      dataIndex: 'name',
      render: (dom, entity) => (
        <a
          onClick={() => {
            history.push(`/library/knowledge/detail/${entity.id}`);
          }}

        >
          {dom}
        </a>
      ),
    },
    {
      title: <FormattedMessage id="knowledge.description" />,
      dataIndex: 'description',
      ellipsis: true,
    },
    {
      title: <FormattedMessage id="knowledge.documentCount" />,
      dataIndex: 'documentCount',
      search: false,
      renderText: (val: number) => `${val} ${intl.formatMessage({ id: 'knowledge.unit.count' })}`,
    },
    {
      title: <FormattedMessage id="knowledge.length" />,
      dataIndex: 'length',
      search: false,
      renderText: (val: number) =>
        `${((val || 0) / 1024).toFixed(2)}k ${intl.formatMessage({
          id: 'knowledge.unit.char',
        })}`,
    },
    {
      title: <FormattedMessage id="knowledge.retrievalCount" />,
      dataIndex: 'retrievalCount',
      search: false,
      renderText: (val: number) => `${val} ${intl.formatMessage({ id: 'knowledge.unit.times' })}`,
    },
    {
      title: <FormattedMessage id="knowledge.creationTime" />,
      dataIndex: 'creationTime',
      valueType: 'dateTime',
      search: false,
      sorter: true,
    },
    {
      title: <FormattedMessage id="knowledge.status" />,
      dataIndex: 'isEnabled',
      valueEnum: {
        true: {
          text: <FormattedMessage id="status.enable" />,
          status: 'Success',
        },
        false: {
          text: <FormattedMessage id="status.disabled" />,
          status: 'Error',
        },
      },
    },
    {
      title: <FormattedMessage id="actions.lable" />,
      dataIndex: 'option',
      valueType: 'option',
      render: (_, record) => [
        access.checkAccess(Permissions.Knowledge.Update) && (
          <a
            key="edit"
            onClick={() => {
              handleUpdateModalOpen(true);
              setCurrentRow(record);
            }}

          >
            <FormattedMessage id="actions.edit" />
          </a>
        ),

        access.checkAccess(Permissions.Knowledge.Delete) && (
          <a
            key="delete"
            onClick={async () => {
              modal.confirm({
                title: <FormattedMessage id="modal.delete.confirm" />,
                content: <FormattedMessage id="modal.delete.content" />,
                okText: <FormattedMessage id="modal.delete.ok" />,
                cancelText: <FormattedMessage id="modal.delete.cancel" />,
                onOk: async () => {
                  await handleRemove([record]);
                  actionRef.current?.reload();
                },
              });
            }}

          >
            <FormattedMessage id="actions.delete" />
          </a>
        ),
      ],
    },
  ];

  return (
    <PageContainer
      header={{
        title: '',
      }}
      breadcrumb={{}}

    >
      <ProTable<Knowledge, API.PageParams>
        bordered
        actionRef={actionRef}
        rowKey="id"
        search={false}
        options={false}
        cardProps={{
          bodyStyle: {
            padding: 0,
          },
        }}
        toolbar={{
          search: {
            onSearch: (value: string) => {
              console.log(value);
              actionRef.current?.reload();
            },
          },
        }}
        toolBarRender={() => [
          access.checkAccess(Permissions.Knowledge.Create) && (
            <Button
              type="primary"
              key="primary"
              onClick={() => {
                handleModalOpen(true);
              }}
              icon={<PlusOutlined />}

            >
              <FormattedMessage id="knowledge.operation.create" />
            </Button>
          ),
        ]}
        request={async (params) => {
          const { current = 1, pageSize = 10 } = params;
          const response = await getKnowledgeList({
            skipCount: (current - 1) * pageSize,
            maxResultCount: pageSize,
            sorting: 'creationTime desc',
          });
          return {
            data: response.items || [],
            success: true,
            total: response.totalCount,
          };
        }}
        columns={columns}
        pagination={{
          defaultPageSize: 20,
          showSizeChanger: true,
        }}

      />

      <CreateModal
        open={createModalOpen}
        onOpenChange={handleModalOpen}
        onFinish={async (value) => {
          const success = await handleAdd(value as Knowledge);
          if (success) {
            handleModalOpen(false);
            if (actionRef.current) {
              actionRef.current.reload();
            }
          }
          return success;
        }}
        type="create"

      />

      <CreateModal
        open={updateModalOpen}
        onOpenChange={handleUpdateModalOpen}
        onFinish={async (value) => {
          const success = await handleUpdate(value as Knowledge);
          if (success) {
            handleUpdateModalOpen(false);
            if (actionRef.current) {
              actionRef.current.reload();
            }
          }
          return success;
        }}
        type="edit"
        values={currentRow || {}}

      />
    </PageContainer>
  );
};

export default KnowledgeManager;
