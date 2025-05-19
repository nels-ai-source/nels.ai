import React from 'react';
import { Button, Input } from 'antd';
import { FormOutlined } from '@ant-design/icons';
import { Sparkles } from 'lucide-react';
import { theme } from 'antd';
import { Bot } from '../../../types/bot';
import { MdEditor } from 'md-editor-rt';
import 'md-editor-rt/lib/style.css';

interface PromptEditorProps {
    bot: Bot | null;
    onChange: (updates: Partial<Bot>) => void;
}

export function PromptEditor({ bot, onChange }: PromptEditorProps) {
    const { token } = theme.useToken();
    return (
        <div className="flex flex-col h-full">
            <header className="flex items-center justify-between px-2">
                <div className="flex items-center text-[14px] font-semibold space-x-2">
                    人设与回复逻辑
                </div>
                <div className="flex items-center space-x-3">
                    <Button
                        type="text"
                        size="small"
                        title="自动优化提示词"
                        icon={<Sparkles color={token.colorPrimary} size={16} />}
                    />
                </div>
            </header>
            <MdEditor
                value={bot?.instructions || ''}
                onChange={(value) => {
                    onChange({ instructions: value } as Bot);
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
