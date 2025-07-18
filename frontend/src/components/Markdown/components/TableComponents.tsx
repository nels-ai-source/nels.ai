import { TableProps } from "../types";

export const TableComponent = ({ children, ...props }: TableProps) => {
  return (
    <table className="markdown-table" {...props}>
      {children}
    </table>
  );
};

export const TheadComponent = ({ children, ...props }: TableProps) => {
  return (
    <thead className="" {...props}>
      {children}
    </thead>
  );
};

export const TbodyComponent = ({ children, ...props }: TableProps) => {
  return (
    <tbody className="" {...props}>
      {children}
    </tbody>
  );
};

export const TrComponent = ({ children, ...props }: TableProps) => {
  return (
    <tr className="" {...props}>
      {children}
    </tr>
  );
};

export const ThComponent = ({ children, ...props }: TableProps) => {
  return (
    <th className="" {...props}>
      {children}
    </th>
  );
};

export const TdComponent = ({ children, ...props }: TableProps) => {
  return (
    <td className="" {...props}>
      {children}
    </td>
  );
};