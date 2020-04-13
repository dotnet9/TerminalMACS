using TerminalMACS.Server.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace TerminalMACS.Server
{
    [DependsOn(
        typeof(ServerEntityFrameworkCoreTestModule)
        )]
    public class ServerDomainTestModule : AbpModule
    {

    }
}