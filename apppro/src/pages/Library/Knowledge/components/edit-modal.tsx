import { KnowledgeDocument } from '@/types/knowledge';
import { UploadOutlined } from '@ant-design/icons';
import { CheckCard, ProFormInstance, StepsForm } from '@ant-design/pro-components';
import { FormattedMessage, useIntl } from '@umijs/max';
import { App, Collapse, Form, Modal, Upload, UploadProps } from 'antd';
import React, { useRef } from 'react';
import './UploadForm.less';
const { Dragger } = Upload;
export type UploadFormProps = {
  open: boolean;
  onOpenChange: (visible: boolean) => void;
  onFinish: (values: KnowledgeDocument) => Promise<boolean>;
};

const PARSE_STRATEGIES = [
  {
    title: 'knowledge.upload.parse.accurate.title',
    description: 'knowledge.upload.parse.accurate.description',
    value: '1',
    disabled: true,
  },
  {
    title: 'knowledge.upload.parse.fast.title',
    description: 'knowledge.upload.parse.fast.description',
    value: '2',
    disabled: false,
  },
] as const;

const SEGMENT_STRATEGIES = [
  {
    title: 'knowledge.upload.segment.auto.title',
    description: 'knowledge.upload.segment.auto.description',
    value: '1',
    disabled: false,
  },
  {
    title: 'knowledge.upload.segment.custom.title',
    description: 'knowledge.upload.segment.custom.description',
    value: '2',
    disabled: true,
  },
  {
    title: 'knowledge.upload.segment.hierarchy.title',
    description: 'knowledge.upload.segment.hierarchy.description',
    value: '3',
    disabled: true,
  },
] as const;

const EditModal: React.FC<UploadFormProps> = ({ open, onOpenChange, onFinish }) => {
  const intl = useIntl();
  const formMapRef = useRef<React.MutableRefObject<ProFormInstance<any> | undefined>[]>([]);
  const { message } = App.useApp();
  const uploadForm = formMapRef.current[0]?.current;

  const props: UploadProps = {
    name: 'file',
    multiple: true,
    action: '/api/file/upload',
    maxCount: 1,
    accept: '.jpg,.jpeg,.png,.tiff,.xlsx,.pptx,.docx,.pdf,.md,.txt,.json,.html',
    beforeUpload: (file) => {
      const allowedTypes = [
        'image/jpeg',
        'image/png',
        'image/tiff',
        'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
        'application/vnd.openxmlformats-officedocument.presentationml.presentation',
        'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
        'application/pdf',
        'text/markdown',
        'text/plain',
        'application/json',
        'text/html',
      ];

      if (!allowedTypes.includes(file.type)) {
        message.error(intl.formatMessage({ id: 'upload.file.type.error' }));
        return Upload.LIST_IGNORE;
      }
      return true;
    },
    onChange(info) {
      const { status } = info.file;

      uploadForm?.setFieldsValue({
        file: info.fileList.length > 0 ? info.fileList[0] : undefined,
      });

      if (status === 'done') {
        message.success(intl.formatMessage({ id: 'upload.file.success' }));
      } else if (status === 'error') {
        message.error(intl.formatMessage({ id: 'upload.file.error' }));
      }
    },
  };

  return (
    <App>
      <StepsForm
        formMapRef={formMapRef}
        onFinish={async () => {
          const uploadValues = await formMapRef.current[0]?.current?.validateFields();
          const settingsValues = await formMapRef.current[1]?.current?.validateFields();

          const finalValues = {
            fileId: uploadValues.file.file.response.id,
            ...settingsValues,
          };

          return onFinish(finalValues);
        }}
        stepsFormRender={(dom, submitter) => {
          return (
            <Modal
              title={intl.formatMessage({ id: 'knowledge.upload.title' })}
              width={800}
              open={open}
              onCancel={() => {
                onOpenChange(false);
              }}
              footer={submitter}
              destroyOnClose
            >
              {dom}
            </Modal>
          );
        }}
        formProps={{
          validateMessages: {
            required: intl.formatMessage({ id: 'form.required' }),
          },
        }}
      >
        <StepsForm.StepForm
          name="upload"
          title={intl.formatMessage({ id: 'knowledge.upload.step.file' })}
        >
          <Form.Item
            name="file"
            rules={[
              {
                required: true,
                message: intl.formatMessage({ id: 'knowledge.upload.file.required' }),
              },
            ]}
          >
            <Dragger {...props}>
              <p className="ant-upload-drag-icon">
                <UploadOutlined />
              </p>
              <p className="ant-upload-text">
                <FormattedMessage id="knowledge.upload.file.dragText" />
              </p>
              <p className="ant-upload-hint">
                <FormattedMessage id="knowledge.upload.file.hint" />
              </p>
            </Dragger>
          </Form.Item>
        </StepsForm.StepForm>

        <StepsForm.StepForm
          name="settings"
          title={intl.formatMessage({ id: 'knowledge.upload.step.settings' })}
          initialValues={{
            parseStrategy: '2',
            segmentStrategy: '1',
          }}
        >
          <Form.Item noStyle>
            <Collapse
              collapsible="header"
              ghost
              defaultActiveKey={['parseStrategy', 'segmentStrategy']}
              items={[
                {
                  key: 'parseStrategy',
                  label: <FormattedMessage id="knowledge.upload.parse.title" />,
                  children: (
                    <Form.Item
                      name="parseStrategy"
                      className="mb-1"
                      rules={[
                        {
                          required: true,
                          message: intl.formatMessage({
                            id: 'knowledge.upload.parse.required',
                          }),
                        },
                      ]}
                    >
                      <CheckCard.Group style={{ width: '100%' }} size="small">
                        {PARSE_STRATEGIES.map((strategy) => (
                          <CheckCard
                            key={strategy.value}
                            title={intl.formatMessage({ id: strategy.title })}
                            description={intl.formatMessage({ id: strategy.description })}
                            disabled={strategy.disabled}
                            value={strategy.value}
                            style={{ width: '100%', marginBlockEnd: 5 }}
                          />
                        ))}
                      </CheckCard.Group>
                    </Form.Item>
                  ),
                },
                {
                  key: 'segmentStrategy',
                  label: <FormattedMessage id="knowledge.upload.segment.title" />,

                  children: (
                    <Form.Item
                      name="segmentStrategy"
                      className="mb-1"
                      rules={[
                        {
                          required: true,
                          message: intl.formatMessage({
                            id: 'knowledge.upload.segment.required',
                          }),
                        },
                      ]}
                    >
                      <CheckCard.Group style={{ width: '100%' }} size="small">
                        {SEGMENT_STRATEGIES.map((strategy) => (
                          <CheckCard
                            key={strategy.value}
                            title={intl.formatMessage({ id: strategy.title })}
                            description={intl.formatMessage({ id: strategy.description })}
                            disabled={strategy.disabled}
                            value={strategy.value}
                            style={{ width: '100%', marginBlockEnd: 5 }}
                          />
                        ))}
                      </CheckCard.Group>
                    </Form.Item>
                  ),
                },
              ]}
            />
          </Form.Item>
        </StepsForm.StepForm>
      </StepsForm>
    </App>
  );
};

export default EditModal;
