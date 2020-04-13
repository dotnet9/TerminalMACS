using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components;
using Volo.Abp.DependencyInjection;

namespace TerminalMACS.Server.Web
{
    [Dependency(ReplaceServices = true)]
    public class ServerBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Server";
    }
}
