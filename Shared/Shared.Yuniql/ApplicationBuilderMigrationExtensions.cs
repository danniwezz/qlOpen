using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Yuniql.AspNetCore;
using Yuniql.SqlServer;

namespace Shared.Yuniql;

public static class ApplicationBuilderMigrationExtensions
{

    /// <summary>
    /// Not really a middleware tho
    /// </summary>
    /// <param name="this"></param>
    /// <param name="sqlConnectionString"></param>
    /// <returns></returns>
    public static WebApplication UseMigrations(this WebApplication @this, string sqlConnectionString, string? currentDirectory = null)
    {
        var traceService = new ConsoleTraceService { IsDebugEnabled = true };
        var dataService = new SqlServerDataService(traceService);
        var bulkService = new SqlServerBulkImportService(traceService);

        @this.UseYuniql(dataService, bulkService, traceService, new Configuration
        {
            Platform = "sqlserver",
            Workspace = Path.Combine(currentDirectory ?? Environment.CurrentDirectory, "Database", "Migrations"),
            ConnectionString = sqlConnectionString,
            IsAutoCreateDatabase = @this.Environment.IsDevelopment(),
            IsDebug = true
        });
        return @this;
    }
}
