using Microsoft.Extensions.Hosting;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;

public class MigrationRunner<T> : IHostedService where T : MigrationPlan
{
    private readonly IScopeProvider _scopeProvider;
    private readonly IMigrationPlanExecutor _executor;
    private readonly IKeyValueService _keyValueService;
    private readonly T _plan;

    public MigrationRunner(
        IScopeProvider scopeProvider,
        IMigrationPlanExecutor executor,
        IKeyValueService keyValueService,
        T plan)
    {
        _scopeProvider = scopeProvider;
        _executor = executor;
        _keyValueService = keyValueService;
        _plan = plan;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _scopeProvider.CreateScope())
        {
            var upgrader = new Upgrader(_plan);

            // Execute the migration plan
            upgrader.Execute(_executor, _scopeProvider, _keyValueService);

            scope.Complete();
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
