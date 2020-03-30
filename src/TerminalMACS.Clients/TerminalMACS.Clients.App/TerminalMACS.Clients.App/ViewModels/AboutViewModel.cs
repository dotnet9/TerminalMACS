using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TerminalMACS.Clients.App.ViewModels
{
    /// <summary>
    /// 关于页面ViewModel
    /// </summary>
    public class AboutViewModel : BaseViewModel
    {
        /// <summary>
        /// 获取或者设置 系统简称名称
        /// </summary>
        public string SystemName { get; set; } = "TerminalMACS";
        /// <summary>
        /// 获取或者设置 系统中文名称
        /// </summary>
        public string SystemChineseName { get; set; } = "多终端管理与检测系统";
        /// <summary>
        /// 获取或者设置 当前客户端名称
        /// </summary>
        public string CurrentClientName { get; set; } = "移动客户端Xamarin.Forms版";
        /// <summary>
        /// 获取或者设置 当前Xamarin.Forms客户端版本
        /// </summary>
        public string CurrentClientVersion { get; set; } = "1.0";
        public AboutViewModel()
        {
            Title = "关于";
            OpenDotnet9WebCommand = new Command(async () => await Browser.OpenAsync("https://dotnet9.com/category/terminalmacs/terminalmacs_client/xamarin-forms-terminalmacs_client"));
            OpenTerminalMACSWebCommand = new Command(async () => await Browser.OpenAsync("https://terminalmacs.com/category/client/xamarin-forms"));
            OpenCurrentClientSourceWebCommand = new Command(async () => await Browser.OpenAsync("https://github.com/dotnet9/TerminalMACS/tree/master/src/TerminalMACS.Clients/TerminalMACS.Clients.App"));
        }

        public ICommand OpenDotnet9WebCommand { get; }
        public ICommand OpenTerminalMACSWebCommand { get; }
        public ICommand OpenCurrentClientSourceWebCommand { get; }
    }
}