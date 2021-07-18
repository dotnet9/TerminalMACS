using System.ComponentModel;
using TerminalMACS.Clients.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TerminalMACS.Clients.App.Views
{
    /// <summary>
    /// Contact list page
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [DesignTimeVisible(false)]
    public partial class ContactPage : ContentPage
    {
        public ContactPage()
        {
            InitializeComponent();
        }
    }
}