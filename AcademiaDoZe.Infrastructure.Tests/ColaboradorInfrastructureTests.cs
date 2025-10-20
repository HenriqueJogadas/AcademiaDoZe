// Henrique Churkin Correia Alberton

using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.ValueObjects;
using AcademiaDoZe.Infrastructure.Repositories;

namespace AcademiaDoZe.Infrastructure.Tests;

public class ColaboradorInfrastructureTests : TestBase
{
    [Fact]
    public async Task Colaborador_LogradouroPorId_CpfJaExiste_Adicionar()
    {

        var logradouroId = 4;
        var repoLogradouroObterPorId = new LogradouroRepository(ConnectionString, DatabaseType);
        Logradouro? logradouro = await repoLogradouroObterPorId.ObterPorId(logradouroId);

        Arquivo arquivo = Arquivo.Criar(new byte[] { 1, 2, 3 });

        var _cpf = "12345678951";

        var repoColaboradorCpf = new ColaboradorRepository(ConnectionString, DatabaseType);

        var cpfExistente = await repoColaboradorCpf.CpfJaExiste(_cpf);
        Assert.False(cpfExistente, "CPF já existe no banco de dados."); 
        var colaborador = Colaborador.Criar(
            1,
        "zé",
        _cpf,
        new DateOnly(2002, 10, 09),
        "49999999999",
        "ze@com.br",
        logradouro!,
        "123",
        "complemento casa",
        "abcBolinhas",
        arquivo,
        new DateOnly(2024, 05, 04),
        EColaboradorTipo.Administrador,
        EColaboradorVinculo.CLT

        );

        var repoColaboradorAdicionar = new ColaboradorRepository(ConnectionString, DatabaseType);
        var colaboradorInserido = await repoColaboradorAdicionar.Adicionar(colaborador);
        Assert.NotNull(colaboradorInserido);
        Assert.True(colaboradorInserido.Id > 0);

    }

    [Fact]
    public async Task Colaborador_ObterPorCpf_Atualizar()
    {
        var _cpf = "12345678951";
        Arquivo arquivo = Arquivo.Criar(new byte[] { 1, 2, 3 });
        var repoColaboradorObterPorCpf = new ColaboradorRepository(ConnectionString, DatabaseType);
        var colaboradorExistente = await repoColaboradorObterPorCpf.ObterPorCpf(_cpf);
        Assert.NotNull(colaboradorExistente);

        var colaboradorAtualizado = Colaborador.Criar(
        colaboradorExistente.Id,
        "zé dos testes 123",
        colaboradorExistente.Cpf,
        colaboradorExistente.DataNascimento,
        colaboradorExistente.Telefone,
        colaboradorExistente.Email,
        colaboradorExistente.Endereco,
        colaboradorExistente.Numero,
        colaboradorExistente.Complemento,
        colaboradorExistente.Senha,
        arquivo,
        colaboradorExistente.DataAdmissao,
        colaboradorExistente.Tipo,
        colaboradorExistente.Vinculo
        );

        var idProperty = typeof(Entity).GetProperty("Id");

        idProperty?.SetValue(colaboradorAtualizado, colaboradorExistente.Id);

        var repoColaboradorAtualizar = new ColaboradorRepository(ConnectionString, DatabaseType);
        var resultadoAtualizacao = await repoColaboradorAtualizar.Atualizar(colaboradorAtualizado);
        Assert.NotNull(resultadoAtualizacao);

        Assert.Equal("zé dos testes 123", resultadoAtualizacao.Nome);

    }

    [Fact]
    public async Task Colaborador_ObterPorCpf_TrocarSenha()
    {
        var _cpf = "12345678951";
        Arquivo arquivo = Arquivo.Criar(new byte[] { 1, 2, 3 });
        var repoColaboradorObterPorCpf = new ColaboradorRepository(ConnectionString, DatabaseType);
        var colaboradorExistente = await repoColaboradorObterPorCpf.ObterPorCpf(_cpf);
        Assert.NotNull(colaboradorExistente);
        var novaSenha = "novaSenha123";
        var repoColaboradorTrocarSenha = new ColaboradorRepository(ConnectionString, DatabaseType);

        var resultadoTrocaSenha = await repoColaboradorTrocarSenha.TrocarSenha(colaboradorExistente.Id, novaSenha);
        Assert.True(resultadoTrocaSenha);

        var repoColaboradorObterPorId = new ColaboradorRepository(ConnectionString, DatabaseType);
        var colaboradorAtualizado = await repoColaboradorObterPorId.ObterPorId(colaboradorExistente.Id);
        Assert.NotNull(colaboradorAtualizado);
        Assert.Equal(novaSenha, colaboradorAtualizado.Senha);
    }
    [Fact]
    public async Task Colaborador_ObterPorCpf_Remover_ObterPorId()
    {
        var _cpf = "12345678951";
        var repoColaboradorObterPorCpf = new ColaboradorRepository(ConnectionString, DatabaseType);
        var colaboradorExistente = await repoColaboradorObterPorCpf.ObterPorCpf(_cpf);
        Assert.NotNull(colaboradorExistente);

        var repoColaboradorRemover = new ColaboradorRepository(ConnectionString, DatabaseType);
        var resultadoRemover = await repoColaboradorRemover.Remover(colaboradorExistente.Id);
        Assert.True(resultadoRemover);

        var repoColaboradorObterPorId = new ColaboradorRepository(ConnectionString, DatabaseType);
        var resultadoRemovido = await repoColaboradorObterPorId.ObterPorId(colaboradorExistente.Id);
        Assert.Null(resultadoRemovido);
    }
    [Fact]
    public async Task Colaborador_ObterTodos()
    {
        var repoColaboradorRepository = new ColaboradorRepository(ConnectionString, DatabaseType);
        var resultado = await repoColaboradorRepository.ObterTodos();
        Assert.NotNull(resultado);
    }
}