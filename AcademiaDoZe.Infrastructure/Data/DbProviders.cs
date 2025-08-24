//Henrique Churkin Correia Alberton
using AcademiaDoZe.Infrastructure.Exceptions;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace AcademiaDoZe.Infrastructure.Data
{

    public enum DatabaseType { SqlServer, MySql }

    public static class DbProvider
    {
        public const int DefaultCommandTimeout = 30;

        public static DbConnection CreateConnection(string connectionString, DatabaseType dbType)
        {
            try
            {
                DbConnection connection = dbType switch
                {
                    DatabaseType.SqlServer => new SqlConnection(connectionString),
                    DatabaseType.MySql => new MySqlConnection(connectionString),
                    _ => throw new InfrastructureException($"SGDB_NAO_SUPORTADO {dbType}")
                };

                if (connection == null)
                    throw new InfrastructureException($"FALHA_CONEXAO {dbType}");

                return connection;
            }
            catch (DbException ex)
            {
                throw new InfrastructureException($"FALHA_CONEXAO {dbType}", ex);
            }
        }

        public static DbCommand CreateCommand(string commandText, DbConnection connection, CommandType commandType = CommandType.Text)
        {
            if (connection == null)
                throw new ArgumentNullException($"COMMAND_CONEXAO_NULL");

            if (string.IsNullOrWhiteSpace(commandText))
                throw new ArgumentException($"COMMAND_TEXT_VAZIO");

            try
            {
                var command = connection.CreateCommand() ?? throw new InfrastructureException("FALHA_CRIAR_COMANDO");
                command.CommandText = commandText;
                command.CommandType = commandType;
                command.CommandTimeout = DefaultCommandTimeout;
                return command;
            }
            catch (DbException ex)
            {
                throw new InfrastructureException($"FALHA_CRIAR_COMANDO", ex);
            }
        }

        public static DbParameter CreateParameter(string name, object value, DbType dbType, DatabaseType databaseType)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"PARAMETER_VAZIO");

            try
            {
                DbParameter parameter = databaseType switch
                {
                    DatabaseType.SqlServer => new SqlParameter(name, value ?? DBNull.Value)
                    {
                        DbType = dbType
                    },
                    DatabaseType.MySql => new MySqlParameter(name, value ?? DBNull.Value)
                    {
                        DbType = dbType
                    },
                    _ => throw new InfrastructureException($"SGDB_NAO_SUPORTADO {databaseType}")
                };

                return parameter;
            }
            catch (DbException ex)
            {
                throw new InfrastructureException($"ERRO_CRIAR_PARAMETRO", ex);
            }
        }
    }
}
