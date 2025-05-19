import * as React from 'react';

const Footer = () => {
  React.useEffect(() => {}, []);
  return (
    <div className=" text-primary p-3  border-t border-secondary flex ">
      <div className="text-xs flex-1">
        Maintained by the AutoGen{' '}
        <a
          target={'_blank'}
          rel={'noopener noreferrer'}
          className="underlipne inline-block border-accent border-b hover:text-accent"
          href="https://microsoft.github.io/autogen/"
        >
          {' '}
          Team.
        </a>
      </div>
    </div>
  );
};
export default Footer;
