import { useXAgent, XRequest } from '@ant-design/x';
import type { BubbleDataType } from '@ant-design/x/es/bubble/BubbleList';
import {
    ChatRequest,
    ChatEventCallbacks,
    ChatCreatedContent,
    ChatInProgressContent,
    MessageContent,
    ChatFailedContent
} from '@/types/agent';

export function useInvokeStreamingAsync() {
    const BASE_URL = '/api/agentActuator/invokeStreaming';
    const dangerouslyApiKey = 'Bearer ' + localStorage.getItem('access_token');
    const [agent] = useXAgent<BubbleDataType>({
        baseURL: BASE_URL,
        dangerouslyApiKey: dangerouslyApiKey,
    });

    const request = XRequest({
        baseURL: BASE_URL,
        dangerouslyApiKey: dangerouslyApiKey,
    });

    const parseSSEData = <T>(data: string | any): T | null => {
        try {
            return typeof data === 'string' ? JSON.parse(data) : data;
        } catch (error) {
            console.error('Failed to parse SSE data:', error);
            return null;
        }
    };

    const invokeChat = async (chatRequest: ChatRequest, callbacks: ChatEventCallbacks) => {
        try {
            await request.create(
                chatRequest,
                {
                    onSuccess: (messages) => {
                        callbacks.onSuccess?.(messages);
                    },
                    onError: (error) => {
                        callbacks.onError?.(error);
                    },
                    onUpdate: (msg) => {
                        switch (msg.event) {
                            case 'conversation.chat.created': {
                                const data = parseSSEData<ChatCreatedContent>(msg.data);
                                if (data) {
                                    callbacks.onChatCreated?.(data);
                                }
                                break;
                            }

                            case 'conversation.chat.in_progress': {
                                const data = parseSSEData<ChatInProgressContent>(msg.data);
                                if (data) {
                                    callbacks.onChatInProgress?.(data);
                                }
                                break;
                            }

                            case 'conversation.message.delta': {
                                const data = parseSSEData<MessageContent>(msg.data);
                                if (data) {
                                    callbacks.onMessageDelta?.(data);
                                }
                                break;
                            }

                            case 'conversation.message.completed': {
                                const data = parseSSEData<MessageContent>(msg.data);
                                if (data) {
                                    callbacks.onMessageCompleted?.(data);
                                }
                                break;
                            }

                            case 'conversation.chat.failed': {
                                const data = parseSSEData<ChatFailedContent>(msg.data);
                                if (data) {
                                    callbacks.onChatFailed?.(data);
                                }
                                break;
                            }

                            default:
                                console.warn('Unknown SSE event type:', msg.event);
                                break;
                        }
                    },
                },
            );
        } catch (error) {
            callbacks.onError?.(error);
        }
    };

    return {
        agent,
        request,
        invokeChat
    };
}