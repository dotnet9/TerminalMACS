using System;
using System.Collections.Generic;
using System.Text;
using TerminalMACS.Server.Localization;
using Volo.Abp.Application.Services;

namespace TerminalMACS.Server
{
    /* Inherit your application services from this class.
     */
    public abstract class ServerAppService : ApplicationService
    {
        protected ServerAppService()
        {
            LocalizationResource = typeof(ServerResource);
        }
    }
}
