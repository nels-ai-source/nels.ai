// @ts-ignore
/* eslint-disable */

declare namespace API {
  type CurrentUser = {
    name?: string;
    avatar?: string;
    id?: string;
    email?: string;
    emailVerified?: boolean;
    userName?: string;
    surName?: string;
    phoneNumber?: string;
    phoneNumberVerified?: boolean;
    tenantId?: string;
    roles?: string[];
    isAuthenticated?: boolean;
    permissions?: string[];
  };

  type LoginResult = {
    access_token?: string;
    token_type?: string;
    expires_in?: int;
  };

  type PageParams = {
    current?: number;
    pageSize?: number;
  };

  type LoginParams = {
    username?: string;
    password?: string;
    client_id?: string;
    grant_type?: string;
    scope?: string;
    client_secret?: string;
  };

  type ErrorResponse = {
    /** 业务约定的错误码 */
    errorCode: string;
    /** 业务上的错误信息 */
    errorMessage?: string;
    /** 业务上的请求是否成功 */
    success?: boolean;
  };


}
