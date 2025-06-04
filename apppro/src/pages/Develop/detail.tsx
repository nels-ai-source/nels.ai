import Independent from '@/components/Chat/index';
import { Agent } from '@/types/agent';
import type { MenuProps } from 'antd';
import { Button, Skeleton } from 'antd';
import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { Header } from './detail/header';
import { PromptEditor } from './detail/prompt';
import { Sidebar } from './detail/sidebar/sidebar';

export function AgentDetail() {
  const [agent, setAgent] = useState<Agent | null>(null);
  const [loading, setLoading] = useState(false);
  const { id } = useParams<{ id: string }>();

  useEffect(() => {
    const fetchAgent = async () => {
      setAgent({
        name: 'test',
        instructions: 'test',
        description: 'test',
        knowledge: [],
      } as unknown as Agent);
      setLoading(false);
    };

    if (id) {
      fetchAgent();
    }
  }, [id]);

  const handleAgentUpdate = (updates: Partial<Agent>) => {
    if (agent) {
      setAgent({ ...agent, ...updates });
    }
  };

  const handleMenuClick: MenuProps['onClick'] = (e) => {
    console.log('click', e);
  };

  return loading ? (
    <div className="flex items-center justify-center text-secondary" data-oid="ppma2aw">
      <Skeleton active data-oid="5gq4f._" />
    </div>
  ) : (
    <div className="flex flex-col h-screen " data-oid="7jv9:fq">
      {/* Header */}
      <Header
        onMenuClick={handleMenuClick}
        agent={agent}
        onPublish={() => {
          console.log(agent);
        }}
        data-oid="pcz3uv0"
      />

      {/* Main Content*/}
      <main className="overflow-auto flex flex-row" data-oid="cq1z2bh">
        <aside
          className="flex flex-col bg-white border-gray-200 shadow-sm"
          style={{ width: '65%' }}
          data-oid="joem8-u"
        >
          <header
            className="px-2 h-32 flex items-center justify-between"
            style={{ borderBottom: '1px solid #E5E7EB' }}
            data-oid="lrm_8uh"
          >
            <div className="flex items-center space-x-2" data-oid="g2mz6e8">
              编排
            </div>
            <div className="flex items-center space-x-3" data-oid="r-u-ebs">
              <Button type="text" size="small" title="模型" data-oid="pt_he--">
                模型
              </Button>
            </div>
          </header>

          <main className="flex flex-row flex-1" data-oid="71.8:.8">
            <aside
              className="flex flex-col p-2 overflow-auto whitespace-pre-wrap break-words"
              style={{ width: '50%' }}
              data-oid="8y-elxf"
            >
              <PromptEditor agent={agent} onChange={handleAgentUpdate} data-oid="rqfo9bc" />
            </aside>

            <div style={{ width: '50%', borderLeft: '1px solid #E5E7EB' }} data-oid="00__qt4">
              {' '}
              {agent && <Sidebar agent={agent} onChange={handleAgentUpdate} data-oid="9xzo-_e" />}
            </div>
          </main>
        </aside>

        {/* Right Sidebar */}
        <main className="p-4" style={{ width: '35%' }} data-oid=":6hrgfm">
          {/* Right Sidebar Content */}
          <Independent data-oid="85eadu."></Independent>
        </main>
      </main>
    </div>
  );
}

export default AgentDetail;
