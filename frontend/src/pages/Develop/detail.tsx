import Independent from '@/components/Chat/index';
import { Agent } from '@/types/agent';
import { getAgent, updateAgent } from '@/services/aigc/agent'
import { message, Button, Skeleton } from 'antd';
import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { Header } from './detail/header';
import { PromptEditor } from './detail/prompt';
import { Sidebar } from './detail/sidebar/sidebar';
import { UUID } from 'crypto';
import { useIntl } from '@umijs/max';
import { error } from 'console';
export function AgentDetail() {
  const [agent, setAgent] = useState<Agent>();
  const [loading, setLoading] = useState(false);
  const { id } = useParams<{ id: UUID }>();
  const intl = useIntl();
  const handleGetAgent = async (id: UUID) => {
    if (!id) return;
    setLoading(false);
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
  const handleUpdateAgent = (agent: Agent) => {
    try {
      setLoading(true);
      updateAgent(agent);
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
    <div className="flex flex-col h-screen ">
      {/* Header */}
      <Header
        agent={agent}
        onChange={(updates) => {
          handleUpdatePartial(updates);
        }}
        onSave={() => { handleUpdateAgent(agent as Agent) }}
      />

      {/* Main Content*/}
      <main className="overflow-auto flex flex-row">
        <aside
          className="flex flex-col bg-white border-gray-200 shadow-sm"
          style={{ width: '65%' }}

        >
          <header
            className="px-2 h-32 flex items-center justify-between"
            style={{ borderBottom: '1px solid #E5E7EB' }}

          >
            <div className="flex items-center space-x-2">
              编排
            </div>
            <div className="flex items-center space-x-3">
              <Button type="text" size="small" title="模型">
                模型
              </Button>
            </div>
          </header>

          <main className="flex flex-row flex-1">
            <aside
              className="flex flex-col p-2 overflow-auto whitespace-pre-wrap break-words"
              style={{ width: '50%' }}

            >
              <PromptEditor agent={agent} onChange={handleUpdatePartial} />
            </aside>

            <div style={{ width: '50%', borderLeft: '1px solid #E5E7EB' }}>
              {' '}
              {agent && <Sidebar agent={agent} onChange={handleUpdatePartial} />}
            </div>
          </main>
        </aside>

        {/* Right Sidebar */}
        <main className="p-4" style={{ width: '35%' }}>
          {/* Right Sidebar Content */}
          <Independent agentData={agent}></Independent>
        </main>
      </main>
    </div>
  );
}

export default AgentDetail;
