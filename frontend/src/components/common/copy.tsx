import { Button, Tooltip, message } from "antd";
import { CopyOutlined } from "@ant-design/icons";
import copy from "copy-to-clipboard";
import { TooltipPlacement } from "antd/es/tooltip";
import { useIntl } from 'umi';

const Copy = ({ content = "", btnStyle = {}, placement = "bottom", onCopy = () => { } }) => {
  const intl = useIntl();
  const [messageApi, contextHolder] = message.useMessage();
  const handleCopy = () => {
    copy(content, {
      debug: true,
      format: "text/plain",
      onCopy: () => {
        messageApi.open({
          type: "success",
          content: intl.formatMessage({ id: 'component.copy.success' }),
        });
        onCopy?.();
      },
    });
  };
  return (
    <>
      {contextHolder}
      <Tooltip title={intl.formatMessage({ id: 'component.copy.tooltip' })} placement={placement as TooltipPlacement}>
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
