import { request } from '@umijs/max';

export async function getModelList(
  params: {
    maxResultCount?: number;
    skipCount?: number;
    sorting?: string;
  },
  options?: { [key: string]: any },
) {
  return request<{
    items: API.ModelItem[];
    totalCount: number;
  }>(`/api/model/getList`, {
    method: 'POST',
    data: {
      ...params,
    },
    ...(options || {}),
  });
}

export async function modelSetting(data: API.ModelSettingDto) {
  return request(`/api/model/modelSetting`, {
    method: 'POST',
    data,
  });
}
