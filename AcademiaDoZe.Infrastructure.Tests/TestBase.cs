// Henrique Churkin Correia Alberton

using AcademiaDoZe.Infrastructure.Data;

namespace AcademiaDoZe.Infrastructure.Tests;

public abstract class TestBase
{
    protected string ConnectionString { get; private set; }
    protected DatabaseType DatabaseType { get; private set; }
    protected TestBase()
    {
        var config = CreateMySqlConfig();
        ConnectionString = config.ConnectionString;
        DatabaseType = config.DatabaseType;
    }
    private (string ConnectionString, DatabaseType DatabaseType) CreateMySqlConfig()
    {
        var connectionString = "Server=localhost;Port=3306;Database=db_academiadoze;User Id=root;Password=admin;";

        return (connectionString, DatabaseType.MySql);

    }
}