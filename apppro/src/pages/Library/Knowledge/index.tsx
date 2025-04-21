import {
  createKnowledge,
  deleteKnowledge,
  getKnowledgeList,
  updateKnowledge,
} from '@/services/aigc/knowledge';
import { PlusOutlined } from '@ant-design/icons';
import type { ActionType, ProColumns } from '@ant-design/pro-components';
import { PageContainer, ProTable } from '@ant-design/pro-components';
import { FormattedMessage, history, useAccess, useIntl } from '@umijs/max';
import { App, Button } from 'antd';
import React, { useRef, useState } from 'react';
import CreateForm from './components/CreateForm';

const Knowledge: React.FC = () => {
  const access = useAccess();
  const { message, modal } = App.useApp();
  const intl = useIntl();

  const handleAdd = async (fields: API.RuleListItem) => {
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

  const handleRemove = async (selectedRows: API.KnowledgeItem[]) => {
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
  const [currentRow, setCurrentRow] = useState<API.KnowledgeItem>();

  const columns: ProColumns<API.KnowledgeItem>[] = [
    {
      dataIndex: 'index',
      valueType: 'index',
      width: 48,
    },
    {
      title: <FormattedMessage id="knowledge.table.name" />,
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
      title: <FormattedMessage id="knowledge.table.description" />,
      dataIndex: 'description',
      ellipsis: true,
    },
    {
      title: <FormattedMessage id="knowledge.table.documentCount" />,
      dataIndex: 'documentCount',
      search: false,
      renderText: (val: number) =>
        `${val} ${intl.formatMessage({ id: 'knowledge.table.unit.count' })}`,
    },
    {
      title: <FormattedMessage id="knowledge.table.length" />,
      dataIndex: 'length',
      search: false,
      renderText: (val: number) =>
        `${((val || 0) / 1024).toFixed(2)}k ${intl.formatMessage({
          id: 'knowledge.table.unit.char',
        })}`,
    },
    {
      title: <FormattedMessage id="knowledge.table.retrievalCount" />,
      dataIndex: 'retrievalCount',
      search: false,
      renderText: (val: number) =>
        `${val} ${intl.formatMessage({ id: 'knowledge.table.unit.times' })}`,
    },
    {
      title: <FormattedMessage id="knowledge.table.creationTime" />,
      dataIndex: 'creationTime',
      valueType: 'dateTime',
      search: false,
      sorter: true,
    },
    {
      title: <FormattedMessage id="knowledge.table.status" />,
      dataIndex: 'isEnabled',
      valueEnum: {
        true: {
          text: <FormattedMessage id="knowledge.table.status.enabled" />,
          status: 'Success',
        },
        false: {
          text: <FormattedMessage id="knowledge.table.status.disabled" />,
          status: 'Error',
        },
      },
    },
    {
      title: <FormattedMessage id="knowledge.table.operation" />,
      dataIndex: 'option',
      valueType: 'option',
      render: (_, record) => [
        <a
          key="edit"
          onClick={() => {
            handleUpdateModalOpen(true);
            setCurrentRow(record);
          }}
        >
          <FormattedMessage id="knowledge.table.operation.edit" />
        </a>,
        <a
          key="delete"
          onClick={async () => {
            modal.confirm({
              title: <FormattedMessage id="operation.delete.confirm" />,
              content: <FormattedMessage id="operation.delete.content" />,
              okText: <FormattedMessage id="operation.delete.ok" />,
              cancelText: <FormattedMessage id="operation.delete.cancel" />,
              onOk: async () => {
                await handleRemove([record]);
                actionRef.current?.reload();
              },
            });
          }}
        >
          <FormattedMessage id="knowledge.table.operation.delete" />
        </a>,
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
      <ProTable<API.KnowledgeItem, API.PageParams>
        actionRef={actionRef}
        rowKey="id"
        search={false}
        options={false}
        cardProps={{
          bodyStyle: {
            padding: 10,
            height: 'calc(100vh - 200px)',
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
          access.checkAccess('Aigc.Knowledge.Create') && (
            <Button
              type="primary"
              key="primary"
              onClick={() => {
                handleModalOpen(true);
              }}
            >
              <PlusOutlined /> <FormattedMessage id="pages.searchTable.new" defaultMessage="New" />
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

      <CreateForm
        open={createModalOpen}
        onOpenChange={handleModalOpen}
        onFinish={async (value) => {
          const success = await handleAdd(value as API.KnowledgeItem);
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

      <CreateForm
        open={updateModalOpen}
        onOpenChange={handleUpdateModalOpen}
        onFinish={async (value) => {
          const success = await handleUpdate(value as API.KnowledgeItem);
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

export default Knowledge;
