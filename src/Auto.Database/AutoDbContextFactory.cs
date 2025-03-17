using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Notio.Logging;
using System;
using System.IO;

namespace Auto.Database;

public class AutoDbContextFactory : IDesignTimeDbContextFactory<AutoDbContext>
{
    public AutoDbContext CreateDbContext(string[] args)
    {
        CLogging.Instance.Info("Starting initialization of AutoDbContext.");

        // Load cấu hình từ appsettings.json
        IConfigurationRoot configuration;
        try
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }
        catch (Exception ex)
        {
            CLogging.Instance.Error("Error loading configuration from appsettings.json.", ex);
            throw;
        }

        // Đọc loại database và connection string
        string dbType = configuration["DatabaseType"] ?? "PostgreSQL";
        string connectionString = configuration.GetConnectionString("DefaultConnection");

        DbContextOptionsBuilder<AutoDbContext> optionsBuilder = new();

        try
        {
            if (dbType.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase))
            {
                optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
                    npgsqlOptions.CommandTimeout(60);
                    npgsqlOptions.MigrationsHistoryTable("__MigrationsHistory", "public");
                    npgsqlOptions.UseRelationalNulls();
                    npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                })
                .EnableSensitiveDataLogging(false)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableDetailedErrors(false)
                .EnableServiceProviderCaching();

                CLogging.Instance.Info("DbContext configured for PostgreSQL.");
            }
            else if (dbType.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
            {
                optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
                    sqlOptions.CommandTimeout(60);
                    sqlOptions.MigrationsHistoryTable("__MigrationsHistory", "dbo");
                    sqlOptions.UseRelationalNulls();
                })
                .EnableThreadSafetyChecks()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                CLogging.Instance.Info("DbContext configured for SQL Server.");
            }
            else
            {
                CLogging.Instance.Warn($"Unsupported database type: {dbType}");
                throw new InvalidOperationException($"Unsupported database type: {dbType}");
            }
        }
        catch (Exception ex)
        {
            CLogging.Instance.Error($"Error configuring DbContext for {dbType}.", ex);
            throw;
        }

        var dbContext = new AutoDbContext(optionsBuilder.Options);
        CLogging.Instance.Info("AutoDbContext successfully created.");
        return dbContext;
    }
}