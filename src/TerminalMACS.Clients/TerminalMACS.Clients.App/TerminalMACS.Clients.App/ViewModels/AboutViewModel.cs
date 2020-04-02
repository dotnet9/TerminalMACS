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
        public AboutViewModel()
        {
            RaiseOpenDotnet9WebCommand = new Command(async () => await Browser.OpenAsync("https://dotnet9.com/category/terminalmacs/terminalmacs_client/xamarin-forms-terminalmacs_client"));
            RaiseOpenTerminalMACSWebCommand = new Command(async () => await Browser.OpenAsync("https://terminalmacs.com/category/client/xamarin-forms"));
            RaiseOpenCurrentClientSourceWebCommand = new Command(async () => await Browser.OpenAsync("https://github.com/dotnet9/TerminalMACS/tree/master/src/TerminalMACS.Clients/TerminalMACS.Clients.App"));
        }

        public ICommand RaiseOpenDotnet9WebCommand { get; }
        public ICommand RaiseOpenTerminalMACSWebCommand { get; }
        public ICommand RaiseOpenCurrentClientSourceWebCommand { get; }
    }
}