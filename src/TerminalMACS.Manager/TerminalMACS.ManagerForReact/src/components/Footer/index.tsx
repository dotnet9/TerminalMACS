import React from 'react';
import { GithubOutlined } from '@ant-design/icons';
import { DefaultFooter } from '@ant-design/pro-layout';

export default () => (
  <DefaultFooter
    copyright="2019 蚂蚁金服体验技术部出品"
    links={[
      {
        key: 'Ant Design Pro',
        title: 'Dotnet9',
        href: 'https://dotnet9.com',
        blankTarget: true,
      },
      {
        key: 'github',
        title: <GithubOutlined />,
        href: 'https://github.com/dotnet9',
        blankTarget: true,
      },
      {
        key: 'Ant Design',
        title: 'TerminalMACS',
        href: 'https://github.com/dotnet9/TerminalMACS',
        blankTarget: true,
      },
    ]}
  />
);
