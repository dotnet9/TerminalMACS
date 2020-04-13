using TerminalMACS.Server.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace TerminalMACS.Server.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(ServerEntityFrameworkCoreDbMigrationsModule),
        typeof(ServerApplicationContractsModule)
        )]
    public class ServerDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
