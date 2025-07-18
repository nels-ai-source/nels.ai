import { createContext, useContext } from 'react';

interface MarkdownContextType {
  id?: string;
}

const MarkdownContext = createContext<MarkdownContextType>({});

export const useMarkdownContext = () => useContext(MarkdownContext);
export const MarkdownProvider = MarkdownContext.Provider;