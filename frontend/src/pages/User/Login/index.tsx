import { login } from '@/services/aigc/api';
import { EyeInvisibleOutlined, EyeTwoTone, LockOutlined, UserOutlined } from '@ant-design/icons';
import { LoginForm, ProFormCheckbox, ProFormText } from '@ant-design/pro-components';
import { FormattedMessage, Helmet, SelectLang, useIntl, useModel } from '@umijs/max';
import { App, Card, Divider, message, Space, Tabs, Typography } from 'antd';
import { createStyles } from 'antd-style';
import React, { useState } from 'react';
import Settings from '../../../../config/defaultSettings';

const { Title, Text } = Typography;

const useStyles = createStyles(({ token }) => {
  return {
    action: {
      marginLeft: '8px',
      color: 'rgba(0, 0, 0, 0.2)',
      fontSize: '24px',
      verticalAlign: 'middle',
      cursor: 'pointer',
      transition: 'color 0.3s',
      '&:hover': {
        color: token.colorPrimaryActive,
      },
    },
    lang: {
      width: 42,
      height: 42,
      lineHeight: '42px',
      position: 'fixed',
      right: 16,
      top: 16,
      borderRadius: token.borderRadius,
      zIndex: 1000,
      ':hover': {
        backgroundColor: token.colorBgTextHover,
      },
    },
    container: {
      display: 'flex',
      flexDirection: 'column',
      height: '100vh',
      overflow: 'auto',
      background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
      position: 'relative',
    },
    backgroundOverlay: {
      position: 'absolute',
      top: 0,
      left: 0,
      right: 0,
      bottom: 0,
      backgroundImage: "url('/auth_bg.png')",
      backgroundSize: 'cover',
      backgroundPosition: 'center',
      opacity: 0.1,
      zIndex: 1,
    },
    contentWrapper: {
      position: 'relative',
      zIndex: 2,
      flex: 1,
      display: 'flex',
      flexDirection: 'column',
      justifyContent: 'center',
      alignItems: 'center',
      padding: '20px',
    },
    loginCard: {
      width: '100%',
      maxWidth: 450,
      borderRadius: 16,
      boxShadow: '0 20px 40px rgba(0, 0, 0, 0.1)',
      border: 'none',
      background: 'rgba(255, 255, 255, 0.95)',
      backdropFilter: 'blur(10px)',
    },
    logoContainer: {
      position: 'absolute',
      top: '20px',
      left: '20px',
      zIndex: 1000,
      display: 'flex',
      alignItems: 'center',
      gap: '12px',
    },
    logo: {
      width: '120px',
      height: '40px',
    },
    welcomeSection: {
      textAlign: 'center',
      marginBottom: 32,
    },
    welcomeTitle: {
      fontSize: '28px',
      fontWeight: 600,
      color: token.colorText,
      marginBottom: 8,
    },
    welcomeSubtitle: {
      fontSize: '16px',
      color: token.colorTextSecondary,
      marginBottom: 0,
    },
    formContainer: {
      padding: '0 8px',
    },
    submitButton: {
      width: '100%',
      height: 48,
      fontSize: '16px',
      fontWeight: 500,
      borderRadius: 8,
    },
    divider: {
      margin: '24px 0',
    },
    socialLogin: {
      textAlign: 'center',
    },
    socialButtons: {
      display: 'flex',
      gap: '12px',
      justifyContent: 'center',
      marginTop: 16,
    },
    socialButton: {
      width: 48,
      height: 48,
      borderRadius: '50%',
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center',
      border: `1px solid ${token.colorBorder}`,
      background: token.colorBgContainer,
      cursor: 'pointer',
      transition: 'all 0.3s',
      '&:hover': {
        borderColor: token.colorPrimary,
        color: token.colorPrimary,
        transform: 'translateY(-2px)',
      },
    },
    footer: {
      textAlign: 'center',
      marginTop: 24,
      color: token.colorTextSecondary,
      fontSize: '14px',
    },
  };
});

const Lang = () => {
  const { styles } = useStyles();

  return <div className={styles.lang}>{SelectLang && <SelectLang />}</div>;
};

