//Henrique Churkin Correia Alberton
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Repositories;
using AcademiaDoZe.Domain.ValueObjects;
using AcademiaDoZe.Infrastructure.Data;
using System.Data;
using System.Data.Common;

namespace AcademiaDoZe.Infrastructure.Repositories
{
    public class AlunoRepository : BaseRepository<Aluno>, IAlunoRepository
    {

        public AlunoRepository(string connectionString, DatabaseType databaseType)
            : base(connectionString, databaseType) { }

        protected override string IdTableName => "id_aluno";

        private static string Digitos(string s) => new string((s ?? string.Empty).Where(char.IsDigit).ToArray());

        protected override async Task<Aluno> MapAsync(DbDataReader reader)
        {
            try
            {
                var logradouroRepo = new LogradouroRepository(_connectionString, _databaseType);
                var endereco = await logradouroRepo.ObterPorId(Convert.ToInt32(reader["id_logradouro"]));

                var aluno = Aluno.Criar(
                    Nome: reader["nome"].ToString()!,
                    Cpf: reader["cpf"].ToString()!,
                    DataNascimento: DateOnly.FromDateTime(Convert.ToDateTime(reader["data_nascimento"])),
                    Telefone: reader["telefone"].ToString()!,
                    Email: reader["email"].ToString()!,
                    Endereco: endereco!,
                    Numero: reader["numero"].ToString()!,
                    Complemento: reader["complemento"].ToString()!,
                    Senha: reader["senha"].ToString()!,
                    Foto: new Arquivo(reader["foto"].ToString()!)
                );

                typeof(Entity).GetProperty("Id")?.SetValue(aluno, Convert.ToInt32(reader["id_aluno"]));
                return aluno;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("ERRO_MAP_ALUNO", ex);
            }
        }

        public override async Task<Aluno> Adicionar(Aluno entity)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();

                string query = _databaseType == DatabaseType.SqlServer
                    ? $"INSERT INTO {TableName} (nome, cpf, data_nascimento, telefone, email, id_logradouro, numero, complemento, senha, foto) " +
                      "OUTPUT INSERTED.id_aluno " +
                      "VALUES (@Nome, @Cpf, @DataNascimento, @Telefone, @Email, @IdLogradouro, @Numero, @Complemento, @Senha, @Foto);"
                    : $"INSERT INTO {TableName} (nome, cpf, data_nascimento, telefone, email, id_logradouro, numero, complemento, senha, foto) " +
                      "VALUES (@Nome, @Cpf, @DataNascimento, @Telefone, @Email, @IdLogradouro, @Numero, @Complemento, @Senha, @Foto); " +
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

                var id = await command.ExecuteScalarAsync();
                if (id != null && id != DBNull.Value)
                    typeof(Entity).GetProperty("Id")?.SetValue(entity, Convert.ToInt32(id));

                return entity;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException("ERRO_ADD_ALUNO", ex);
            }
        }

        public override async Task<Aluno> Atualizar(Aluno entity)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();

                string query = $"UPDATE {TableName} SET " +
                               "nome = @Nome, cpf = @Cpf, data_nascimento = @DataNascimento, telefone = @Telefone, " +
                               "email = @Email, id_logradouro = @IdLogradouro, numero = @Numero, complemento = @Complemento, " +
                               "senha = @Senha, foto = @Foto " +
                               "WHERE id_aluno = @Id";

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

                int rows = await command.ExecuteNonQueryAsync();
                if (rows == 0)
                    throw new InvalidOperationException($"ALUNO_NAO_LOCALIZADO_ID_{entity.Id}");

                return entity;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException("ERRO_UPDATE_ALUNO", ex);
            }
        }

        public async Task<Aluno?> ObterPorCpf(string cpf)
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
            catch (DbException ex) { throw new InvalidOperationException($"ERRO_OBTER_ALUNO_POR_CPF_{soDigitos}", ex); }
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
            catch (DbException ex) { throw new InvalidOperationException($"ERRO_TROCAR_SENHA_ALUNO_ID_{id}", ex); }
        }
    }
}
