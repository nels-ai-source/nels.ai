import { request } from '@umijs/max';

/** login POST /connect/token */
export async function login(body: API.LoginParams, options?: { [key: string]: any }) {
  const formData = new URLSearchParams();
  Object.entries(body).forEach(([key, value]) => {
    if (value !== undefined && value !== null) {
      formData.append(key, value.toString());
    }
  });

  return request<API.LoginResult>('/connect/token', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/x-www-form-urlencoded',
    },
    data: formData.toString(),
    ...(options || {}),
  });
}

/** getConfiguration POST /api/app/getConfiguration */
export async function getConfiguration(options?: { [key: string]: any }) {
  return request<{
    userInfo: API.CurrentUser;
    permissions: string[];
  }>('/api/app/getConfiguration', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    ...(options || {}),
  });
}

/** logout POST /api/account/logout */
export async function logout(options?: { [key: string]: any }) {
  return request<Record<string, any>>('/api/account/logout', {
    method: 'GET',
    ...(options || {}),
  });
}
