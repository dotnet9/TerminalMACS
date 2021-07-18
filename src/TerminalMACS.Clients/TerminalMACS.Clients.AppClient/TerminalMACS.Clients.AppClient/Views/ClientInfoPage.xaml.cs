using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TerminalMACS.Clients.App.Views
{
    /// <summary>
    /// Basic information page of client
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientInfoPage : ContentPage
    {
        public ClientInfoPage()
        {
            InitializeComponent();
        }
    }
}