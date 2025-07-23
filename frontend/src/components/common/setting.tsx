import { Dropdown, Tooltip } from "antd";
import { SettingOutlined } from "@ant-design/icons";
import { useIntl } from 'umi';

const Setting = ({
  support,
  tip,
  onClick,
}: {
  support: string[];
  tip: string;
  onClick: (key: string) => void;
}) => {
  const intl = useIntl();

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
          {intl.formatMessage({ id: 'component.setting.edit' })}
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
          {intl.formatMessage({ id: 'component.setting.share' })}
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
          {intl.formatMessage({ id: 'component.setting.unshare' })}
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
          {intl.formatMessage({ id: 'component.setting.delete' })}
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
