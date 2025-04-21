import { request } from '@umijs/max';

export async function getAgentList(
  params: {
    maxResultCount?: number;
    skipCount?: number;
    sorting?: string;
  },
  options?: { [key: string]: any },
) {
  return request<{
    items: API.Agent[];
    totalCount: number;
  }>(`/api/agent/getList`, {
    method: 'POST',
    data: {
      ...params,
    },
    ...(options || {}),
  });
}
