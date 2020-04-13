using Volo.Abp.Modularity;

namespace TerminalMACS.Server
{
    [DependsOn(
        typeof(ServerApplicationModule),
        typeof(ServerDomainTestModule)
        )]
    public class ServerApplicationTestModule : AbpModule
    {

    }
}