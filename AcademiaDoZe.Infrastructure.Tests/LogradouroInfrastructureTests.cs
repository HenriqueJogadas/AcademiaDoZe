// Henrique Churkin Correia Alberton

using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Infrastructure.Repositories;

namespace AcademiaDoZe.Infrastructure.Tests;

public class LogradouroInfrastructureTests : TestBase
{
    [Fact]
    public async Task Logradouro_Adicionar()
    {
        var _cep = "12345698";

        var logradouro = Logradouro.Criar(8, _cep, "Rua dos Testes", "Bairro Teste", "Cidade Teste", "TS", "Pais teste");

        var repoLogradouroAdd = new LogradouroRepository(ConnectionString, DatabaseType);
        var logradouroInserido = await repoLogradouroAdd.Adicionar(logradouro);
        Assert.NotNull(logradouroInserido);
        Assert.True(logradouroInserido.Id > 0);

    }
    [Fact]
    public async Task Logradouro_ObterPorCep_Atualizar()
    {
        var _cep = "12345698";

        var repoLogradouroBuscaCep = new LogradouroRepository(ConnectionString, DatabaseType);

        var logradouroPorCep = await repoLogradouroBuscaCep.ObterPorCep(_cep);
        Assert.NotNull(logradouroPorCep);

        var logradouroAtualizado = Logradouro.Criar(2, _cep, "Rua Atualizada", "Bairro Atualizado", "Cidade Atualizada", "AT", "Pais atualizado");


        var idProperty = typeof(Entity).GetProperty("Id");

        idProperty?.SetValue(logradouroAtualizado, logradouroPorCep.Id);
        var repoLogradouroEdit = new LogradouroRepository(ConnectionString, DatabaseType);
        var resultadoAtualizacao = await repoLogradouroEdit.Atualizar(logradouroAtualizado);
        Assert.NotNull(resultadoAtualizacao);

        Assert.Equal("AT", resultadoAtualizacao.Estado);
        Assert.Equal("Rua Atualizada", resultadoAtualizacao.Nome);

    }

    [Fact]
    public async Task Logradouro_ObterPorCep_Remover_ObterPorId()
    {
        var _cep = "12345698";

        var repoLogradouroBuscaCep = new LogradouroRepository(ConnectionString, DatabaseType);

        var logradouroPorCep = await repoLogradouroBuscaCep.ObterPorCep(_cep);
        Assert.NotNull(logradouroPorCep);

        var repoLogradouroDel = new LogradouroRepository(ConnectionString, DatabaseType);
        var resultadoRemocao = await repoLogradouroDel.Remover(logradouroPorCep.Id);
        Assert.True(resultadoRemocao);

        var repoLogradouroPorId = new LogradouroRepository(ConnectionString, DatabaseType);
        var logradouroRemovido = await repoLogradouroPorId.ObterPorId(logradouroPorCep.Id);
        Assert.Null(logradouroRemovido);
    }
    [Fact]
    public async Task Logradouro_ObterPorCidade()
    {
        var cidadeExistente = "Lages";

        var repoLogradouroPorCidade = new LogradouroRepository(ConnectionString, DatabaseType);
        var resultados = await repoLogradouroPorCidade.ObterPorCidade(cidadeExistente);
        Assert.NotNull(resultados);
        Assert.NotEmpty(resultados);

    }
    [Fact]
    public async Task Logradouro_ObterTodos()
    {
        var repoLogradouroTodos = new LogradouroRepository(ConnectionString, DatabaseType);

        var resultado = await repoLogradouroTodos.ObterTodos();
        Assert.NotNull(resultado);
    }
}