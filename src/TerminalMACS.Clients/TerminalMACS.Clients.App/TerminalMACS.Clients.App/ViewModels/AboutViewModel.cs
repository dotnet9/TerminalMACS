using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TerminalMACS.Clients.App.ViewModels
{
    /// <summary>
    /// About page view ViewModel
    /// </summary>
    public class AboutViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets System abbreviation.
        /// </summary>
        public string SystemName { get; set; } = "TerminalMACS";
        /// <summary>
        /// Gets or sets chinese name.
        /// </summary>
        public string SystemChineseName { get; set; } = "多终端管理与检测系统";
        /// <summary>
        /// Gets or sets current client name.
        /// </summary>
        public string CurrentClientName { get; set; } = "移动客户端Xamarin.Forms版";
        /// <summary>
        /// 获取或者设置 current client version.
        /// </summary>
        public string CurrentClientVersion { get; set; } = "1.0";
        public AboutViewModel()
        {
            Title = "关于";
            RaiseOpenDotnet9WebCommand = new Command(async () => await Browser.OpenAsync("https://dotnet9.com/category/terminalmacs/terminalmacs_client/xamarin-forms-terminalmacs_client"));
            RaiseOpenTerminalMACSWebCommand = new Command(async () => await Browser.OpenAsync("https://terminalmacs.com/category/client/xamarin-forms"));
            RaiseOpenCurrentClientSourceWebCommand = new Command(async () => await Browser.OpenAsync("https://github.com/dotnet9/TerminalMACS/tree/master/src/TerminalMACS.Clients/TerminalMACS.Clients.App"));
        }

        public ICommand RaiseOpenDotnet9WebCommand { get; }
        public ICommand RaiseOpenTerminalMACSWebCommand { get; }
        public ICommand RaiseOpenCurrentClientSourceWebCommand { get; }
    }
}