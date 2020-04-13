using System.Threading.Tasks;

namespace TerminalMACS.Server.Data
{
    public interface IServerDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
