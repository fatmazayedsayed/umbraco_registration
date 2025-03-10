using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseModelDefinitions;

public class CreateRegistrationTable : MigrationBase
{
    public CreateRegistrationTable(IMigrationContext context) : base(context) { }

    protected override void Migrate()
    {
        if (!TableExists("Registration"))
        {
            Create.Table("Registration")
                .WithColumn("Id").AsInt32().PrimaryKey("PK_Registration").Identity()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("Email").AsString(255).NotNullable()
                .WithColumn("productId").AsInt32().NotNullable()
                 .WithColumn("CreatedDate").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
                .Do();
        }
    }
}
