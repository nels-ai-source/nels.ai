import { ModelSelect } from '@/pages/Model/components/model-select';
import { Knowledge } from '@/types/knowledge';
import { ModelType } from '@/types/model';
import { CloudSyncOutlined, FileExcelOutlined, FileTextOutlined } from '@ant-design/icons';
import { CheckCard, ModalForm, ProFormText, ProFormTextArea } from '@ant-design/pro-components';
import { FormattedMessage, useIntl } from '@umijs/max';
import { App, Col, Form, Row } from 'antd';
import FormItem from 'antd/es/form/FormItem';
import React from 'react';

export type CreateFormProps = {
  open: boolean;
  onOpenChange: (visible: boolean) => void;
  onFinish: (values: Knowledge) => Promise<boolean>;
  values?: Partial<Knowledge>;
  type?: 'create' | 'edit';
};

const CreateForm: React.FC<CreateFormProps> = ({
  open,
  onOpenChange,
  onFinish,
  values,
  type = 'create',
}) => {
  const intl = useIntl();
  const isEdit = type === 'edit';
  const [form] = Form.useForm();

  const handleFinish = async (formValues: Record<string, any>) => {
    const { name = '', description = '', format, importType } = formValues;
    const formData = {
      ...(values || {}),
      name,
      description,
      ...(isEdit
        ? {}
        : {
            format: format || 'text',
            importType: importType || 'local',
          }),
    };
    return onFinish(formData as unknown as Knowledge);
  };

  return (
    <ModalForm
      form={form}
      title={<FormattedMessage id={isEdit ? 'knowledge.edit.title' : 'knowledge.create.title'} />}
      width="800px"
      open={open}
      onOpenChange={(visible) => {
        if (!visible) {
          form.resetFields();
        }
        onOpenChange(visible);
      }}
      onFinish={handleFinish}
      initialValues={{ ...values, format: 'text', importType: 'local' }}
    >
      {!isEdit && (
        <App>
          <Form.Item name="format" label={<FormattedMessage id="knowledge.format" />}>
            <CheckCard.Group style={{ width: '100%' }}>
              <Row>
                <Col span={8}>
                  <CheckCard
                    title={<FormattedMessage id="knowledge.formatType.text.title" />}
                    avatar={<FileTextOutlined style={{ fontSize: 24 }} />}
                    description={<FormattedMessage id="knowledge.formatType.text.description" />}
                    value="text"
                    style={{ width: '95%', marginBlockEnd: 0 }}
                  />
                </Col>
                <Col span={8}>
                  <CheckCard
                    title={<FormattedMessage id="knowledge.formatType.table.title" />}
                    avatar={<FileExcelOutlined style={{ fontSize: 24 }} />}
                    description={
                      <FormattedMessage id="knowledge.formatType.table.description" />
                    }
                    value="table"
                    style={{ width: '95%', marginBlockEnd: 0 }}
                    disabled
                  />
                </Col>
                <Col span={8}>
                  <CheckCard
                    title={<FormattedMessage id="knowledge.formatType.image.title" />}
                    avatar={<FileExcelOutlined style={{ fontSize: 24 }} />}
                    description={
                      <FormattedMessage id="knowledge.formatType.image.description" />
                    }
                    value="image"
                    style={{ width: '95%', marginBlockEnd: 0 }}
                    disabled
                  />
                </Col>
              </Row>
            </CheckCard.Group>
          </Form.Item>
        </App>
      )}
      <ProFormText
        label={<FormattedMessage id="knowledge.name" />}
        rules={[
          {
            required: true,
            message: intl.formatMessage({ id: 'knowledge.required.name' }),
          },
        ]}
        width="lg"
        name="name"
        placeholder={intl.formatMessage({ id: 'knowledge.placeholder.name' })}
        fieldProps={{
          maxLength: 64,
          showCount: true,
          style: { width: '100%' },
        }}
      />
      <FormItem
        name="model"
        label={<FormattedMessage id="knowledge.model" />}
        rules={[
          {
            required: true,
            message: intl.formatMessage({ id: 'knowledge.required.model' }),
          },
        ]}
      >
        <ModelSelect filter={{ type: ModelType.Embedding }}></ModelSelect>
      </FormItem>
      <ProFormTextArea
        label={<FormattedMessage id="knowledge.description" />}
        width="lg"
        name="description"
        placeholder={intl.formatMessage({ id: 'knowledge.placeholder.description' })}
        fieldProps={{
          maxLength: 512,
          showCount: true,
          style: { width: '100%' },
        }}
      />
      {!isEdit && (
        <App>
          <Form.Item
            name="importType"
            label={<FormattedMessage id="knowledge.import" />}
          >
            <CheckCard.Group style={{ width: '100%' }}>
              <Row>
                <Col span={12}>
                  <CheckCard
                    title={<FormattedMessage id="knowledge.importType.local.title" />}
                    avatar={<FileTextOutlined style={{ fontSize: 20 }} />}
                    description={
                      <FormattedMessage id="knowledge.importType.local.description" />
                    }
                    value="local"
                    style={{ width: '95%', marginBlockEnd: 0 }}
                  />
                </Col>
                <Col span={12}>
                  <CheckCard
                    title={<FormattedMessage id="knowledge.importType.online.title" />}
                    avatar={<CloudSyncOutlined style={{ fontSize: 20 }} />}
                    description={
                      <FormattedMessage id="knowledge.importType.online.description" />
                    }
                    value="online"
                    style={{ width: '95%', marginBlockEnd: 0 }}
                    disabled
                  />
                </Col>
              </Row>
            </CheckCard.Group>
          </Form.Item>
        </App>
      )}
    </ModalForm>
  );
};

export default CreateForm;
