import { Button, Tooltip, message } from "antd";
import { CopyOutlined } from "@ant-design/icons";
import copy from "copy-to-clipboard";
import { TooltipPlacement } from "antd/es/tooltip";

const Copy = ({ content = "", btnStyle = {}, placement = "bottom", onCopy = () => { } }) => {
  const [messageApi, contextHolder] = message.useMessage();
  const handleCopy = () => {
    copy(content, {
      debug: true,
      format: "text/plain",
      onCopy: () => {
        messageApi.open({
          type: "success",
          content: "复制成功",
        });
        onCopy?.();
      },
    });
  };
  return (
    <>
      {contextHolder}
      <Tooltip title="复制" placement={placement as TooltipPlacement}>
        <Button
          type="text"
          style={btnStyle}
          icon={<CopyOutlined style={{ color: "#c5c5c5", fontSize: "16px" }} />}
          onClick={handleCopy}
        />
      </Tooltip>
    </>
  );
};

export default Copy;
