using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Auto.Database;

public class AutoDbContextFactory : IDesignTimeDbContextFactory<AutoDbContext>
{
    public AutoDbContext CreateDbContext(string[] args)
    {
        // Load cấu hình từ appsettings.json
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        // Đọc loại database và connection string
        string dbType = configuration["DatabaseType"] ?? "PostgreSQL";
        string connectionString = configuration.GetConnectionString("DefaultConnection");

        DbContextOptionsBuilder<AutoDbContext> optionsBuilder = new();

        if (dbType.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase))
        {
            optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
                npgsqlOptions.CommandTimeout(60);
                npgsqlOptions.MigrationsHistoryTable("__MigrationsHistory", "public");
                npgsqlOptions.UseRelationalNulls();
                npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery); // Tối ưu Include()
            })
            .EnableSensitiveDataLogging(false) // Giảm log để tối ưu
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking) // Hạn chế tracking nếu không cần
            .EnableDetailedErrors(false) // Tránh lỗi chi tiết làm chậm hiệu suất
            .EnableServiceProviderCaching(); // Tăng tốc tạo DbContext
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
        }
        else
        {
            throw new InvalidOperationException($"Unsupported database type: {dbType}");
        }

        return new AutoDbContext(optionsBuilder.Options);
    }
}