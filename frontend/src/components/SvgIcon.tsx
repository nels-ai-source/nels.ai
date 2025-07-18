interface SvgIconProps {
  name: string;
  prefix?: string;
  color?: string;
  size?: number | string;
  className?: string;
  style?: React.CSSProperties;
  onClick?: () => void;
}

const SvgIcon = ({ name, prefix = 'icon', color = 'currentColor', size = '1em', className, style = {}, onClick }: SvgIconProps) => {
  const symbolId = `#${prefix}-${name}`;

  return (
    <svg aria-hidden="true" width={size} height={size} style={{ color, fontSize: size, ...style }} className={className} onClick={onClick}>
      <use href={symbolId} />
    </svg>
  );
};

export default SvgIcon;