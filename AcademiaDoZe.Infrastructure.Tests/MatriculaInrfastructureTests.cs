// Henrique Churkin Correia Alberton

using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.ValueObjects;
using AcademiaDoZe.Infrastructure.Repositories;

namespace AcademiaDoZe.Infrastructure.Tests;

public class MatriculaInfrastructureTests : TestBase
{
    [Fact]
    public async Task Matricula_Adicionar()
    {
        var repoLogradouro = new LogradouroRepository(ConnectionString, DatabaseType);
        var logradouro = await repoLogradouro.ObterPorId(4);
        Assert.NotNull(logradouro);

        var arquivo = Arquivo.Criar(new byte[] { 1, 2, 3 });

        var _cpf = "11140608981";
        var repoAluno = new AlunoRepository(ConnectionString, DatabaseType);
        Assert.False(await repoAluno.CpfJaExiste(_cpf), "CPF já existe no banco.");

        var aluno = Aluno.Criar(
            1, 
            "Aluno Teste",
            _cpf,
            new DateOnly(2010, 10, 09),
            "49999999999",
            "aluno@teste.com",
            logradouro!,
            "123",
            "Complemento casa",
            "Senha@123",
            arquivo
        );
        await repoAluno.Adicionar(aluno);

        var matricula = Matricula.Criar(
            1, 
            aluno,
            EMatriculaPlano.Semestral,
            DateOnly.FromDateTime(DateTime.Today),
            DateOnly.FromDateTime(DateTime.Today.AddMonths(6)),
            "Emagrecer",
            EMatriculaRestricoes.Alergias,
            arquivo,
            "Sem observações"
        );

        var repoMatricula = new MatriculaRepository(ConnectionString, DatabaseType);
        var matriculaInserida = await repoMatricula.Adicionar(matricula);

        Assert.NotNull(matriculaInserida);
        Assert.True(matriculaInserida.Id > 0);
    }

    [Fact]
    public async Task Matricula_ObterPorAluno_Atualizar()
    {
        var repoMatricula = new MatriculaRepository(ConnectionString, DatabaseType);
        var repoAluno = new AlunoRepository(ConnectionString, DatabaseType);

        var aluno = await repoAluno.ObterPorCpf("11140608981");
        Assert.NotNull(aluno);

        var matriculas = (await repoMatricula.ObterPorAluno(aluno!.Id)).ToList();
        Assert.NotEmpty(matriculas);

        var matricula = matriculas.First();

        var arquivo = Arquivo.Criar(new byte[] { 1, 2, 3 });
        var matriculaAtualizada = Matricula.Criar(
            1, 
            aluno,
            EMatriculaPlano.Anual,
            new DateOnly(2020, 05, 20),
            new DateOnly(2020, 05, 20).AddMonths(12),
            "Hipertrofia",
            EMatriculaRestricoes.Alergias,
            arquivo,
            "Observação atualizada"
        );

        typeof(Entity).GetProperty("Id")?.SetValue(matriculaAtualizada, matricula.Id);

        var resultado = await repoMatricula.Atualizar(matriculaAtualizada);

        Assert.NotNull(resultado);
        Assert.Equal("Hipertrofia", resultado.Objetivo);
        Assert.Equal("Observação atualizada", resultado.ObservacoesRestricoes);
        Assert.Equal(EMatriculaPlano.Anual, resultado.Plano);
    }

    [Fact]
    public async Task Matricula_ObterPorAluno_Remover_ObterPorId()
    {
        var repoMatricula = new MatriculaRepository(ConnectionString, DatabaseType);
        var repoAluno = new AlunoRepository(ConnectionString, DatabaseType);

        var aluno = await repoAluno.ObterPorCpf("11140608981");
        Assert.NotNull(aluno);

        var matriculas = (await repoMatricula.ObterPorAluno(aluno!.Id)).ToList();
        Assert.NotEmpty(matriculas);

        var matricula = matriculas.First(); 
        
        var resultadoRemocao = await repoMatricula.Remover(matricula.Id);

        Assert.True(resultadoRemocao);

        var matriculaRemovida = await repoMatricula.ObterPorId(matricula.Id);
        Assert.Null(matriculaRemovida);
    }

    [Fact]
    public async Task Matrcula_ObterTodos()
    {
        var repoMatriculaTodos = new MatriculaRepository(ConnectionString, DatabaseType);

        var resultado = await repoMatriculaTodos.ObterTodos();
        Assert.NotNull(resultado);
    }

    [Fact]
    public async Task Matricula_ObterPorId()
    {
        var repoMatricula = new MatriculaRepository(ConnectionString, DatabaseType);
        var repoAluno = new AlunoRepository(ConnectionString, DatabaseType);

        var aluno = await repoAluno.ObterPorCpf("12345678901");
        Assert.NotNull(aluno);

        var matriculas = (await repoMatricula.ObterPorAluno(aluno!.Id)).ToList();
        Assert.NotEmpty(matriculas);

        var matricula = matriculas.First();

        var matriculaPorId = await repoMatricula.ObterPorId(matricula.Id);
        Assert.NotNull(matriculaPorId);
        Assert.Equal(matricula.Id, matriculaPorId.Id);
    }
}