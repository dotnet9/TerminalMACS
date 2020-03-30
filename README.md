# TerminalMACS-多终端管理与检测系统

中文全称：多终端管理与检测系统

英文简称：TerminaMACS，全称：Terminal Management and Check System

本文同步更新地址：
- https://dotnet9.com/11429.html
- https://terminalmacs.com/813.html

本系统使用技术栈较多：C/S(WPF + Prism + log4net, Qt Quick + CTK + log4qt)、B/S(Vue + Element UI + .Net Web API)、移动端（Xamarin.Forms, Flutter、Android原生(Java、Kotlin)）。 

计划开发周期1年以上（2020-03-21开始）。

TerminalMACS思维导图
![TerminalMACS思维导图](docs/imgs/first_workprogress.png)

## 一、本系统可监控多种终端资源：

移动端
- Android
- iOS

PC端
- Windows
- Linux
- Mac

## 二、整个系统分为三类进程：

1. 客户端(Client)

客户端用于获取终端数据，如手机的通讯录、通话记录等，windows终端的文件系统，linux终端的进程列表等，包括：
- 移动客户端（Android、iOS）；
- PC客户端(Windows、Linux、Mac)。

2. 服务端(Server)

用于与客户端和管理端数据中转，包括获取客户端数据、向客户端发送命令、向管理端发送数据等。

3. 管理端(Manager)

用于管理客户端，展示客户端数据、命令交互等，包括：
- 移动管理端（Android、iOS）；
- PC管理端(Windows、Linux、Mac)；
- B/S管理端。

## 三、各子模块技术栈

1. 客户端(Client)
- 移动客户端（Android、iOS）：跨平台版本1（C# + Xamarin.Forms）、跨平台版本2（Dart + Flutter）、Android原生(Java、Kotlin)。
- PC客户端(Windows、Linux、Mac)：C# 控制台。

2. 服务端

.NET 5 WEB API + Entity Framework Core(MySql）+ Redis + RabbitMQ。

3. 管理端
- 移动管理端（Android、iOS）：跨平台版本1（C# + Xamarin.Forms）、跨平台版本2（Dart + Flutter）、Android原生(Java、Kotlin)。
- PC管理端(Windows)：C# + WPF。
- PC管理端(Windows、Linux、Mac)：C++ + Qt Quick。

## 四、开发进度
2020-03-21
项目才计划不久，先开发移动客户端基本功能：
1. 获取通讯录数据
参考文章：[Getting phone contacts in Xamarin Forms](https://www.xamboy.com/2019/10/10/getting-phone-contacts-in-xamarin-forms/)

参考上面的文章，Android获取通讯录功能已经完成大部分（获取所有通讯录信息并展示），iOS由于没有相关环境，iOS代码是已经添加了，只是未测试，有条件的同学可以帮忙测试下。

下面是部分截图，稍作整理，出篇文章介绍获取通讯录功能代码，大部分是参考上面的文章。
![关于移动客户端](docs/imgs/About_MobileClient.jpg)

![通讯录-移动客户端](docs/imgs/Contact_MobileClient.jpg)

本项目源码已经开源：https://github.com/dotnet9/TerminalMACS