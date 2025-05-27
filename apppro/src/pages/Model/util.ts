import { ModelProvider } from '../../types/model';

/**
 * Get enum label
 * @param enumObj Enum object
 * @param enumName Enum name
 * @param value Enum value
 * @returns Label text object with id
 */
export const getEnumLabel = <T extends Record<string, number | string>>(
  enumObj: T,
  enumName: string,
  value: number,
): { id: string } => {
  const enumKey = Object.entries(enumObj)
    .filter(([key]) => isNaN(Number(key)))
    .find(([, val]) => val === value)?.[0];

  if (!enumKey) {
    return { id: `${enumName}.unknown` };
  }

  return {
    id: `${enumName}.${enumKey}`,
  };
};

/**
 * Get model provider icon
 * @param provider Provider enum value
 * @returns Icon path
 * @throws {Error} When provider is invalid
 */
export const getProviderIcon = (provider: ModelProvider): string => {
  if (!(provider in ModelProvider)) {
    throw new Error(`Invalid provider: ${provider}`);
  }

  return `/images/model/${ModelProvider[provider].toLowerCase()}.svg`;
};
