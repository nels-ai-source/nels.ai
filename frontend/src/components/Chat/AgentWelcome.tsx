import React from 'react';
import { createStyles } from 'antd-style';
import Markdown from '../Markdown';
import { Prompts, Bubble } from '@ant-design/x';
import { Agent } from '@/types/agent';

const useStyle = createStyles(({ }) => ({
  container: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    justifyContent: 'center',
    width: '100%',
    overflow: 'hidden',
    maxHeight: 'calc(100vh - 200px)',
  },
  iconContainer: {
    marginBottom: '12px',
  },
  icon: {
    width: '80px',
    height: '80px',
    borderRadius: '8px',
    objectFit: 'cover',
  },
  title: {
    fontSize: '22px',
    fontWeight: 'bold',
    marginBottom: '12px',
    textAlign: 'center',
  },
  description: {
    fontSize: '14px',
    marginBottom: '12px',
    width: '100%',
  },
  promptsContainer: {
    width: '100%',
    margin: '0 auto',
    overflow: 'hidden',
    maxHeight: '40vh',
  },
}));

interface AgentWelcomeProps {
  agent: Agent;
  onPromptClick?: (text: string) => void;
}

export const AgentWelcome: React.FC<AgentWelcomeProps> = ({ agent, onPromptClick }) => {
  const { styles } = useStyle();

  return (
    <div className={styles.container}>
      <div className={styles.iconContainer}>
        <img src={agent.icon} className={styles.icon} alt={agent.name} />
      </div>
      <div className={styles.title}>{agent.name}</div>
      <div className={styles.description}>
        {agent.prologue && <Bubble content={<Markdown content={agent.prologue} id={'prologue'} />} placement="start"/>}
      </div>
      <div className={styles.promptsContainer}>
        <div style={{ maxHeight: '100%', overflowY: 'auto' }}>
          <Prompts
            vertical
            items={agent.questions.map((question) => ({
              label: question.content,
              value: question.content,
              key: question.id,
            }))}
            onItemClick={(item) => onPromptClick?.(item.data.label as string)}
          />
        </div>
      </div>
    </div>
  );
};