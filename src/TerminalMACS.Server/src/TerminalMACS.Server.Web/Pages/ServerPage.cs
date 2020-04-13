using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using TerminalMACS.Server.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace TerminalMACS.Server.Web.Pages
{
    /* Inherit your UI Pages from this class. To do that, add this line to your Pages (.cshtml files under the Page folder):
     * @inherits TerminalMACS.Server.Web.Pages.ServerPage
     */
    public abstract class ServerPage : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<ServerResource> L { get; set; }
    }
}
