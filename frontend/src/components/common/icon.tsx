const Icon = ({ index, name, styles = {}, size }: { index: number; name: string; styles?: React.CSSProperties; size?: number }) => {
  const botIconColors = [
    "#619df5",
    "#967ef5",
    "#dccf56",
    "#8d93a6",
    "#619df5",
    "#a8de63",
    "#d192f9",
  ];
  const len = botIconColors.length;
  return (
    <span
      style={{
        ...styles,
        background: botIconColors[index % len],
        width: size,
        height: size, 
        minWidth: size, 
        minHeight: size, 
        maxWidth: size, 
        maxHeight: size,
      }}
      className="agent-icon text-[16px] bg-[#fff] items-center justify-center flex rounded-[15px] text-[#fff] font-semibold"
    >
      {name?.slice(0, 1)}
    </span>
  );
};
export default Icon;