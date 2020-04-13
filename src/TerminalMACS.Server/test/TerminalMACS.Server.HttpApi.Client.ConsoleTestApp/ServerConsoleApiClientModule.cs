using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace TerminalMACS.Server.HttpApi.Client.ConsoleTestApp
{
    [DependsOn(
        typeof(ServerHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class ServerConsoleApiClientModule : AbpModule
    {
        
    }
}
