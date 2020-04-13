using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using TerminalMACS.Server.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace TerminalMACS.Server.Web.Pages
{
    public abstract class ServerPage : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<ServerResource> L { get; set; }
    }
}
