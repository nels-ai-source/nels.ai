import { Knowledge, KnowledgeDocument, KnowledgeDocumentParagraph } from '@/types/knowledge';
import { request } from '@umijs/max';

export async function getKnowledge(id: string) {
  return request<Knowledge>(`/api/knowledge/get?id=${id}`, {
    method: 'POST',
  });
}

export async function getKnowledgeList(
  params: {
    maxResultCount?: number;
    skipCount?: number;
    sorting?: string;
  },
  options?: { [key: string]: any },
) {
  return request<{
    items: Knowledge[];
    totalCount: number;
  }>(`/api/knowledge/getList`, {
    method: 'POST',
    data: {
      ...params,
    },
    ...(options || {}),
  });
}

export async function createKnowledge(options?: { [id: string]: any }) {
  return request<Knowledge>(`/api/knowledge/create`, {
    method: 'POST',
    data: {
      ...(options || {}),
    },
  });
}
export async function updateKnowledge(options?: { [id: string]: any }) {
  return request<Knowledge>(`/api/knowledge/update?id=${options?.id}`, {
    method: 'POST',
    data: {
      ...(options || {}),
    },
  });
}
export async function deleteKnowledge(options?: { [id: string]: any }) {
  return request<Record<string, any>>(`/api/knowledge/delete?id=${options?.id}`, {
    method: 'POST',
  });
}

export async function deleteManyKnowledge(options?: [{ [id: string]: any }]) {
  return request<Record<string, any>>(`/api/knowledge/deleteMany`, {
    method: 'POST',
    data: {
      ...(options || {}),
    },
  });
}

export async function getKnowledgeDocumentList(knowledgeId: string) {
  return request<[KnowledgeDocument]>(`/api/knowledgeDocument/getList?knowledgeId=${knowledgeId}`, {
    method: 'POST',
  });
}
export async function getParagraphList(knowledgeDocumentId: string) {
  return request<KnowledgeDocumentParagraph[]>(
    `/api/knowledgeDocument/getParagraphList?knowledgeDocumentId=${knowledgeDocumentId}`,
    {
      method: 'POST',
    },
  );
}
export async function addKnowledgeDocument(options?: { [id: string]: any }) {
  return request<API.KnowledgeItem>(`/api/knowledgeDocument/create`, {
    method: 'POST',
    data: {
      ...(options || {}),
    },
  });
}
export async function updateKnowledgeDocument(knowledgeDocumentId: string, name: string) {
  return request(
    `/api/knowledgeDocument/update?knowledgeDocumentId=${knowledgeDocumentId}&name=${name}`,
    {
      method: 'POST',
    },
  );
}
export async function deleteKnowledgeDocument(knowledgeDocumentId: string) {
  return request(`/api/knowledgeDocument/delete?knowledgeDocumentId=${knowledgeDocumentId}`, {
    method: 'POST',
  });
}
