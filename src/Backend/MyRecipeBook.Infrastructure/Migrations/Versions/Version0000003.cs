using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.AddTableImageIdentifier, "Add ImageIdentifier column to Recipe table")]
public class Version0000003 : VersionBase
{
    public override void Up()
    {
        Alter.Table("Recipes").AddColumn("ImageIdentifier").AsString().Nullable();
    }
}