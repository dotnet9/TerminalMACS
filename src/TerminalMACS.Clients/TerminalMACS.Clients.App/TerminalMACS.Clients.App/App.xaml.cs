using TerminalMACS.Clients.App.Services;
using TerminalMACS.Clients.App.Views;
using Xamarin.Forms;

namespace TerminalMACS.Clients.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
