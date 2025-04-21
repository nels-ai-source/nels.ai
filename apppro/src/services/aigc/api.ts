// @ts-ignore
/* eslint-disable */
import { request } from '@umijs/max';

/** login POST /connect/token */
export async function login(body: API.LoginParams, options?: { [key: string]: any }) {
  const formData = new URLSearchParams();
  Object.entries(body).forEach(([key, value]) => {
    formData.append(key, value.toString());
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

/** knowledge.get POST /api/knowledge/get */
export async function getKnowledge(id: string) {
  return request<API.KnowledgeItem>(`/api/knowledge/get?id=${id}`, {
    method: 'POST',
  });
}
/** knowledge.getAllList POST /api/knowledge/getAllList */
export async function getKnowledgeList(
  params: {
    maxResultCount?: number;
    skipCount?: number;
    sorting?: string;
  },
  options?: { [key: string]: any },
) {
  return request<API.KnowledgeItem[]>(`/api/knowledge/getList`, {
    method: 'POST',
    data: {
      ...params,
    },
    ...(options || {}),
  });
}
/** knowledge.create POST /api/knowledge/create */
export async function createKnowledge(options?: { [id: string]: any }) {
  return request<API.KnowledgeItem>(`/api/knowledge/create`, {
    method: 'POST',
    data: {
      ...(options || {}),
    },
  });
}
/** knowledge.update POST /api/knowledge/update */
export async function updateKnowledge(options?: { [id: string]: any }) {
  return request<API.KnowledgeItem>(`/api/knowledge/update?id=${options?.id}`, {
    method: 'POST',
    data: {
      ...(options || {}),
    },
  });
}
/** knowledge.delete POST /api/knowledge/delete */
export async function deleteKnowledge(options?: { [id: string]: any }) {
  return request<Record<string, any>>(`/api/knowledge/delete?id=${options?.id}`, {
    method: 'POST',
  });
}
/** knowledge.deleteMany POST /api/knowledge/deleteMany */
export async function deleteManyKnowledge(options?: [{ [id: string]: any }]) {
  return request<Record<string, any>>(`/api/knowledge/deleteMany`, {
    method: 'POST',
    data: {
      ...(options || {}),
    },
  });
}
/** knowledgeDocument.addKnowledgeDocument POST /api/knowledgeDocument/addKnowledgeDocument */
export async function addKnowledgeDocument(options?: { [id: string]: any }) {
  return request<API.KnowledgeItem>(`/api/knowledgeDocument/addKnowledgeDocument`, {
    method: 'POST',
    data: {
      ...(options || {}),
    },
  });
}

/** 此处后端没有提供注释 GET /api/notices */
export async function getNotices(options?: { [key: string]: any }) {
  return request<API.NoticeIconList>('/api/notices', {
    method: 'GET',
    ...(options || {}),
  });
}

/** 获取规则列表 GET /api/rule */
export async function rule(
  params: {
    // query
    /** 当前的页码 */
    current?: number;
    /** 页面的容量 */
    pageSize?: number;
  },
  options?: { [key: string]: any },
) {
  return request<API.RuleList>('/api/rule', {
    method: 'GET',
    params: {
      ...params,
    },
    ...(options || {}),
  });
}

/** 更新规则 PUT /api/rule */
export async function updateRule(options?: { [key: string]: any }) {
  return request<API.RuleListItem>('/api/rule', {
    method: 'POST',
    data: {
      method: 'update',
      ...(options || {}),
    },
  });
}

/** 新建规则 POST /api/rule */
export async function addRule(options?: { [key: string]: any }) {
  return request<API.RuleListItem>('/api/rule', {
    method: 'POST',
    data: {
      method: 'post',
      ...(options || {}),
    },
  });
}

/** 删除规则 DELETE /api/rule */
export async function removeRule(options?: { [key: string]: any }) {
  return request<Record<string, any>>('/api/rule', {
    method: 'POST',
    data: {
      method: 'delete',
      ...(options || {}),
    },
  });
}
