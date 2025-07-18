import { Button, theme } from 'antd';
import { Sparkles } from 'lucide-react';
import { MdEditor } from 'md-editor-rt';
import 'md-editor-rt/lib/style.css';
import { Agent } from '@/types/agent';
import { useIntl } from '@umijs/max';

interface PromptEditorProps {
  agent: Agent | null;
  onChange: (updates: Partial<Agent>) => void;
}

export function PromptEditor({ agent, onChange }: PromptEditorProps) {
  const { token } = theme.useToken();
  const intl = useIntl();
  return (
    <div className="flex flex-col h-full">
      <header className="flex items-center justify-between px-2">
        <div className="flex items-center text-[14px] font-semibold space-x-2">{intl.formatMessage({ id: 'agent.detail.promptTitle' })}</div>
        <div className="flex items-center space-x-3">
          <Button
            type="text"
            size="small"
            title={intl.formatMessage({ id: 'agent.detail.optimizePrompt' })}
            icon={<Sparkles color={token.colorPrimary} size={16} />}
          />
        </div>
      </header>
      <MdEditor
        value={agent?.instructions || ''}
        onChange={(value) => {
          onChange({ instructions: value } as Agent);
        }}
        preview={false}
        toolbars={[]}
        footers={[]}
        style={{
          height: 'calc(100vh - 140px)',
          border: 'none',
        }}
      />
    </div>
  );
}

export default PromptEditor;
