using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace TerminalMACS.Clients.App.ViewModels
{
    /*************************************************************************************
	 * CLR版本：       4.0.30319.42000
	 * 类 名 称：       ClientInfoViewModel
	 * 机器名称：       DESKTOP-OHIDTJ6
	 * 命名空间：       TerminalMACS.Clients.App.ViewModels
	 * 文 件 名：       ClientInfoViewModel
	 * 创建时间：       2020/3/31 21:17:26
	 * 作    者：       TerminalMACS
	 * 说   明：		手机基本信息VM
	 * 修改时间：
	 * 修 改 人：
	*************************************************************************************/
    public class ClientInfoViewModel : BaseViewModel
    {
        /// <summary>
        /// 获取或者设置 设备型号
        /// </summary>
        public string Model { get; private set; } = DeviceInfo.Model;
        /// <summary>
        /// 获取或者设置 设备厂商
        /// </summary>
        public string Manufacturer { get; private set; } = DeviceInfo.Manufacturer;
        /// <summary>
        /// 获取或者设置 设备名称
        /// </summary>
        public string Name { get; private set; } = DeviceInfo.Name;
        /// <summary>
        /// 获取或者设置 设备版本
        /// </summary>
        public string VersionString { get; private set; } = DeviceInfo.VersionString;
        /// <summary>
        /// 获取或者设置 设备版本
        /// </summary>
        public Version Version { get; private set; } = DeviceInfo.Version;
        /// <summary>
        /// 获取或者设置 设备平台
        /// </summary>
        public DevicePlatform Platform { get; private set; } = DeviceInfo.Platform;
        /// <summary>
        /// 获取或者设置 怎么翻译？习语？
        /// </summary>
        public DeviceIdiom Idiom { get; private set; } = DeviceInfo.Idiom;
        /// <summary>
        /// 获取或者设置 设备类型
        /// </summary>
        public DeviceType DeviceType { get; private set; } = DeviceInfo.DeviceType;
    }
}
