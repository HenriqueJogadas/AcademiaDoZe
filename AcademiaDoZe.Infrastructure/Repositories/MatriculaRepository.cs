//Henrique Churkin Correia Alberton
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Repositories;
using AcademiaDoZe.Domain.ValueObjects;
using AcademiaDoZe.Infrastructure.Data;
using System.Data;
using System.Data.Common;

namespace AcademiaDoZe.Infrastructure.Repositories
{
    public class MatriculaRepository : BaseRepository<Matricula>, IMatriculaRepository
    {
        private readonly string _connectionString;
        private readonly DatabaseType _databaseType;

        public MatriculaRepository(string connectionString, DatabaseType databaseType)
      : base(connectionString, databaseType) { }

        public async Task<IEnumerable<Matricula>> ObterPorAluno(int alunoId)
        {
            var lista = new List<Matricula>();

            using var connection = DbProvider.CreateConnection(_connectionString, _databaseType);
            connection.Open();

            using var command = DbProvider.CreateCommand(
                @"SELECT * FROM Matricula WHERE AlunoId = @AlunoId",
                connection,
                CommandType.Text
            );
            command.Parameters.Add(DbProvider.CreateParameter("@AlunoId", alunoId, DbType.Int32, _databaseType));

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(MapMatricula(reader));
            }

            return lista;
        }

        public async Task<IEnumerable<Matricula>> ObterAtivas()
        {
            var lista = new List<Matricula>();

            using var connection = DbProvider.CreateConnection(_connectionString, _databaseType);
            connection.Open();

            using var command = DbProvider.CreateCommand(
                @"SELECT * FROM Matricula WHERE DataFim >= @Hoje",
                connection,
                CommandType.Text
            );
            command.Parameters.Add(DbProvider.CreateParameter("@Hoje", DateOnly.FromDateTime(DateTime.Today), DbType.Date, _databaseType));

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(MapMatricula(reader));
            }

