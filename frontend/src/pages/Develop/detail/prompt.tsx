import React, { useState } from 'react';
import { Button, theme } from 'antd';
import { Sparkles } from 'lucide-react';
import 'md-editor-rt/lib/style.css';
import { Agent } from '@/types/agent';
import { useIntl } from '@umijs/max';
import { PromptEditorWithInputs, IFlowValue } from '@flowgram.ai/form-materials';
import { StandaloneScopeProvider } from '@/components/common/standaloneScopeProvider'

interface PromptEditorProps {
  agent: Agent;
  onChange: (updates: Partial<Agent>) => void;
}

export function PromptEditor({ agent, onChange }: PromptEditorProps) {
  const { token } = theme.useToken();
  const intl = useIntl();

  const [value, setValue] = useState<{ type: 'template'; content: string }>({
    type: 'template',
    content: agent.instructions
  });

  const inputsValues: Record<string, IFlowValue> = {
    name: { type: 'constant', content: intl.formatMessage({ id: 'agent.detail.exampleName' }) },
    location: { type: 'constant', content: intl.formatMessage({ id: 'agent.detail.exampleLocation' }) },
  };

  const handleChange = (newValue?: IFlowValue) => {
    if (newValue && newValue.type === 'template') {
      setValue(newValue as { type: 'template'; content: string });
      if (onChange) {
        onChange({ instructions: newValue.content });
      }
    }
  };

  return (
    <div className="flex flex-col h-full">
      <header className="flex items-center justify-between px-2" style={{ height: '32px', padding: '12px 0 0 0' }}>
        <div className="flex items-center font-semibold space-x-2" style={{ fontSize: '16px', fontWeight: 600 }}>
          {intl.formatMessage({ id: 'agent.detail.promptTitle' })}
        </div>
        <div className="flex items-center space-x-3">
          <Button
            type="text"
            size="small"
            title={intl.formatMessage({ id: 'agent.detail.optimizePrompt' })}
            icon={<Sparkles color={token.colorPrimary} size={16} />}
            onClick={() => {
              console.log(intl.formatMessage({ id: 'agent.detail.optimizePromptLog' }));
            }}
          />
        </div>
      </header>

      <StandaloneScopeProvider
        scopeId="prompt-editor-scope"
        initialData={{
          type: 'object',
          properties: inputsValues
        }}
      >
        <PromptEditorWithInputs
          value={value}
          onChange={handleChange}
          inputsValues={inputsValues}
          placeholder={intl.formatMessage({ id: 'agent.detail.promptPlaceholder' })}
          activeLinePlaceholder={intl.formatMessage({ id: 'agent.detail.activeLinePlaceholder' })}
          style={{
            height: 'calc(100vh - 100px)',
            border: 'none',
            padding: '12px 0 0',
            backgroundColor: 'transparent',
          }}
        />
      </StandaloneScopeProvider>
    </div>
  );
}

export default PromptEditor;
