using Umbraco.Cms.Infrastructure.Migrations;

namespace umbraco_registration.Composers
{
    public class CustomTableMigrationPlan : MigrationPlan
    {
        public CustomTableMigrationPlan() : base("CustomRegistration")
        {
            From(string.Empty)
                .To<CreateRegistrationTable>("CreateRegistrationTable");
        }
    }
}
