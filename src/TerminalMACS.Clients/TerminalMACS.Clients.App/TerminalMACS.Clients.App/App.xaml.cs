using TerminalMACS.Clients.App.Services;
using TerminalMACS.Clients.App.Views;
using Xamarin.Forms;

namespace TerminalMACS.Clients.App
{
    public partial class App : Application
    {
        public static App Instance { get; private set; }
        public IContactsService ContactsService { get; private set; }

        /// <summary>
        /// Contact service instance, transferred from terminal app to this shared library
        /// </summary>
        /// <param name="contactsService"></param>
        public App(IContactsService contactsService)
        {
            Instance = this;
            ContactsService = contactsService;
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
