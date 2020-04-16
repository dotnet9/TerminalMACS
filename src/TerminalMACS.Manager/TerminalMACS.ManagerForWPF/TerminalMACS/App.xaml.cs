using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System.IO;
using System.Windows;
using TerminalMACS.Infrastructure.UI;
using TerminalMACS.Views;

namespace TerminalMACS
{
    public partial class App : PrismApplication
    {
        App()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");
        }
        protected override Window CreateShell()
        {
            LanguageHelper.SetLanguage();
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            string modulePath = @".\Modules";
            if(!Directory.Exists(modulePath))
            {
                Directory.CreateDirectory(modulePath);
            }
            return new DirectoryModuleCatalog() { ModulePath = modulePath };
        }
    }
}
