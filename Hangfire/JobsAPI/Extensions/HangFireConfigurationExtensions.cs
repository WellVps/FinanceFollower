using Hangfire;
using Hangfire.Mongo;
using JobsAPI.Options;
using JobsService.Base;
using JobsAPI.Security;
using Microsoft.Extensions.Options;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;

namespace JobsAPI.Extensions;

public static class HangFireConfigurationExtensions
{
    public static void AddHangFireConfigs(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<HangFireOptions>(op => {
            var connection = configuration[$"MongoDB:Jobs:HangFire"];
            op.DBContext = $"{connection}";
        });

        services.AddHangfire((provider, configuration) => {
            var options = provider.GetRequiredService<IOptions<HangFireOptions>>().Value;
            configuration.UseSimpleAssemblyNameTypeSerializer();
            configuration.UseRecommendedSerializerSettings();
            configuration.UseMongoStorage(options.DBContext, new MongoStorageOptions
            {
                MigrationOptions = new MongoMigrationOptions
                {
                    MigrationStrategy = new DropMongoMigrationStrategy(),
                    BackupStrategy = new NoneMongoBackupStrategy()
                }
            });
        });

        services.AddHangfireServer(options => {
            options.ServerName = "FinanceFollower Jobs";
        });
    }

    public static void ScheduleBackgroundJobs(this IApplicationBuilder app)
    {
        app.UseHangfireDashboard("/jobs", new DashboardOptions()
        {
            DashboardTitle = "FinanceFollower Jobs",
            DarkModeEnabled = true,
            IsReadOnlyFunc = context => true,
            Authorization = new[] { new HangFireAuthorization() },
            DisplayStorageConnectionString = false,
            AppPath = "/swagger/index.html",
        });

        using(var scope = app.ApplicationServices.CreateScope())
        {
            var jobManager = scope.ServiceProvider.GetService<IJobManager>();
            jobManager?.RegisterBackgroundJobs();
        }
    }
}
