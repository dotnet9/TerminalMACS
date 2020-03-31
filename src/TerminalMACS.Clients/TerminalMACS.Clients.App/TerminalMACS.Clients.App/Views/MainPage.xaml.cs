using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using TerminalMACS.Clients.App.Models;
using Xamarin.Forms;

namespace TerminalMACS.Clients.App.Views
{
    /// <summary>
    /// 主页
    /// </summary>
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            MenuPages.Add((int)MenuItemType.About, (NavigationPage)Detail);
        }

        /// <summary>
        /// 异步跳转菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.ClientInfo:
                        MenuPages.Add(id, new NavigationPage(new ClientInfoPage()));
                        break;
                    case (int)MenuItemType.Contacts:
                        MenuPages.Add(id, new NavigationPage(new ContactPage()));
                        break;
                    case (int)MenuItemType.About:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}