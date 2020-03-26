using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TerminalMACS.Clients.App.Services;
using TerminalMACS.Clients.App.Views;

namespace TerminalMACS.Clients.App
{
    public partial class App : Application
    {
        public static App Instance { get; private set; }
        public IContactsService ContactsService { get; private set; }
        public App(IContactsService contactsService)
        {
            Instance = this;
            ContactsService = contactsService;
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
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
