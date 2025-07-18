import { fetchConversationHistory, removeConversation, fetchConversationMessages } from "../../api/history"
import { useState } from "react"
import { ChatData, HistoryTypes, HistoryMessageDetail } from "../../types/index"
import { CHAIN_COMPONENT_STATUS } from "../../constants"
export const useHistory = () => {
  const [history, setHistory] = useState<HistoryTypes>({
    today: [],
    week: [],
    month: []
  });
  const [loading, setLoading] = useState<boolean>(false);
  const getHistory = async () => {
    setLoading(true);
    const res = await fetchConversationHistory();
    setLoading(false);
    setHistory(res);
  };

  const removeHistory = (id: string) => {
    // 遍历并更新每个时间段的历史记录
    const updatedHistory = {
      today: history.today.filter(item => item.id !== id),
      week: history.week.filter(item => item.id !== id),
      month: history.month.filter(item => item.id !== id)
    };
    setHistory(updatedHistory);
    removeConversation(id);
  };

  const getMessages = async (id: string) => {
    const res = await fetchConversationMessages(id);
    const messages: ChatData[] = []
    let botId = "";
    let conversationId = "";
    if (res instanceof Array) {
      res.forEach((item: HistoryMessageDetail) => {
        botId = item.botId;
        conversationId = item.conversationId;
        const message = messages.find((m: ChatData) => item.chatId === m.chatId);
        const type = item.role === 'tool' ? 'action' : 'text';
        if (item.role !== "user") {
          item.role = "assistant";
        }
        let content: Record<string, unknown> = {
          type: type,
          content: item.content,
        }
        if (type === 'action' && item.content) {
          const _value = JSON.parse(item.content);
          const metadata = (_value && typeof _value.metadata === 'string') ? JSON.parse(_value.metadata) : {};
          content.content = {
            title: _value.title ?? '',
            type: type,
            icon: _value.icon ?? '',
            status: 'done',
            metadata: metadata,
          }
        }
        if (message) {
          const last = message.messages[message.messages.length - 1]
          if (last.role === item.role) {
            last.content.push(content)
          } else {
            message.messages.push({
              knowledge: [],
              id: item.id,
              chatId: item.chatId,
              role: item.role,
              status: CHAIN_COMPONENT_STATUS.SUCCESS,
              content: [
                content
              ]
            })
          }
        } else {
          messages.push({
            id: item.chatId,
            chatId: item.chatId,
            botId: item.botId,
            feedback: item.chat?.userFeedback,
            messages: [
              {
                knowledge: [],
                id: item.id,
                chatId: item.chatId,
                role: item.role,
                status: CHAIN_COMPONENT_STATUS.SUCCESS,
                content: [
                  content
                ]
              }
            ]
          })
        }
      });
    }
    return {data: messages, botId, conversationId};
  }
  return {
    history,
    loading,
    getHistory,
    removeHistory,
    getMessages
  }
}