// Henrique Churkin Correia Alberton

using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Repositories;
using AcademiaDoZe.Infrastructure.Data;
using System.Data;
using System.Data.Common;

namespace AcademiaDoZe.Infrastructure.Repositories;

public class LogradouroRepository : BaseRepository<Logradouro>, ILogradouroRepository
{
    public LogradouroRepository(string connectionString, DatabaseType databaseType) : base(connectionString, databaseType) { }
    protected override async Task<Logradouro> MapAsync(DbDataReader reader)
    {
        try
        {
            var logradouro = Logradouro.Criar(
            id: Convert.ToInt32(reader["id_logradouro"]),
            cep: reader["cep"].ToString()!,
            nome: reader["nome"].ToString()!,
            bairro: reader["bairro"].ToString()!,
            cidade: reader["cidade"].ToString()!,
            estado: reader["estado"].ToString()!,
            pais: reader["pais"].ToString()!);
            
            var idProperty = typeof(Entity).GetProperty("Id");
            idProperty?.SetValue(logradouro, Convert.ToInt32(reader["id_logradouro"]));
            return logradouro;
        }
        catch (DbException ex) { throw new InvalidOperationException($"Erro ao mapear dados do logradouro: {ex.Message}", ex); }
    }
    public override async Task<Logradouro> Adicionar(Logradouro entity)
    {
        try
        {
            await using var connection = await GetOpenConnectionAsync();
            string query = _databaseType == DatabaseType.SqlServer
            ? $"INSERT INTO {TableName} (cep, nome, bairro, cidade, estado, pais) "
            + "OUTPUT INSERTED.id_logradouro "
            + "VALUES (@Cep, @Nome, @Bairro, @Cidade, @Estado, @Pais);"
            : $"INSERT INTO {TableName} (cep, nome, bairro, cidade, estado, pais) "
            + "VALUES (@Cep, @Nome, @Bairro, @Cidade, @Estado, @Pais); "
            + "SELECT LAST_INSERT_ID();";
            await using var command = DbProvider.CreateCommand(query, connection);
            command.Parameters.Add(DbProvider.CreateParameter("@Cep", entity.Cep, DbType.String, _databaseType));
            command.Parameters.Add(DbProvider.CreateParameter("@Nome", entity.Nome, DbType.String, _databaseType));
            command.Parameters.Add(DbProvider.CreateParameter("@Bairro", entity.Bairro, DbType.String, _databaseType));
            command.Parameters.Add(DbProvider.CreateParameter("@Cidade", entity.Cidade, DbType.String, _databaseType));
            command.Parameters.Add(DbProvider.CreateParameter("@Estado", entity.Estado, DbType.String, _databaseType));
            command.Parameters.Add(DbProvider.CreateParameter("@Pais", entity.Pais, DbType.String, _databaseType));
            var id = await command.ExecuteScalarAsync();
            if (id != null && id != DBNull.Value)
            {

                var idProperty = typeof(Entity).GetProperty("Id");
                idProperty?.SetValue(entity, Convert.ToInt32(id));
            }
            return entity;
        }
        catch (DbException ex) { throw new InvalidOperationException($"ERRO_ADD_LOGRADOURO", ex); }
    }
    public async Task<Logradouro?> ObterPorCep(string cep)
    {
        cep = new string(cep.Where(char.IsDigit).ToArray());
        try
        {
            await using var connection = await GetOpenConnectionAsync();
            string query = $"SELECT * FROM {TableName} WHERE cep = @Cep";
            await using var command = DbProvider.CreateCommand(query, connection);
            command.Parameters.Add(DbProvider.CreateParameter("@Cep", cep, DbType.String, _databaseType));
            using var reader = await command.ExecuteReaderAsync();
            return await reader.ReadAsync() ? await MapAsync(reader) : null;
        }
        catch (DbException ex) { throw new InvalidOperationException($"ERRO_OBTER_LOGRADOURO_POR_CEP_{cep}", ex); }
    }

    public override async Task<Logradouro> Atualizar(Logradouro entity)
    {
        try
        {
            await using var connection = await GetOpenConnectionAsync();
            string query = $"UPDATE {TableName} "
            + "SET cep = @Cep, "
            + "nome = @Nome, "
            + "bairro = @Bairro, "
            + "cidade = @Cidade, "
            + "estado = @Estado, "
            + "pais = @Pais "
            + "WHERE id_logradouro = @Id";
            await using var command = DbProvider.CreateCommand(query, connection);
            command.Parameters.Add(DbProvider.CreateParameter("@Id", entity.Id, DbType.Int32, _databaseType));
            command.Parameters.Add(DbProvider.CreateParameter("@Cep", entity.Cep, DbType.String, _databaseType));
            command.Parameters.Add(DbProvider.CreateParameter("@Nome", entity.Nome, DbType.String, _databaseType));
            command.Parameters.Add(DbProvider.CreateParameter("@Bairro", entity.Bairro, DbType.String, _databaseType));
            command.Parameters.Add(DbProvider.CreateParameter("@Cidade", entity.Cidade, DbType.String, _databaseType));
            command.Parameters.Add(DbProvider.CreateParameter("@Estado", entity.Estado, DbType.String, _databaseType));
            command.Parameters.Add(DbProvider.CreateParameter("@Pais", entity.Pais, DbType.String, _databaseType));
            int rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected == 0)
            {
                throw new InvalidOperationException($"LOGRADOURO_NAO_LOCALIZADO_ID_{entity.Id}");
            }
            return entity;
        }
        catch (DbException ex) { throw new InvalidOperationException($"ERRO_UPDATE_LOGRADOURO", ex); }
    }
    public async Task<IEnumerable<Logradouro>> ObterPorCidade(string cidade)
    {
        try
        {
            await using var connection = await GetOpenConnectionAsync();
            string query = $"SELECT * FROM {TableName} WHERE cidade = @Cidade";
            await using var command = DbProvider.CreateCommand(query, connection);
            command.Parameters.Add(DbProvider.CreateParameter("@Cidade", cidade.Trim(), DbType.String, _databaseType));
            using var reader = await command.ExecuteReaderAsync();
            var logradouros = new List<Logradouro>();
            while (await reader.ReadAsync())
            {
                logradouros.Add(await MapAsync(reader));
            }
            return logradouros;
        }
        catch (DbException ex) { throw new InvalidOperationException($"ERRO_OBTER_LOGRADOURO_POR_CIDADE_{cidade}", ex); }
    }
}