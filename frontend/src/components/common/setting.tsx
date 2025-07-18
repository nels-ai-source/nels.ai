import { Dropdown, Tooltip } from "antd";
import { SettingOutlined } from "@ant-design/icons";

const Setting = ({
  support,
  tip,
  onClick,
}: {
  support: string[];
  tip: string;
  onClick: (key: string) => void;
}) => {
  if (!support?.length) {
    return null;
  }
  const items = [
    {
      key: "edit",
      label: (
        <span
          className="inline-block w-full h-full px-[12px] py-[2px]"
          onClick={() => onClick("edit")}
        >
          编辑
        </span>
      ),
    },
    {
      key: "share",
      label: (
        <span
          className="inline-block w-full h-full px-[12px] py-[2px]"
          onClick={() => onClick("share")}
        >
          分享
        </span>
      ),
    },
    {
      key: "unshare",
      label: (
        <span
          className="inline-block w-full h-full px-[12px] py-[2px]"
          onClick={() => onClick("unshare")}
        >
          取消分享
        </span>
      ),
    },
    {
      key: "del",
      label: (
        <span
          className="inline-block w-full h-full px-[12px] py-[2px]"
          onClick={() => onClick("del")}
        >
          删除
        </span>
      ),
    },
  ];
  return (
    <Dropdown
      trigger={["click"]}
      menu={{ items: items.filter((item) => support.includes(item.key)) }}
      placement="bottomRight"
      arrow
      overlayStyle={{ marginTop: "-13px" }}
    >
      <Tooltip title={tip} placement="left">
        <div className="cursor-pointer px-[6px] py-[0] inline">
          <SettingOutlined className="text-[16px]" />
        </div>
      </Tooltip>
    </Dropdown>
  );
};

export default Setting;
