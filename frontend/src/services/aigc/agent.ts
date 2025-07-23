import { request } from '@umijs/max';
import { Agent, AgentFilter, ChatRequest, CancellationToken } from '@/types/agent';
import { XStream } from '@ant-design/x';
import { UUID } from 'crypto';

export async function getAgentList(input: AgentFilter) {
  return request<{
    items: Agent[];
    totalCount: number;
  }>(`/api/agent/getList`, {
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

export function createCancellationToken(): CancellationToken {
  let isCancelled = false;
  const callbacks: (() => void)[] = [];

  return {
    get isCancelled() {
      return isCancelled;
    },
    cancel() {
      if (!isCancelled) {
        isCancelled = true;
        callbacks.forEach(callback => callback());
      }
    },
    onCancelled(callback: () => void) {
      if (isCancelled) {
        callback();
      } else {
        callbacks.push(callback);
      }
    }
  };
}

export async function invokeStreamingAsync(
  conversationId: string,
  agentId: string,
  content: string,
  cancellationToken?: CancellationToken
): Promise<ReadableStream<Uint8Array>> {
  const chatRequest: ChatRequest = {
    agentId: agentId,
    messages: [
      {
        role: 'user',
        content: content
      }]
  };

  const abortController = new AbortController();

  if (cancellationToken) {
    cancellationToken.onCancelled(() => {
      abortController.abort();
    });

    if (cancellationToken.isCancelled) {
      throw new Error('Operation was cancelled');
    }
  }

  const token = localStorage.getItem('access_token');
  const headers = {
    'Content-Type': 'application/json',
    'Accept': 'text/event-stream',
    'Cache-Control': 'no-cache',
    'Authorization': token ? `Bearer ${token}` : '',
  };
  const response = await fetch(`/api/agentActuator/invokeStreaming?conversationId=${conversationId}`, {
    method: 'POST',
    headers: headers,
    body: JSON.stringify(chatRequest),
    signal: abortController.signal
  });

  if (!response.ok) {
    throw new Error(`HTTP error! status: ${response.status}`);
  }

  if (!response.body) {
    throw new Error('Response body is null');
  }

  return response.body;
}

export async function parseSSEStreamWithXStream(
  readableStream: ReadableStream<Uint8Array>,
  cancellationToken?: CancellationToken,
  callbacks?: {
    onChatCreated?: (data: any) => void;
    onMessageDelta?: (data: any) => void;
    onMessageCompleted?: (data: any) => void;
    onChatFailed?: (data: any) => void;
    onDone?: () => void;
    onCancelled?: () => void;
  }
): Promise<void> {
  try {
    for await (const chunk of XStream({
      readableStream,
    })) {
      if (cancellationToken?.isCancelled) {
        callbacks?.onCancelled?.();
        break;
      }
      try {
        switch (chunk.event) {
          case 'conversation.chat.created':
            callbacks?.onChatCreated?.(chunk.data);
            break;
          case 'conversation.message.delta':
            callbacks?.onMessageDelta?.(chunk.data);
            break;
          case 'conversation.message.completed':
            callbacks?.onMessageCompleted?.(chunk.data);
            break;
          case 'conversation.chat.failed':
            callbacks?.onChatFailed?.(chunk.data);
            break;
          case 'done':
            callbacks?.onDone?.();
            return;
        }
      } catch (error) {
        console.error('Error parsing SSE data:', error);
      }
    }


  } catch (error) {
    if (error instanceof Error && error.name === 'AbortError') {
      callbacks?.onCancelled?.();
    } else {
      throw error;
    }
  }
}

export async function invokeStreamingWithXStream(
  conversationId: string,
  agentId: string,
  message: string,
  cancellationToken?: CancellationToken,
  callbacks?: {
    onChatCreated?: (data: any) => void;
    onMessageDelta?: (data: any) => void;
    onMessageCompleted?: (data: any) => void;
    onChatFailed?: (data: any) => void;
    onDone?: () => void;
    onCancelled?: () => void;
  }
): Promise<void> {
  try {
    const stream = await invokeStreamingAsync(conversationId, agentId, message, cancellationToken);

    await parseSSEStreamWithXStream(
      stream,
      cancellationToken,
      callbacks
    );
  } catch (error) {
    if (error instanceof Error && error.name === 'AbortError') {
      callbacks?.onCancelled?.();
    } else {
      throw error;
    }
  }
}