const Login: React.FC = () => {
  const [type, setType] = useState<string>('account');
  const [loading, setLoading] = useState(false);
  const { initialState, setInitialState } = useModel('@@initialState');
  const { styles } = useStyles();
  const intl = useIntl();

  const fetchUserInfo = async () => {
    const userInfo = await initialState?.fetchUserInfo?.();
    if (userInfo) {
      setInitialState((s) => ({
        ...s,
        currentUser: userInfo,
      }));
    }
  };

  const handleSubmit = async (values: API.LoginParams) => {
    setLoading(true);
    try {
      const formData = {
        client_id: process.env.UMI_APP_CLIENT_ID,
        grant_type: process.env.UMI_APP_GRANT_TYPE,
        scope: process.env.UMI_APP_SCOPE,
        client_secret: process.env.UMI_APP_CLIENT_SECRET,
        username: values.username || '',
        password: values.password || '',
      };

      const msg = await login(formData);
      if (msg.access_token) {
        localStorage.setItem('access_token', msg.access_token);
        const defaultLoginSuccessMessage = intl.formatMessage({
          id: 'user.login.success',
        });
        message.success(defaultLoginSuccessMessage);
        await fetchUserInfo();
        const urlParams = new URL(window.location.href).searchParams;
        const redirect = urlParams.get('redirect');
        window.location.href = redirect || '/home';
        return;
      }
    } catch (error) {
      const defaultLoginFailureMessage = intl.formatMessage({
        id: 'user.login.failure',
      });
      console.log(error);
      message.error(defaultLoginFailureMessage);
    } finally {
      setLoading(false);
    }
  };

  return (
    <App>
      <div className={styles.container}>
        <div className={styles.backgroundOverlay} />

        <Helmet>
          <title>
            {intl.formatMessage({
              id: 'menu.login',
            })}
            - {Settings.title}
          </title>
        </Helmet>

        <Lang />

        <div className={styles.logoContainer}>
          <img alt="logo" src="/nels_logo3.svg" className={styles.logo} />
        </div>

        <div className={styles.contentWrapper}>
          <Card className={styles.loginCard}>
            <div className={styles.welcomeSection}>
              <Title level={2} className={styles.welcomeTitle}>
                {intl.formatMessage({
                  id: 'user.layouts.userLayout.title',
                  defaultMessage: '欢迎使用 NelsAI',
                })}
              </Title>
              <Text className={styles.welcomeSubtitle}>
                {intl.formatMessage({
                  id: 'user.login.subtitle',
                  defaultMessage: '智能 AIGC 应用开发平台',
                })}
              </Text>
            </div>

            <div className={styles.formContainer}>
              <LoginForm
                loading={loading}
                contentStyle={{
                  minWidth: 'auto',
                  maxWidth: 'none',
                }}
                initialValues={{
                  autoLogin: true,
                }}
                onFinish={async (values) => {
                  await handleSubmit(values as API.LoginParams);
                }}
              >
                <Tabs
                  activeKey={type}
                  onChange={setType}
                  centered
                  items={[
                    {
                      key: 'account',
                      label: intl.formatMessage({
                        id: 'user.login.accountLogin.tab',
                      }),
                    },
                  ]}
                />

                {type === 'account' && (
                  <>
                    <ProFormText
                      name="username"
                      fieldProps={{
                        size: 'large',
                        prefix: <UserOutlined style={{ color: '#1890ff' }} />,
                      }}
                      placeholder={intl.formatMessage({
                        id: 'user.login.username.placeholder',
                        defaultMessage: '请输入用户名',
                      })}
                      rules={[
                        {
                          required: true,
                          message: (
                            <FormattedMessage
                              id="user.login.username.required"
                              defaultMessage="请输入用户名!"
                            />
                          ),
                        },
                      ]}
                    />

                    <ProFormText.Password
                      name="password"
                      fieldProps={{
                        size: 'large',
                        prefix: <LockOutlined style={{ color: '#1890ff' }} />,
                        iconRender: (visible) =>
                          visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />,
                      }}
                      placeholder={intl.formatMessage({
                        id: 'user.login.password.placeholder',
                        defaultMessage: '请输入密码',
                      })}
                      rules={[
                        {
                          required: true,
                          message: (
                            <FormattedMessage
                              id="user.login.password.required"
                              defaultMessage="请输入密码！"
                            />
                          ),
                        },
                      ]}
                    />
                  </>
                )}

                <div
                  style={{
                    marginBottom: 24,
                    display: 'flex',
                    justifyContent: 'space-between',
                    alignItems: 'center',
                  }}
                >
                  <ProFormCheckbox noStyle name="autoLogin">
                    <FormattedMessage id="user.login.rememberMe" />
                  </ProFormCheckbox>
                  <a
                    style={{
                      color: '#1890ff',
                    }}
                  >
                    <FormattedMessage id="user.login.forgotPassword" />
                  </a>
                </div>
              </LoginForm>

              <Divider className={styles.divider}>
                <Text type="secondary">
                  {intl.formatMessage({
                    id: 'user.login.loginWith',
                    defaultMessage: '其他登录方式',
                  })}
                </Text>
              </Divider>

              <div className={styles.socialLogin}>
                <Space className={styles.socialButtons}>
                  <div className={styles.socialButton} title="微信登录">
                    <svg width="20" height="20" viewBox="0 0 24 24" fill="currentColor">
                      <path d="M8.691 2.188C3.891 2.188 0 5.476 0 9.53c0 2.212 1.17 4.203 3.002 5.55a.59.59 0 0 1 .213.665l-.39 1.48c-.019.07-.048.141-.048.213 0 .163.13.295.29.295a.326.326 0 0 0 .167-.054l1.903-1.114a.864.864 0 0 1 .717-.098 10.16 10.16 0 0 0 2.837.403c.276 0 .543-.027.811-.05-.857-2.578.157-4.972 1.932-6.446 1.703-1.415 4.882-1.900 7.852.194.002-.632-.15-1.248-.44-1.821C17.194 4.188 13.31 2.188 8.691 2.188z" />
                    </svg>
                  </div>
                  <div className={styles.socialButton} title="QQ登录">
                    <svg width="20" height="20" viewBox="0 0 24 24" fill="currentColor">
                      <path d="M12.017 0C5.396 0 .029 5.367.029 11.987c0 6.62 5.367 11.987 11.988 11.987s11.987-5.367 11.987-11.987C24.004 5.367 18.637.001 12.017.001zM8.449 16.988c-1.161 0-2.101-.94-2.101-2.101s.94-2.101 2.101-2.101 2.101.94 2.101 2.101-.94 2.101-2.101 2.101zm7.138 0c-1.161 0-2.101-.94-2.101-2.101s.94-2.101 2.101-2.101 2.101.94 2.101 2.101-.94 2.101-2.101 2.101z" />
                    </svg>
                  </div>
                  <div className={styles.socialButton} title="GitHub登录">
                    <svg width="20" height="20" viewBox="0 0 24 24" fill="currentColor">
                      <path d="M12 0c-6.626 0-12 5.373-12 12 0 5.302 3.438 9.8 8.207 11.387.599.111.793-.261.793-.577v-2.234c-3.338.726-4.033-1.416-4.033-1.416-.546-1.387-1.333-1.756-1.333-1.756-1.089-.745.083-.729.083-.729 1.205.084 1.839 1.237 1.839 1.237 1.07 1.834 2.807 1.304 3.492.997.107-.775.418-1.305.762-1.604-2.665-.305-5.467-1.334-5.467-5.931 0-1.311.469-2.381 1.236-3.221-.124-.303-.535-1.524.117-3.176 0 0 1.008-.322 3.301 1.23.957-.266 1.983-.399 3.003-.404 1.02.005 2.047.138 3.006.404 2.291-1.552 3.297-1.23 3.297-1.23.653 1.653.242 2.874.118 3.176.77.84 1.235 1.911 1.235 3.221 0 4.609-2.807 5.624-5.479 5.921.43.372.823 1.102.823 2.222v3.293c0 .319.192.694.801.576 4.765-1.589 8.199-6.086 8.199-11.386 0-6.627-5.373-12-12-12z" />
                    </svg>
                  </div>
                </Space>
              </div>
            </div>

            <div className={styles.footer}>
              <Text type="secondary">© 2024 NelsAI. All rights reserved.</Text>
            </div>
          </Card>
        </div>
      </div>
    </App>
  );
};

export default Login;