            return lista;
        }

        public async Task<IEnumerable<Matricula>> ObterVencendoEmDias(int dias)
        {
            var lista = new List<Matricula>();
            var hoje = DateOnly.FromDateTime(DateTime.Today);
            var dataMax = hoje.AddDays(dias);

            using var connection = DbProvider.CreateConnection(_connectionString, _databaseType);
            connection.Open();

            using var command = DbProvider.CreateCommand(
                @"SELECT * FROM Matricula WHERE DataFim BETWEEN @Hoje AND @DataMax",
                connection,
                CommandType.Text
            );
            command.Parameters.Add(DbProvider.CreateParameter("@Hoje", hoje, DbType.Date, _databaseType));
            command.Parameters.Add(DbProvider.CreateParameter("@DataMax", dataMax, DbType.Date, _databaseType));

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(MapMatricula(reader));
            }

            return lista;
        }

        private Matricula MapMatricula(DbDataReader reader)
        {
            // nada funciona tudo da errado como pode?
            // desisto
            var aluno = Aluno.Criar(
                Nome: reader["nome"].ToString()!,
                Cpf: reader["cpf"].ToString()!,
                DataNascimento: DateOnly.FromDateTime(Convert.ToDateTime(reader["data_nascimento"])),
                Telefone: reader["telefone"].ToString()!,
                Email: reader["email"].ToString()!,
                Endereco: reader["endereco"],
                Numero: reader["numero"].ToString()!,
                Complemento: reader["complemento"].ToString()!,
                Senha: reader["senha"].ToString()!,
                Foto: new Arquivo(reader["foto"].ToString()!)
            );

            var plano = (EnumPlano)Enum.Parse(typeof(EnumPlano), reader["Plano"].ToString());
            var restricoes = (EnumRestricoes)Enum.Parse(typeof(EnumRestricoes), reader["RestricoesMedicas"].ToString());
            var dataInicio = DateOnly.FromDateTime(Convert.ToDateTime(reader["DataInicio"]));
            var dataFinal = DateOnly.FromDateTime(Convert.ToDateTime(reader["DataFim"]));
            var objetivo = reader["Objetivo"].ToString();
            var observacoes = reader["ObservacoesRestricoes"].ToString();
            Arquivo laudomedico = null;

            return Matricula.Criar(aluno, plano, dataInicio, dataFinal, objetivo, restricoes, laudomedico, observacoes);
        }
        protected override async Task<Matricula> MapAsync(DbDataReader reader)
        {
            try
            {
                var alunoRepo = new AlunoRepository(_connectionString, _databaseType);
                var aluno = await alunoRepo.ObterPorId(Convert.ToInt32(reader["id_aluno"]));

                var matricula = Matricula.Criar(
                    Aluno: aluno!,
                    Plano: (EnumPlano)Convert.ToInt32(reader["plano"]),
                    DataInicio: DateOnly.FromDateTime(Convert.ToDateTime(reader["data_inicio"])),
                    DataFinal: DateOnly.FromDateTime(Convert.ToDateTime(reader["data_fim"])),
                    Objetivo: reader["objetivo"].ToString()!,
                    Restricoes: (EnumRestricoes)Convert.ToInt32(reader["restricoes_medicas"]),
                    LaudoMedico: string.IsNullOrWhiteSpace(reader["laudo_medico"]?.ToString()) ? null! : new Arquivo(reader["laudo_medico"].ToString()!),
                    ObservacoesRestricoes: reader["observacoes_restricoes"]?.ToString() ?? string.Empty
                );

                typeof(Entity).GetProperty("Id")?.SetValue(matricula, Convert.ToInt32(reader["id_matricula"]));
                return matricula;
            }
            catch (Exception ex) { throw new InvalidOperationException("ERRO_MAP_MATRICULA", ex); }
        }

        public override async Task<Matricula> Adicionar(Matricula entity)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();

                string query = _databaseType == DatabaseType.SqlServer
                    ? $"INSERT INTO {TableName} (id_aluno, plano, data_inicio, data_fim, objetivo, restricoes_medicas, laudo_medico, observacoes_restricoes) " +
                      "OUTPUT INSERTED.id_matricula " +
                      "VALUES (@IdAluno, @Plano, @DataInicio, @DataFim, @Objetivo, @Restricoes, @Laudo, @Obs);"
                    : $"INSERT INTO {TableName} (id_aluno, plano, data_inicio, data_fim, objetivo, restricoes_medicas, laudo_medico, observacoes_restricoes) " +
                      "VALUES (@IdAluno, @Plano, @DataInicio, @DataFim, @Objetivo, @Restricoes, @Laudo, @Obs); SELECT LAST_INSERT_ID();";

                await using var command = DbProvider.CreateCommand(query, connection);

                command.Parameters.Add(DbProvider.CreateParameter("@IdAluno", entity.Aluno.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Plano", (int)entity.Plano, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@DataInicio", entity.DataInicio.ToDateTime(TimeOnly.MinValue), DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@DataFim", entity.DataFinal.ToDateTime(TimeOnly.MinValue), DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Objetivo", entity.Objetivo, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Restricoes", (int)entity.Restricoes, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Laudo", entity.LaudoMedico?.Caminho ?? string.Empty, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Obs", entity.ObservacoesRestricoes ?? string.Empty, DbType.String, _databaseType));

                var id = await command.ExecuteScalarAsync();
                if (id != null && id != DBNull.Value)
                    typeof(Entity).GetProperty("Id")?.SetValue(entity, Convert.ToInt32(id));

                return entity;
            }
            catch (DbException ex) { throw new InvalidOperationException("ERRO_ADD_MATRICULA", ex); }
        }

        public override async Task<Matricula> Atualizar(Matricula entity)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();

                string query = $"UPDATE {TableName} SET " +
                               "id_aluno = @IdAluno, plano = @Plano, data_inicio = @DataInicio, data_fim = @DataFim, objetivo = @Objetivo, " +
                               "restricoes_medicas = @Restricoes, laudo_medico = @Laudo, observacoes_restricoes = @Obs " +
                               "WHERE id_matricula = @Id";

                await using var command = DbProvider.CreateCommand(query, connection);

                command.Parameters.Add(DbProvider.CreateParameter("@Id", entity.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@IdAluno", entity.Aluno.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Plano", (int)entity.Plano, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@DataInicio", entity.DataInicio.ToDateTime(TimeOnly.MinValue), DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@DataFim", entity.DataFinal.ToDateTime(TimeOnly.MinValue), DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Objetivo", entity.Objetivo, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Restricoes", (int)entity.Restricoes, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Obs", entity.ObservacoesRestricoes ?? string.Empty, DbType.String, _databaseType));

                var rows = await command.ExecuteNonQueryAsync();
                if (rows == 0) throw new InvalidOperationException($"MATRICULA_NAO_LOCALIZADA_ID_{entity.Id}");

                return entity;
            }
            catch (DbException ex) { throw new InvalidOperationException("ERRO_UPDATE_MATRICULA", ex); }
        }
    }
}
