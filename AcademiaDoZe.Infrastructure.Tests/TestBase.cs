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
        var connectionString = "Server=localhost;Database=db_academia_do_ze;User Id=root;Password=rayssa;";

        return (connectionString, DatabaseType.MySql);

    }
}