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
      title: <FormattedMessage id="knowledge.name" data-oid="4jh6p.5" />,
      dataIndex: 'name',
      render: (dom, entity) => (
        <a
          onClick={() => {
            history.push(`/library/knowledge/detail/${entity.id}`);
          }}
          data-oid="skao2vg"
        >
          {dom}
        </a>
      ),
    },
    {
      title: <FormattedMessage id="knowledge.description" data-oid="l02oa-y" />,
      dataIndex: 'description',
      ellipsis: true,
    },
    {
      title: <FormattedMessage id="knowledge.documentCount" data-oid="7ri-axm" />,
      dataIndex: 'documentCount',
      search: false,
      renderText: (val: number) => `${val} ${intl.formatMessage({ id: 'knowledge.unit.count' })}`,
    },
    {
      title: <FormattedMessage id="knowledge.length" data-oid="q7qjeed" />,
      dataIndex: 'length',
      search: false,
      renderText: (val: number) =>
        `${((val || 0) / 1024).toFixed(2)}k ${intl.formatMessage({
          id: 'knowledge.unit.char',
        })}`,
    },
    {
      title: <FormattedMessage id="knowledge.retrievalCount" data-oid="7669y:k" />,
      dataIndex: 'retrievalCount',
      search: false,
      renderText: (val: number) => `${val} ${intl.formatMessage({ id: 'knowledge.unit.times' })}`,
    },
    {
      title: <FormattedMessage id="knowledge.creationTime" data-oid="-:5qhj5" />,
      dataIndex: 'creationTime',
      valueType: 'dateTime',
      search: false,
      sorter: true,
    },
    {
      title: <FormattedMessage id="knowledge.status" data-oid="x0n-t89" />,
      dataIndex: 'isEnabled',
      valueEnum: {
        true: {
          text: <FormattedMessage id="status.enable" data-oid="q4.xw.w" />,
          status: 'Success',
        },
        false: {
          text: <FormattedMessage id="status.disabled" data-oid="uw5is.2" />,
          status: 'Error',
        },
      },
    },
    {
      title: <FormattedMessage id="actions.lable" data-oid="9v5b_0q" />,
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
            data-oid="yp_5vcw"
          >
            <FormattedMessage id="actions.edit" data-oid="1zb_wn6" />
          </a>
        ),

        access.checkAccess(Permissions.Knowledge.Delete) && (
          <a
            key="delete"
            onClick={async () => {
              modal.confirm({
                title: <FormattedMessage id="modal.delete.confirm" data-oid="arnhi9y" />,
                content: <FormattedMessage id="modal.delete.content" data-oid="nkytydv" />,
                okText: <FormattedMessage id="modal.delete.ok" data-oid="ptvwjxj" />,
                cancelText: <FormattedMessage id="modal.delete.cancel" data-oid="8:jkepr" />,
                onOk: async () => {
                  await handleRemove([record]);
                  actionRef.current?.reload();
                },
              });
            }}
            data-oid="l3_fnfo"
          >
            <FormattedMessage id="actions.delete" data-oid="npd-dmr" />
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
      data-oid="b5y0w.y"
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
              icon={<PlusOutlined data-oid="3_:y10r" />}
              data-oid="0z9h7.u"
            >
              <FormattedMessage id="knowledge.operation.create" data-oid="jmcq4oj" />
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
        data-oid="f.y30t0"
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
        data-oid="lzyyp9q"
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
        data-oid="eph9oux"
      />
    </PageContainer>
  );
};

export default KnowledgeManager;
