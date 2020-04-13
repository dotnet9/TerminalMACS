using TerminalMACS.Server.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace TerminalMACS.Server.Web.Pages
{
    public abstract class ServerPageModel : AbpPageModel
    {
        protected ServerPageModel()
        {
            LocalizationResourceType = typeof(ServerResource);
        }
    }
}