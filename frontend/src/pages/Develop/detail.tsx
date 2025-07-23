import Independent from '@/components/Chat/index';
import { Agent } from '@/types/agent';
import { getAgent, updateAgent } from '@/services/aigc/agent'
import { message, Skeleton } from 'antd';
import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { Header } from './detail/header';
import { PromptEditor } from './detail/prompt';
import { Sidebar } from './detail/sidebar/sidebar';
import { UUID } from 'crypto';
import { useIntl } from '@umijs/max';
import './agent.css';

export function AgentDetail() {
  const [agent, setAgent] = useState<Agent>({} as Agent);
  const [loading, setLoading] = useState(false);
  const { id } = useParams<{ id: UUID }>();
  const intl = useIntl();

  const handleGetAgent = async (id: UUID) => {
    if (!id) return;
    setLoading(true);
    try {
      const res = await getAgent(id);
      setAgent(res);
    } catch (error) {
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (id) {
      handleGetAgent(id);
    }
  }, [id]);

  const handleUpdatePartial = (updates: Partial<Agent>) => {
    if (agent) {
      setAgent({ ...agent, ...updates });
    }
  };

  const handleUpdateAgent = async (agent: Agent) => {
    try {
      setLoading(true);
      await updateAgent(agent);
      message.success(intl.formatMessage({ id: 'actions.success' }));
    } catch (error) {
      message.error(intl.formatMessage({ id: 'actions.failed' }));
    } finally {
      setLoading(false);
    }
  };

  return loading ? (
    <div className="flex items-center justify-center text-secondary">
      <Skeleton active />
    </div>
  ) : (
    <div className="flex flex-col h-screen">
      {/* Header */}
      <Header
        agent={agent}
        onChange={(updates) => {
          handleUpdatePartial(updates);
        }}
        onSave={() => { handleUpdateAgent(agent) }}
      />

      {/* Main Content*/}
      {agent.id &&
        <main className="overflow-auto flex flex-row">
          <aside className="flex flex-col bg-white border-gray-200 shadow-sm agent-aside">
            <div className="flex flex-col flex-1 overflow-auto">
              <PromptEditor agent={agent} onChange={handleUpdatePartial} />
            </div>
          </aside>
          <aside className="flex flex-col bg-white border-gray-200 shadow-sm border-l border-r agent-aside">
            <div className="flex flex-col flex-1 overflow-auto">
              <Sidebar agent={agent} onChange={handleUpdatePartial} />
            </div>
          </aside>
          <main className="p-4 agent-main">
            <header className="flex items-center justify-between px-2 agent-header">
              <div className="flex items-center font-semibold space-x-2 agent-title">
                {intl.formatMessage({ id: 'agent.detail.previewAndDebug' })}
              </div>
              <div className="flex items-center space-x-3">

              </div>
            </header>
            <Independent agentData={agent}></Independent>
          </main>
        </main>
      }
    </div>
  );
}

export default AgentDetail;
