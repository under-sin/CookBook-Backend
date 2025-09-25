using System.Data;
using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_RECIPES, "Create a table to save the recipe's information")]
public class Version0000002 : VersionBase
{
    public override void Up()
    {
        CreateTable("Recipes")
            .WithColumn("Title").AsString(255).NotNullable()
            .WithColumn("CookingTime").AsInt32().Nullable()
            .WithColumn("Difficulty").AsInt32().Nullable()
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Recipe_User_Id", "Users", "Id");

        CreateTable("Ingredients")
            .WithColumn("Item").AsString().NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_Ingredient_RecipeId", "Recipes", "Id")
            .OnDelete(Rule.Cascade); // Delete cascade
        
        CreateTable("Instructions")
            .WithColumn("Step").AsInt32().NotNullable()
            .WithColumn("Text").AsString(2000).NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_Instruction_RecipeId", "Recipes", "Id")
            .OnDelete(Rule.Cascade); 
        
        CreateTable("DishTypes")
            .WithColumn("Type").AsInt32().NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_DishType_RecipeId", "Recipes", "Id")
            .OnDelete(Rule.Cascade); 
    }
}