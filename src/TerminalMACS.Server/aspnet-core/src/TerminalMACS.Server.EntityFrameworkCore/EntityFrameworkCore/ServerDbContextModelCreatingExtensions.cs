using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace TerminalMACS.Server.EntityFrameworkCore
{
    public static class ServerDbContextModelCreatingExtensions
    {
        public static void ConfigureServer(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(ServerConsts.DbTablePrefix + "YourEntities", ServerConsts.DbSchema);

            //    //...
            //});
        }
    }
}