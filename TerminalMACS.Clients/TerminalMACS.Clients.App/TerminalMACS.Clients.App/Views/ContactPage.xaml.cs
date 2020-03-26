using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalMACS.Clients.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TerminalMACS.Clients.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactPage : ContentPage
    {
        public ContactPage()
        {
            BindingContext = new ContactViewModel(App.Instance.ContactsService);
            InitializeComponent();
        }
    }
}