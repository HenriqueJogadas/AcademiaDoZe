//Henrique Churkin Correia Alberton
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Repositories;
using AcademiaDoZe.Infrastructure.Data;
using System.Data;
using System.Data.Common;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Infrastructure.Repositories
{
    public class ColaboradorRepository : BaseRepository<Colaborador>, IColaboradorRepository
    {

        public ColaboradorRepository(string connectionString, DatabaseType databaseType)
            : base(connectionString, databaseType) { }

        protected override string IdTableName => "id_colaborador";

        private static string Digitos(string s) => new string((s ?? string.Empty).Where(char.IsDigit).ToArray());

        protected override async Task<Colaborador> MapAsync(DbDataReader reader)
        {
            try
            {
                var logradouroRepo = new LogradouroRepository(_connectionString, _databaseType);
                var endereco = await logradouroRepo.ObterPorId(Convert.ToInt32(reader["id_logradouro"]));

                var colaborador = Colaborador.Criar(
                    Nome: reader["nome"].ToString()!,
                    Cpf: reader["cpf"].ToString()!,
                    DataNascimento: DateOnly.FromDateTime(Convert.ToDateTime(reader["data_nascimento"])),
                    Telefone: reader["telefone"].ToString()!,
                    Email: reader["email"].ToString()!,
                    Endereco: endereco!,
                    Numero: reader["numero"].ToString()!,
                    Complemento: reader["complemento"].ToString()!,
                    Senha: reader["senha"].ToString()!,
                    Foto: new Arquivo(reader["foto"].ToString()!),
                    DataAdmissao: DateOnly.FromDateTime(Convert.ToDateTime(reader["data_admissao"])),
                    Cargo: (EnumCargo)Convert.ToInt32(reader["tipo"]),
                    Vinculo: (EnumVinculo)Convert.ToInt32(reader["vinculo"])
                );

                typeof(Entity).GetProperty("Id")?.SetValue(colaborador, Convert.ToInt32(reader["id_colaborador"]));
                return colaborador;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("ERRO_MAP_COLABORADOR", ex);
            }
        }

        public override async Task<Colaborador> Adicionar(Colaborador entity)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();

                string query = _databaseType == DatabaseType.SqlServer
                    ? $"INSERT INTO {TableName} (nome, cpf, data_nascimento, telefone, email, id_logradouro, numero, complemento, senha, foto, data_admissao, tipo, vinculo) " +
                      "OUTPUT INSERTED.id_colaborador " +
                      "VALUES (@Nome, @Cpf, @DataNascimento, @Telefone, @Email, @IdLogradouro, @Numero, @Complemento, @Senha, @Foto, @DataAdmissao, @Tipo, @Vinculo);"
                    : $"INSERT INTO {TableName} (nome, cpf, data_nascimento, telefone, email, id_logradouro, numero, complemento, senha, foto, data_admissao, tipo, vinculo) " +
                      "VALUES (@Nome, @Cpf, @DataNascimento, @Telefone, @Email, @IdLogradouro, @Numero, @Complemento, @Senha, @Foto, @DataAdmissao, @Tipo, @Vinculo); " +
                      "SELECT LAST_INSERT_ID();";

                await using var command = DbProvider.CreateCommand(query, connection);

                command.Parameters.Add(DbProvider.CreateParameter("@Nome", entity.Nome, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Cpf", Digitos(entity.Cpf), DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@DataNascimento", entity.DataNascimento.ToDateTime(TimeOnly.MinValue), DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Telefone", Digitos(entity.Telefone), DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Email", entity.Email, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@IdLogradouro", entity.Endereco.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Numero", entity.Numero, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Complemento", entity.Complemento, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Senha", entity.Senha, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Foto", entity.Foto, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@DataAdmissao", entity.DataAdmissao.ToDateTime(TimeOnly.MinValue), DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Tipo", (int)entity.Cargo, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Vinculo", (int)entity.Vinculo, DbType.Int32, _databaseType));

                var id = await command.ExecuteScalarAsync();
                if (id != null && id != DBNull.Value)
                    typeof(Entity).GetProperty("Id")?.SetValue(entity, Convert.ToInt32(id));

                return entity;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException("ERRO_ADD_COLABORADOR", ex);
            }
        }

        public override async Task<Colaborador> Atualizar(Colaborador entity)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();

                string query = $"UPDATE {TableName} SET " +
                               "nome = @Nome, cpf = @Cpf, data_nascimento = @DataNascimento, telefone = @Telefone, " +
                               "email = @Email, id_logradouro = @IdLogradouro, numero = @Numero, complemento = @Complemento, " +
                               "senha = @Senha, foto = @Foto, data_admissao = @DataAdmissao, tipo = @Tipo, vinculo = @Vinculo " +
                               "WHERE id_colaborador = @Id";

                await using var command = DbProvider.CreateCommand(query, connection);

                command.Parameters.Add(DbProvider.CreateParameter("@Id", entity.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Nome", entity.Nome, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Cpf", Digitos(entity.Cpf), DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@DataNascimento", entity.DataNascimento.ToDateTime(TimeOnly.MinValue), DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Telefone", Digitos(entity.Telefone), DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Email", entity.Email, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@IdLogradouro", entity.Endereco.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Numero", entity.Numero, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Complemento", entity.Complemento, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Senha", entity.Senha, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Foto", entity.Foto, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@DataAdmissao", entity.DataAdmissao.ToDateTime(TimeOnly.MinValue), DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Tipo", (int)entity.Cargo, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Vinculo", (int)entity.Vinculo, DbType.Int32, _databaseType));

                int rows = await command.ExecuteNonQueryAsync();
                if (rows == 0)
                    throw new InvalidOperationException($"COLABORADOR_NAO_LOCALIZADO_ID_{entity.Id}");

                return entity;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException("ERRO_UPDATE_COLABORADOR", ex);
            }
        }

        public async Task<Colaborador?> ObterPorCpf(string cpf)
        {
            var soDigitos = Digitos(cpf);
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = $"SELECT * FROM {TableName} WHERE cpf = @Cpf";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Cpf", soDigitos, DbType.String, _databaseType));
                await using var reader = await command.ExecuteReaderAsync();
                return await reader.ReadAsync() ? await MapAsync(reader) : null;
            }
            catch (DbException ex) { throw new InvalidOperationException($"ERRO_OBTER_COLABORADOR_POR_CPF_{soDigitos}", ex); }
        }

        public async Task<bool> CpfJaExiste(string cpf, int? ignorarId = null)
        {
            var soDigitos = Digitos(cpf);
            try
            {
                await using var connection = await GetOpenConnectionAsync();

                string query = ignorarId.HasValue
                    ? $"SELECT COUNT(1) FROM {TableName} WHERE cpf = @Cpf AND {IdTableName} <> @Id"
                    : $"SELECT COUNT(1) FROM {TableName} WHERE cpf = @Cpf";

                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Cpf", soDigitos, DbType.String, _databaseType));
                if (ignorarId.HasValue)
                    command.Parameters.Add(DbProvider.CreateParameter("@Id", ignorarId.Value, DbType.Int32, _databaseType));

                var count = Convert.ToInt32(await command.ExecuteScalarAsync());
                return count > 0;
            }
            catch (DbException ex) { throw new InvalidOperationException($"ERRO_CPF_JA_EXISTE_{soDigitos}", ex); }
        }

        public async Task<bool> TrocarSenha(int id, string novaSenhaHash)
        {
            if (id <= 0) throw new ArgumentException("ID_INVALIDO", nameof(id));
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = $"UPDATE {TableName} SET senha = @Senha WHERE {IdTableName} = @Id";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Senha", novaSenhaHash, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Id", id, DbType.Int32, _databaseType));
                var rows = await command.ExecuteNonQueryAsync();
                return rows > 0;
            }
            catch (DbException ex) { throw new InvalidOperationException($"ERRO_TROCAR_SENHA_COLABORADOR_ID_{id}", ex); }
        }
    }
}
