// Rayssa Romaniuk

using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using Moq;

namespace AcademiaDoZe.Application.Tests;

public class MoqLogradouroServiceTests
{
    private readonly Mock<ILogradouroService> _logradouroServiceMock;
    private readonly ILogradouroService _logradouroService;
    public MoqLogradouroServiceTests()
    {
        _logradouroServiceMock = new Mock<ILogradouroService>();
        _logradouroService = _logradouroServiceMock.Object;
    }
    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarLogradouro_QuandoExistir()
    {
        var logradouroId = 1;
        var logradouroDto = new LogradouroDTO { Id = logradouroId, Cep = "12345678", Nome = "Rua Teste", Bairro = "Centro", Cidade = "São Paulo", Estado = "SP", Pais = "Brasil" };

        _logradouroServiceMock.Setup(s => s.ObterPorIdAsync(logradouroId)).ReturnsAsync(logradouroDto);
        var result = await _logradouroService.ObterPorIdAsync(logradouroId);


        Assert.NotNull(result);
        Assert.Equal(logradouroDto.Cep, result.Cep);
        _logradouroServiceMock.Verify(s => s.ObterPorIdAsync(logradouroId), Times.Once);
    }
    [Fact]
    public async Task ObterTodosAsync_DeveRetornarLogradouros_QuandoExistirem()
    {

        var logradourosDto = new List<LogradouroDTO>

{
new LogradouroDTO {Id = 1, Cep = "12345678", Nome = "Rua Teste 1", Bairro = "Centro", Cidade = "São Paulo", Estado = "SP", Pais = "Brasil" },
new LogradouroDTO {Id = 2, Cep = "87654321", Nome = "Rua Teste 2", Bairro = "Centro", Cidade = "Rio de Janeiro", Estado = "RJ", Pais = "Brasil"}
};
        _logradouroServiceMock.Setup(s => s.ObterTodosAsync()).ReturnsAsync(logradourosDto);

        var result = await _logradouroService.ObterTodosAsync();


        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        _logradouroServiceMock.Verify(s => s.ObterTodosAsync(), Times.Once);
    }
    [Fact]
    public async Task ObterTodosAsync_DeveRetornarListaVazia_QuandoNaoHouverLogradouros()
    {
        _logradouroServiceMock.Setup(s => s.ObterTodosAsync()).ReturnsAsync(new List<LogradouroDTO>());

        var result = await _logradouroService.ObterTodosAsync();


        Assert.NotNull(result);
        Assert.Empty(result);

        _logradouroServiceMock.Verify(s => s.ObterTodosAsync(), Times.Once);
    }
    [Fact]
    public async Task ObterPorCepAsync_DeveRetornarNull_QuandoNaoExistir()
    {

        var cep = "00000000";

        _logradouroServiceMock.Setup(s => s.ObterPorCepAsync(cep)).ReturnsAsync((LogradouroDTO)null!);
        var result = await _logradouroService.ObterPorCepAsync(cep);

        Assert.Null(result);

        _logradouroServiceMock.Verify(s => s.ObterPorCepAsync(cep), Times.Once);
    }

