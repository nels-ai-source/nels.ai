import { ProFormText, ModalForm } from '@ant-design/pro-components';
import { Form } from 'antd';
import { useIntl } from '@umijs/max';

export type SetingFormProps = {
  open: boolean;
  onOpenChange: (visible: boolean) => void;
  onFinish: (values: API.ModelSettingDto) => Promise<boolean>;
  provider?: number;
};

const SetingForm: React.FC<SetingFormProps> = ({ open, onOpenChange, onFinish, provider = 0 }) => {
  const [form] = Form.useForm<API.ModelSettingDto>();
  const intl = useIntl();

  const handleFinish = async (values: any) => {
    const formData: API.ModelSettingDto = {
      provider: provider,
      ...values,
    };
    return onFinish(formData);
  };
  type FieldConfig = {
    name: string;
    label: string;
    required: boolean;
    isPassword?: boolean;
  };
  const providerFields: { [key: number]: FieldConfig[] } = {
    0: [
      // OpenAI
      { name: 'accessKey', label: 'model.setting.label.apiKey', required: true, isPassword: true },
    ],
    1: [
      // AzureOpenAI
      { name: 'endpoint', label: 'model.setting.label.endpoint', required: true },
      { name: 'accessKey', label: 'model.setting.label.apiKey', required: true, isPassword: true },
      { name: 'deploymentName', label: 'model.setting.label.deploymentName', required: true },
    ],
    21: [
      // DashScope
      { name: 'accessKey', label: 'model.setting.label.apiKey', required: true, isPassword: true },
    ],
    22: [
      // DeepSeek
      { name: 'accessKey', label: 'model.setting.label.apiKey', required: true, isPassword: true },
    ],
    23: [
      // Kimi
      { name: 'accessKey', label: 'model.setting.label.apiKey', required: true, isPassword: true },
      {
        name: 'secretKey',
        label: 'model.setting.label.secretKey',
        required: true,
        isPassword: true,
      },
    ],
  };

  return (
    <ModalForm
      form={form}
      open={open}
      onOpenChange={(visible) => {
        if (!visible) {
          form.resetFields();
        }
        onOpenChange(visible);
      }}
      initialValues={{ provider: provider }}
      onFinish={handleFinish}
      title={intl.formatMessage({ id: 'model.setting.title' })}
    >
      {providerFields[provider]?.map((field) =>
        field.isPassword ? (
          <ProFormText.Password
            key={field.name}
            name={field.name}
            label={intl.formatMessage({ id: field.label })}
            rules={
              field.required
                ? [
                    {
                      required: true,
                      message: intl.formatMessage({ id: 'form.required' }),
                    },
                  ]
                : []
            }
            fieldProps={{
              autoComplete: 'new-password',
            }}
          />
        ) : (
          <ProFormText
            key={field.name}
            name={field.name}
            label={intl.formatMessage({ id: field.label })}
            rules={
              field.required
                ? [{ required: true, message: intl.formatMessage({ id: 'form.required' }) }]
                : []
            }
          />
        ),
      )}
    </ModalForm>
  );
};

export default SetingForm;
