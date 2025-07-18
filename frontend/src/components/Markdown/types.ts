export interface MarkdownProps {
  content: string;
  id?: string;
}

export interface CodeProps {
  inline?: boolean;
  className?: string;
  children: React.ReactNode;
  [key: string]: any;
}

export interface ImageProps {
  src?: string;
  alt?: string;
  title?: string;
  [key: string]: any;
}

export interface LinkProps {
  href?: string;
  children: React.ReactNode;
  [key: string]: any;
}

export interface TableProps {
  children: React.ReactNode;
  [key: string]: any;
}