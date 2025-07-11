import { request } from '@umijs/max';
import { Agent,AgentFilter } from '@/types/agent';
import { UUID } from 'crypto';

export async function getAgentList(input: AgentFilter) {
  return request<Agent[]>(`/api/agent/getAllList`, {
    method: 'POST',
    data: {
      ...input,
    },
  });
}
export async function getAgent(id: UUID) {
  return request<Agent>(`/api/agent/get?id=${id}`, {
    method: 'POST',
  });
}
export async function createAgent(data: Agent) {
  return request<void>(`/api/agent/create`, {
    method: 'POST',
    data: data,
  });
}
export async function updateAgent(data: Agent) {
  return request<void>(`/api/agent/update?id=${data.id}`, {
    method: 'POST',
    data: data,
  });
}
export async function deleteAgent(id: UUID) {
  return request<void>(`/api/agent/delete?id=${id}`, {
    method: 'POST',
  });
}