    [Fact]
    public async Task AdicionarAsync_DeveAdicionarLogradouro_QuandoDadosValidos()
    {
        var logradouroDto = new LogradouroDTO { Cep = "12345678", Nome = "Nova Rua", Bairro = "Centro", Cidade = "São Paulo", Estado = "SP", Pais = "Brasil" };
        var logradouroCriado = new LogradouroDTO
        {
            Id = 1,
            Cep = logradouroDto.Cep,
            Nome = logradouroDto.Nome,
            Bairro = logradouroDto.Bairro,
            Cidade = logradouroDto.Cidade,
            Estado = logradouroDto.Estado,
            Pais = logradouroDto.Pais
        };
        _logradouroServiceMock.Setup(s => s.AdicionarAsync(It.IsAny<LogradouroDTO>())).ReturnsAsync(logradouroCriado);
        var result = await _logradouroService.AdicionarAsync(logradouroDto);
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        _logradouroServiceMock.Verify(s => s.AdicionarAsync(It.IsAny<LogradouroDTO>()), Times.Once);
    }
    [Fact]
    public async Task ObterPorCepAsync_DeveRetornarLogradouro_QuandoExistir()
    {
        var cep = "12345678";
        var logradouroDto = new LogradouroDTO { Id = 1, Cep = cep, Nome = "Rua do CEP", Bairro = "Centro", Cidade = "São Paulo", Estado = "SP", Pais = "Brasil" };
        _logradouroServiceMock.Setup(s => s.ObterPorCepAsync(cep)).ReturnsAsync(logradouroDto);
        var result = await _logradouroService.ObterPorCepAsync(cep);
        Assert.NotNull(result);
        Assert.Equal(cep, result.Cep);
        _logradouroServiceMock.Verify(s => s.ObterPorCepAsync(cep), Times.Once);
    }
    [Fact]
    public async Task ObterPorCidadeAsync_DeveRetornarListaVazia_QuandoNaoHouverLogradourosNaCidade()
    {
        var cidade = "Cidade Inexistente";
        _logradouroServiceMock.Setup(s => s.ObterPorCidadeAsync(cidade)).ReturnsAsync(new List<LogradouroDTO>());
        var result = await _logradouroService.ObterPorCidadeAsync(cidade);
        Assert.NotNull(result);
        Assert.Empty(result);
        _logradouroServiceMock.Verify(s => s.ObterPorCidadeAsync(cidade), Times.Once);
    }

    [Fact]
    public async Task AtualizarAsync_DeveAtualizarLogradouro_QuandoDadosValidos()
    {
        var logradouroId = 1;
        var logradouroExistente = new LogradouroDTO { Id = logradouroId, Cep = "12345678", Nome = "Rua Antiga", Bairro = "Centro", Cidade = "São Paulo", Estado = "SP", Pais = "Brasil" };
        var logradouroAtualizado = new LogradouroDTO { Id = logradouroId, Cep = "12345678", Nome = "Rua Atualizada", Bairro = "Centro", Cidade = "São Paulo", Estado = "SP", Pais = "Brasil" };
        _logradouroServiceMock.Setup(s => s.AtualizarAsync(It.IsAny<LogradouroDTO>())).ReturnsAsync(logradouroAtualizado);
        var result = await _logradouroService.AtualizarAsync(logradouroAtualizado);
        Assert.NotNull(result);
        Assert.Equal("Rua Atualizada", result.Nome);
        _logradouroServiceMock.Verify(s => s.AtualizarAsync(It.IsAny<LogradouroDTO>()), Times.Once);
    }
    [Fact]
    public async Task RemoverAsync_DeveRemoverLogradouro_QuandoExistir()
    {
        var logradouroId = 1;
        _logradouroServiceMock.Setup(s => s.RemoverAsync(logradouroId)).ReturnsAsync(true);
        var result = await _logradouroService.RemoverAsync(logradouroId);
        Assert.True(result);
        _logradouroServiceMock.Verify(s => s.RemoverAsync(logradouroId), Times.Once);
    }
    [Fact]
    public async Task ObterPorCidadeAsync_DeveRetornarLogradouros_QuandoExistirem()
    {
        var cidade = "São Paulo";
        var logradourosDto = new List<LogradouroDTO>
{
new LogradouroDTO {Id = 1, Cep = "12345678", Nome = "Rua Teste 1", Bairro = "Centro", Cidade = cidade, Estado = "SP", Pais = "Brasil" },
new LogradouroDTO {Id = 2, Cep = "87654321", Nome = "Rua Teste 2", Bairro = "Bela Vista", Cidade = cidade, Estado = "SP", Pais = "Brasil" }
};
        _logradouroServiceMock.Setup(s => s.ObterPorCidadeAsync(cidade)).ReturnsAsync(logradourosDto);
        var result = await _logradouroService.ObterPorCidadeAsync(cidade);
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, l => Assert.Equal(cidade, l.Cidade));
        _logradouroServiceMock.Verify(s => s.ObterPorCidadeAsync(cidade), Times.Once);
    }
}