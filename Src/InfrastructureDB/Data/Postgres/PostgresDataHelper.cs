using System.Runtime.CompilerServices;

namespace InfrastructureDB.Data.Postgres
{
    public static class PostgresDataHelper
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }
}
