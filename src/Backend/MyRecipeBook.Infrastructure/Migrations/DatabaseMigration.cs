using Dapper;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Extensions;
using MySqlConnector;

namespace MyRecipeBook.Infrastructure.Migrations;

public static class DatabaseMigration
{
    public static void Migrate(string connectionString, IServiceProvider serviceProvider)
    {
        EnsureDatabase(connectionString);
        MigrateDatabase(serviceProvider);
    }

    public static void EnsureDatabase(string connectionString)
    {
        var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);
        var databaseName = connectionStringBuilder.Database;

        connectionStringBuilder.Remove("Database");
        
        using var dbConnection = new MySqlConnection(connectionStringBuilder.ConnectionString);

        var parameters = new DynamicParameters();
        parameters.Add("dbname", databaseName);

        var records = dbConnection
            .Query("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @dbname", parameters);

        if (records.Any().IsFalse())
            dbConnection.Execute($"CREATE DATABASE {databaseName}");
    }

    private static void MigrateDatabase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        
        // Lista todas as migrations que vai ta dentro do versions
        runner.ListMigrations();
        runner.MigrateUp();
    }
}