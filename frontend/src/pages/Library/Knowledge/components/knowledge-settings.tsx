import React from 'react';
import { Divider, Form, Input, Radio, Slider, Space, Switch, Typography } from 'antd';

import { useIntl } from 'umi';
import { KnowledgeOption, SearchStrategy, ReplyMode, SourceDisplayMode } from '@/types/agent';

const { Title } = Typography;


interface KnowledgeSettingsProps {
  option: KnowledgeOption;
  onSettingsChange: (knowledgeOption: KnowledgeOption) => void;
}


export const KnowledgeSettings: React.FC<KnowledgeSettingsProps> = ({
  option: KnowledgeOption,
  onSettingsChange
}) => {

  const intl = useIntl();
  const [form] = Form.useForm<KnowledgeOption>();

  React.useEffect(() => {
    form.setFieldsValue(KnowledgeOption);
  }, [form, KnowledgeOption]);

  const handleValuesChange = (_: any, allValues: KnowledgeOption) => {
    onSettingsChange(allValues);
  };

  return (
    <div className="setting p-6" style={{ width: '600px', minHeight: '300px' }}>
      <Form
        form={form}
        layout="horizontal"
        initialValues={KnowledgeOption}
        onValuesChange={handleValuesChange}
        labelCol={{ span: 6 }}
        wrapperCol={{ span: 18 }}
      >
        <Title level={4} className="mb-4 text-left">
          {intl.formatMessage({ id: 'knowledgeSettings.title' })}
        </Title>

        <Divider orientation="left" orientationMargin="0" className="mb-2">
          <span className="text-sm font-medium text-gray-600">
            {intl.formatMessage({ id: 'knowledgeSettings.sections.recall' })}
          </span>
        </Divider>
        <Form.Item hidden={true} name="id" />
        <Form.Item
          tooltip={intl.formatMessage({ id: 'knowledgeSettings.tooltips.invokeMethod' })}
          label={intl.formatMessage({ id: 'knowledgeSettings.labels.invokeMethod' })}
          name="autoInvoke"
        >
          <Radio.Group>
            <Space direction="horizontal" size="large">
              <Radio value={true} className="text-sm">
                {intl.formatMessage({ id: 'knowledgeSettings.labels.autoInvoke' })}
              </Radio>
              <Radio value={false} className="text-sm">
                {intl.formatMessage({ id: 'knowledgeSettings.labels.manualInvoke' })}
              </Radio>
            </Space>
          </Radio.Group>
        </Form.Item>

        <Form.Item
          label={intl.formatMessage({ id: 'knowledgeSettings.labels.searchStrategy' })}
          tooltip={intl.formatMessage({ id: 'knowledgeSettings.tooltips.searchStrategy' })}
          name="searchStrategy"
          className="mb-2"
        >
          <Radio.Group>
            <Space direction="horizontal" size="middle" wrap>
              <Radio value={SearchStrategy.Hybrid} className="text-sm">
                {intl.formatMessage({ id: 'knowledgeSettings.labels.hybrid' })}
              </Radio>
              <Radio value={SearchStrategy.Semantic} className="text-sm">
                {intl.formatMessage({ id: 'knowledgeSettings.labels.semantic' })}
              </Radio>
              <Radio value={SearchStrategy.FullText} className="text-sm">
                {intl.formatMessage({ id: 'knowledgeSettings.labels.fulltext' })}
              </Radio>
            </Space>
          </Radio.Group>
        </Form.Item>

        <Form.Item
          label={intl.formatMessage({ id: 'knowledgeSettings.labels.maxRecallCount' })}
          name="maxRecallCount"
          className="mb-2"
          tooltip={intl.formatMessage({ id: 'knowledgeSettings.tooltips.maxRecallCount' })}
        >
          <Slider
            min={1}
            max={10}
            step={1}
            style={{ flex: 1 }}
            tooltip={{ formatter: (value) => `${value}` }}
            defaultValue={3}
            marks={{
              1: '1',
              3: intl.formatMessage({ id: 'knowledgeSettings.marks.maxRecallCount.default' }),
              10: '10'
            }}
          />
        </Form.Item>

        <Form.Item
          label={intl.formatMessage({ id: 'knowledgeSettings.labels.minMatchScore' })}
          name="minMatchScore"
          className="mb-2"
          tooltip={intl.formatMessage({ id: 'knowledgeSettings.tooltips.minMatchScore' })}
        >
          <Slider
            min={0}
            max={0.99}
            step={0.01}
            style={{ flex: 1 }}
            tooltip={{ formatter: (value) => `${value}` }}
            defaultValue={0.50}
            marks={{
              0: '0',
              0.50: intl.formatMessage({ id: 'knowledgeSettings.marks.minMatchScore.default' }),
              0.99: '0.99'
            }}
          />
        </Form.Item>

        <Divider orientation="left" orientationMargin="0" className="mb-2 mt-6">
          <span className="text-sm font-medium text-gray-600">
            {intl.formatMessage({ id: 'knowledgeSettings.sections.reply' })}
          </span>
        </Divider>

        <Form.Item
          label={intl.formatMessage({ id: 'knowledgeSettings.labels.replyMode' })}
          name="replyMode"
          className="mb-2"
          tooltip={intl.formatMessage({ id: 'knowledgeSettings.tooltips.replyMode' })}
        >
          <Radio.Group>
            <Space direction="horizontal" size="large">
              <Radio value={ReplyMode.Default} className="text-sm">
                {intl.formatMessage({ id: 'knowledgeSettings.labels.defaultReply' })}
              </Radio>
              <Radio value={ReplyMode.Custom} className="text-sm">
                {intl.formatMessage({ id: 'knowledgeSettings.labels.customReplyMode' })}
              </Radio>
            </Space>
          </Radio.Group>
        </Form.Item>

        <Form.Item
          noStyle
          shouldUpdate={(prevValues, currentValues) => prevValues.replyMode !== currentValues.replyMode}
        >
          {({ getFieldValue }) => {
            const replyMode = getFieldValue('replyMode');
            return replyMode === ReplyMode.Custom ? (
              <Form.Item
                label={intl.formatMessage({ id: 'knowledgeSettings.labels.customReply' })}
                name="customReply"
                className="mb-2"
                tooltip={intl.formatMessage({ id: 'knowledgeSettings.tooltips.customReply' })}
              >
                <Input.TextArea
                  placeholder={intl.formatMessage({ id: 'knowledgeSettings.placeholders.customReply' })}
                  rows={3}
                  maxLength={256}
                  showCount
                />
              </Form.Item>
            ) : null;
          }}
        </Form.Item>

        <Divider orientation="left" orientationMargin="0" className="mb-2 mt-6">
          <span className="text-sm font-medium text-gray-600">
            {intl.formatMessage({ id: 'knowledgeSettings.sections.source' })}
          </span>
        </Divider>

        <Form.Item
          label={intl.formatMessage({ id: 'knowledgeSettings.labels.showSource' })}
          name="showSource"
          className="mb-2"
          tooltip={intl.formatMessage({ id: 'knowledgeSettings.tooltips.showSource' })}
          valuePropName="checked"
        >
          <Switch />
        </Form.Item>

        <Form.Item
          noStyle
          shouldUpdate={(prevValues, currentValues) => prevValues.showSource !== currentValues.showSource}
        >
          {({ getFieldValue }) => {
            const showSource = getFieldValue('showSource');
            return showSource ? (
              <Form.Item
                label={intl.formatMessage({ id: 'knowledgeSettings.labels.sourceDisplayMode' })}
                name="sourceDisplayMode"
                className="mb-2"
                tooltip={intl.formatMessage({ id: 'knowledgeSettings.tooltips.sourceDisplayMode' })}
              >
                <Radio.Group>
                  <Space direction="horizontal" size="large">
                    <Radio value={SourceDisplayMode.Card} className="text-sm">
                      {intl.formatMessage({ id: 'knowledgeSettings.labels.cardMode' })}
                    </Radio>
                    <Radio value={SourceDisplayMode.Text} className="text-sm">
                      {intl.formatMessage({ id: 'knowledgeSettings.labels.textMode' })}
                    </Radio>
                  </Space>
                </Radio.Group>
              </Form.Item>
            ) : null;
          }}
        </Form.Item>
      </Form>
    </div >
  );
};


export default KnowledgeSettings;