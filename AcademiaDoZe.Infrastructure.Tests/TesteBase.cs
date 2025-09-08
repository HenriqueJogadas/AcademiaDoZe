// Henrique Churkin Correia Alberton
using AcademiaDoZe.Infrastructure.Data;

namespace AcademiaDoZe.Infrastructure.Tests
{
    public abstract class TestBase
    {

        protected string ConnectionString { get; private set; }
        protected DatabaseType DatabaseType { get; private set; }

        protected TestBase()
        {
            var config = CreateLocalDbConfig();
            ConnectionString = config.ConnectionString;
            DatabaseType = config.DatabaseType;
        }

        private (string ConnectionString, DatabaseType DatabaseType) CreateLocalDbConfig()
        {
            var connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=AcademiaDoZeTest;Trusted_Connection=True;";

            return (connectionString, DatabaseType.SqlServer);
        }

        private (string ConnectionString, DatabaseType DatabaseType) CreateMySqlConfig()
        {
            var connectionString = "Server=localhost;Database=db_academia_do_ze;User Id=root;Password=abcBolinhas12345;";
            return (connectionString, DatabaseType.MySql);
        }
    }
}
