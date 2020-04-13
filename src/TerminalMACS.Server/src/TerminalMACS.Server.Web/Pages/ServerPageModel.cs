using TerminalMACS.Server.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace TerminalMACS.Server.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class ServerPageModel : AbpPageModel
    {
        protected ServerPageModel()
        {
            LocalizationResourceType = typeof(ServerResource);
        }
    }
}