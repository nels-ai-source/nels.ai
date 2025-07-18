import { useState } from "react";
import { Prism as SyntaxHighlighter } from "react-syntax-highlighter";
import { vscDarkPlus } from "react-syntax-highlighter/dist/esm/styles/prism";
import { Button } from "antd";
import { UpOutlined } from "@ant-design/icons";
import { useIntl } from "@umijs/max";
import Copy from "../../common/copy";
import { CodeProps } from "../types";

export const CodeComponent = ({
  inline,
  className,
  children,
  ...props
}: CodeProps) => {
  const [isExpanded, setIsExpanded] = useState(true);
  const intl = useIntl();
  const match = /language-(\w+)/.exec(className || "");
  const code = String(children || "").replace(/\n$/, "");
  const codeLines = code.split('\n');
  const shouldShowToggle = codeLines.length > 8;
  const displayCode = shouldShowToggle && !isExpanded ? codeLines.slice(0, 8).join('\n') : code;


  return !inline && match ? (
    <div className="code-block">
      <div className="code-header">
        <span className="language-label">{match[1]}</span>
        <div className="code-actions">
          <Copy content={code} />
        </div>
      </div>
      <SyntaxHighlighter
        style={vscDarkPlus}
        customStyle={{
          margin: '0px',
          padding: '0px', 
          background: '#2E2E31'
        }}
        language={match[1]}
        PreTag="div"
        showLineNumbers={true}
        wrapLines={true}
        wrapLongLines={true}
        {...props}
      >
        {displayCode}
      </SyntaxHighlighter>
      {shouldShowToggle && (
        <div className="code-toggle-container">
          <Button
            type="text"
            size="small"
            onClick={() => setIsExpanded(!isExpanded)}
            className="code-toggle-button"
          >
            {isExpanded 
              ? intl.formatMessage({ id: 'component.codeBlock.collapse' })
              : intl.formatMessage(
                  { id: 'component.codeBlock.expand' },
                  { count: codeLines.length - 8 }
                )
            }
            <UpOutlined 
              className={`code-toggle-icon ${isExpanded ? 'expanded' : 'collapsed'}`} 
            />
          </Button>
        </div>
      )}
    </div>
  ) : (
    <code className={className} {...props}>
      {children}
    </code>
  );
};