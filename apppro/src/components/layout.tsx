import * as React from 'react';
import Footer from './footer';
import 'antd/dist/reset.css';
import SideBar from './sidebar';
import ContentHeader from './contentheader';
import { ConfigProvider, theme } from 'antd';

const classNames = (...classes: (string | undefined | boolean)[]) => {
  return classes.filter(Boolean).join(' ');
};

type Props = {
  title: string;
  link: string;
  children?: React.ReactNode;
  showHeader?: boolean;
  restricted?: boolean;
  meta?: any;
};

const Layout = ({ meta, link, children, showHeader = true }: Props) => {
  const [isMobileMenuOpen, setIsMobileMenuOpen] = React.useState(false);

  // Close mobile menu on route change
  React.useEffect(() => {
    setIsMobileMenuOpen(false);
  }, [link]);

  React.useEffect(() => {});

  const layoutContent = (
    <div className="min-h-screen flex">
      {/* Desktop sidebar */}
      <div className="hidden md:flex md:flex-col md:fixed md:inset-y-0">
        <SideBar link={link} meta={meta} isMobile={false} />
      </div>

      {/* Content area */}
      <div
        className={classNames(
          'flex-1 flex flex-col min-h-screen',
          'transition-all duration-300 ease-in-out',
          'md:pl-16',
          'md:pl-72',
        )}
      >
        {showHeader && (
          <ContentHeader
            isMobileMenuOpen={isMobileMenuOpen}
            onMobileMenuToggle={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
          />
        )}

        <ConfigProvider
          theme={{
            token: {
              borderRadius: 4,
              colorBgBase: '#05080C',
            },
            algorithm: theme.darkAlgorithm,
          }}
        >
          <main className="flex-1 p-2 text-primary">{children}</main>
        </ConfigProvider>

        <Footer />
      </div>
    </div>
  );

  // Otherwise, render without protection
  return layoutContent;
};

export default Layout;
