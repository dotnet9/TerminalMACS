using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace TerminalMACS.Clients.App.ViewModels
{
    /// <summary>
    /// Client base information page ViewModel
    /// </summary>
    public class ClientInfoViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the model of the device.
        /// </summary>
        public string Model { get; private set; } = DeviceInfo.Model;
        /// <summary>
        /// Gets or sets the manufacturer of the device.
        /// </summary>
        public string Manufacturer { get; private set; } = DeviceInfo.Manufacturer;
        /// <summary>
        /// Gets or sets the name of the device.
        /// </summary>
        public string Name { get; private set; } = DeviceInfo.Name;
        /// <summary>
        /// Gets or sets the version of the operating system.
        /// </summary>
        public string VersionString { get; private set; } = DeviceInfo.VersionString;
        /// <summary>
        /// Gets or sets the version of the operating system.
        /// </summary>
        public Version Version { get; private set; } = DeviceInfo.Version;
        /// <summary>
        /// Gets or sets the platform or operating system of the device.
        /// </summary>
        public DevicePlatform Platform { get; private set; } = DeviceInfo.Platform;
        /// <summary>
        /// Gets or sets the idiom of the device.
        /// </summary>
        public DeviceIdiom Idiom { get; private set; } = DeviceInfo.Idiom;
        /// <summary>
        /// Gets or sets the type of device the application is running on.
        /// </summary>
        public DeviceType DeviceType { get; private set; } = DeviceInfo.DeviceType;
    }
}
