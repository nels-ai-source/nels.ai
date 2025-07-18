import { message } from "antd";
import copy from "copy-to-clipboard";
import { LinkProps } from "../types";

export const LinkComponent = ({ href, children }: LinkProps) => {
  const [messageApi, contextHolder] = message.useMessage();
  const handleCopyLink = async () => {
    if (href) {
      copy(href, {
        debug: true,
        format: "text/plain",
        onCopy: () => {
          messageApi.open({
            type: "success",
            content: "Link copied successfully, open in browser.",
          });
        },
      });
    }
  };

  return (
    <>
      {contextHolder}
      <span
        onClick={handleCopyLink}
        className="markdown-link"
        title="Click to copy link"
      >
        {children}
      </span>
    </>
  );
};