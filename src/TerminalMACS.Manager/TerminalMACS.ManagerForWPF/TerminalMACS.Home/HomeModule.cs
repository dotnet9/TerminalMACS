using Prism.Ioc;
using Prism.Regions;
using TerminalMACS.Home.Views;
using TerminalMACS.Infrastructure.UI;
using TerminalMACS.Infrastructure.UI.Modularity;
using Unity;

namespace TerminalMACS.Home
{
    public class HomeModule : ModuleBase
    {
        private readonly IRegionManager _regionManager;
        public HomeModule(IUnityContainer container, IRegionManager regionManager) : base(container)
        {
            _regionManager = regionManager;
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.MainTabRegion, typeof(MainTabItem));
            _regionManager.RegisterViewWithRegion(RegionNames.SettingsTabRegion, typeof(SettingsTabItem));
        }
    }
}