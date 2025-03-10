using Microsoft.Extensions.Hosting;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;
using umbraco_registration.Composers;

public class CustomMigrationHostedService : IHostedService
{
    private readonly IMigrationPlanExecutor _migrationPlanExecutor;
    private readonly IScopeProvider _scopeProvider;
    private readonly IKeyValueService _keyValueService;

    public CustomMigrationHostedService(
        IMigrationPlanExecutor migrationPlanExecutor,
        IScopeProvider scopeProvider,
        IKeyValueService keyValueService)
    {
        _migrationPlanExecutor = migrationPlanExecutor;
        _scopeProvider = scopeProvider;
        _keyValueService = keyValueService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var migrationPlan = new CustomTableMigrationPlan();
        var upgrader = new Upgrader(migrationPlan);

        upgrader.Execute(_migrationPlanExecutor, _scopeProvider, _keyValueService);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
