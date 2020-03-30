using System.ComponentModel;
using TerminalMACS.Clients.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TerminalMACS.Clients.App.Views
{
    /// <summary>
    /// 通讯录页面
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [DesignTimeVisible(false)]
    public partial class ContactPage : ContentPage
    {
        public ContactPage()
        {
            BindingContext = new ContactViewModel(App.Instance.ContactsService);
            InitializeComponent();
        }
    }
}