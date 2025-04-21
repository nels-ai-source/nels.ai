import openAIIcon from '/public/modelIcons/GPT-3.5_v2.png';
import dashScopeIcon from '/public/modelIcons/qwen_v2.png';
import deepSeekIcon from '/public/modelIcons/deepseek_v2.png';
import kimiIcon from '/public/modelIcons/moonshot_v2.png';

export const modelIcons: { [key: number]: string } = {
  0: openAIIcon,
  1: openAIIcon,
  21: dashScopeIcon,
  22: deepSeekIcon,
  23: kimiIcon,
};

export const modelProvider: { [key: number]: string } = {
  0: 'OpenAI',
  1: 'AzureOpenAI',
  21: 'DashScope',
  22: 'DeepSeek',
  23: 'Kimi',
};

export const modelCapability: { [key: number]: { value: string; labelKey: string; color: string } } = {
  0: { value: 'TextGeneration', labelKey: 'model.capability.textGeneration', color: '#1677ff' },
  1: { value: 'ImageComprehend', labelKey: 'model.capability.imageComprehend', color: '#13c2c2' },
  2: { value: 'AudioComprehend', labelKey: 'model.capability.audioComprehend', color: '#eb2f96' },
  3: { value: 'VideoComprehend', labelKey: 'model.capability.videoComprehend', color: '#722ed1' },
  4: { value: 'Embedding', labelKey: 'model.capability.embedding', color: '#52c41a' },
  51: { value: 'Reasoning', labelKey: 'model.capability.reasoning', color: '#fa8c16' },
  52: { value: 'FunctionCall', labelKey: 'model.capability.functionCall', color: '#a0d911' },
  53: { value: 'JsonOutput', labelKey: 'model.capability.jsonOutput', color: '#52c41a' },
};
