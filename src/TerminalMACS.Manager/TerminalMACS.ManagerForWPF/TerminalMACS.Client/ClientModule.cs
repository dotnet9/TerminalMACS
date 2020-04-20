using Prism.Ioc;
using Prism.Regions;
using TerminalMACS.Client.Views;
using TerminalMACS.Infrastructure.UI;
using TerminalMACS.Infrastructure.UI.Modularity;
using Unity;
using WpfExtensions.Xaml;

namespace TerminalMACS.Client
{
    public class ClientModule : ModuleBase
    {
        private readonly IRegionManager _regionManager;
        public ClientModule(IUnityContainer container, IRegionManager regionManager) : base(container)
        {
            I18nManager.Instance.Add(TerminalMACS.Client.I18nResources.UiResource.ResourceManager);
            _regionManager = regionManager;
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.MainTabRegion, typeof(MainTabItem));
            _regionManager.RegisterViewWithRegion(RegionNames.SettingsTabRegion, typeof(SettingsTabItem));
        }
    }
}