import { unified } from "unified";
import remarkParse from "remark-parse";
import remarkGfm from "remark-gfm";
import remarkRehype from "remark-rehype";
import rehypeReact from "rehype-react";
import { useMemo } from "react";
import * as runtime from "react/jsx-runtime";
import {
  CodeComponent,
  ImageComponent,
  LinkComponent,
  TableComponent,
  TheadComponent,
  TbodyComponent,
  TrComponent,
  ThComponent,
  TdComponent
} from './components';
import { MarkdownProps } from './types';
import { MarkdownProvider } from './context';

const components = {
  table: TableComponent,
  thead: TheadComponent,
  tbody: TbodyComponent,
  tr: TrComponent,
  th: ThComponent,
  td: TdComponent,
  code: CodeComponent,
  img: ImageComponent,
  a: LinkComponent,
};

const Markdown = ({ content, id }: MarkdownProps) => {

  const processor = useMemo(() => {
    return unified()
      .use(remarkParse)
      .use(remarkGfm)
      .use(remarkRehype)
      .use(rehypeReact, {
        jsx: runtime.jsx,
        jsxs: runtime.jsxs,
        Fragment: runtime.Fragment,
        components,
      } as any);
  }, []);

  const result = useMemo(() => {
    return processor.processSync(content).result;
  }, [content, processor]);

  return (
    <MarkdownProvider value={{ id }}>
      <div className="markdown-body">{result}</div>
    </MarkdownProvider>
  );
};

export default Markdown;