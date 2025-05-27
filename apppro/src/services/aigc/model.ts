import type { Model, ModelFilter, ModelSetKey } from '@/types/model';
import { request } from '@umijs/max';
import { UUID } from 'crypto';

export async function getModelList(input: ModelFilter) {
  return request<Model[]>(`/api/model/getAllList`, {
    method: 'POST',
    data: {
      ...input,
    },
  });
}
export async function createModeList(data: Model[]) {
  return request<void>(`/api/model/createList`, {
    method: 'POST',
    data: data,
  });
}
export async function updateModel(data: Model) {
  return request<void>(`/api/model/update?id=${data.id}`, {
    method: 'POST',
    data: data,
  });
}
export async function deleteModel(id: UUID) {
  return request<void>(`/api/model/delete?id=${id}`, {
    method: 'POST',
  });
}
export async function setIsEnabled(id: UUID, isEnabled: boolean) {
  return request<void>(`/api/model/setIsEnabled?id=${id}&isEnabled=${isEnabled}`, {
    method: 'POST',
  });
}
export async function setKey(data: ModelSetKey) {
  return request<Model>(`/api/model/setKey`, {
    method: 'POST',
    data: data,
  });
}
