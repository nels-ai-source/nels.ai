import { Image } from "antd";
import { ImageProps } from "../types";

export const ImageComponent = ({ src, alt, title }: ImageProps) => {
  return <Image src={src} alt={alt} title={title} />;
};