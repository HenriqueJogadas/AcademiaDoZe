// Rayssa Romaniuk

using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using Moq;

namespace AcademiaDoZe.Application.Tests;

public class MoqAlunoServiceTests
{
    private readonly Mock<IAlunoService> _alunoServiceMock;
    private readonly IAlunoService _alunoService;
    public MoqAlunoServiceTests()
    {
        _alunoServiceMock = new Mock<IAlunoService>();
        _alunoService = _alunoServiceMock.Object;
    }
    private AlunoDTO CriarAlunoPadrao(int id = 1)
    {
        return new AlunoDTO
        {
            Id = id,
            Nome = "Aluno Teste",
            Cpf = "12345678901",
            DataNascimento = DateOnly.FromDateTime(DateTime.Now.AddYears(-30)),
            Telefone = "11999999999",
            Email = "aluno@teste.com",
            Endereco = new LogradouroDTO { Id = 1, Cep = "12345678", Nome = "Rua Teste", Bairro = "Centro", Cidade = "São Paulo", Estado = "SP", Pais = "Brasil" },
            Numero = "100",
            Complemento = "Apto 101",
            Senha = "Senha@123"
        };
    }
    [Theory]
    [InlineData("12345678901", null, true)]
    [InlineData("12345678901", 1, false)]
    [InlineData("99999999999", null, false)]
    public async Task CpfJaExisteAsync_DeveRetornarResultadoCorreto(string cpf, int? id, bool resultadoEsperado)
    {
        _alunoServiceMock.Setup(s => s.CpfJaExisteAsync(cpf, id)).ReturnsAsync(resultadoEsperado);

        var resultado = await _alunoService.CpfJaExisteAsync(cpf, id);


        Assert.Equal(resultadoEsperado, resultado);

        _alunoServiceMock.Verify(s => s.CpfJaExisteAsync(cpf, id), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarAluno_QuandoExistir()
    {
        var alunoId = 1;
        var alunoDto = CriarAlunoPadrao(alunoId);
        _alunoServiceMock.Setup(s => s.ObterPorIdAsync(alunoId)).ReturnsAsync(alunoDto);
        var result = await _alunoService.ObterPorIdAsync(alunoId);
        Assert.NotNull(result);
        Assert.Equal(alunoDto.Cpf, result.Cpf);
        _alunoServiceMock.Verify(s => s.ObterPorIdAsync(alunoId), Times.Once);
    }
    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarNull_QuandoNaoExistir()
    {
        var alunoId = 999;
        _alunoServiceMock.Setup(s => s.ObterPorIdAsync(alunoId)).ReturnsAsync((AlunoDTO)null!);
        var result = await _alunoService.ObterPorIdAsync(alunoId);
        Assert.Null(result);
        _alunoServiceMock.Verify(s => s.ObterPorIdAsync(alunoId), Times.Once);
    }
    [Fact]
    public async Task ObterTodosAsync_DeveRetornarAlunos_QuandoExistirem()
    {
        var alunosDto = new List<AlunoDTO>
{
CriarAlunoPadrao(1),
CriarAlunoPadrao(2)
};
        _alunoServiceMock.Setup(s => s.ObterTodosAsync()).ReturnsAsync(alunosDto);
        var result = await _alunoService.ObterTodosAsync();
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _alunoServiceMock.Verify(s => s.ObterTodosAsync(), Times.Once);
    }

    [Fact]
    public async Task ObterTodosAsync_DeveRetornarListaVazia_QuandoNaoHouverAlunos()
    {
        _alunoServiceMock.Setup(s => s.ObterTodosAsync()).ReturnsAsync(new List<AlunoDTO>());
        var result = await _alunoService.ObterTodosAsync();
        Assert.NotNull(result);
        Assert.Empty(result);
        _alunoServiceMock.Verify(s => s.ObterTodosAsync(), Times.Once);
    }
    [Fact]
    public async Task AdicionarAsync_DeveAdicionarAluno_QuandoDadosValidos()
    {
        var alunoDto = CriarAlunoPadrao(0); 
        var alunoCriado = CriarAlunoPadrao(1);
        _alunoServiceMock.Setup(s => s.AdicionarAsync(It.IsAny<AlunoDTO>())).ReturnsAsync(alunoCriado);
        var result = await _alunoService.AdicionarAsync(alunoDto);
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        _alunoServiceMock.Verify(s => s.AdicionarAsync(It.IsAny<AlunoDTO>()), Times.Once);
    }
    [Fact]
    public async Task AtualizarAsync_DeveAtualizaAluno_QuandoDadosValidos()
    {
        var alunoId = 1;
        var alunoAtualizado = CriarAlunoPadrao(alunoId);
        alunoAtualizado.Nome = "Nome Atualizado";
        _alunoServiceMock.Setup(s => s.AtualizarAsync(It.IsAny<AlunoDTO>())).ReturnsAsync(alunoAtualizado);
        var result = await _alunoService.AtualizarAsync(alunoAtualizado);
        Assert.NotNull(result);
        Assert.Equal("Nome Atualizado", result.Nome);
        _alunoServiceMock.Verify(s => s.AtualizarAsync(It.IsAny<AlunoDTO>()), Times.Once);
    }

    [Fact]
    public async Task RemoverAsync_DeveRemoverAluno_QuandoExistir()
    {

        var alunoId = 1;
        _alunoServiceMock.Setup(s => s.RemoverAsync(alunoId)).ReturnsAsync(true);
        var result = await _alunoService.RemoverAsync(alunoId);
        Assert.True(result);
        _alunoServiceMock.Verify(s => s.RemoverAsync(alunoId), Times.Once);
    }
    [Fact]
    public async Task RemoverAsync_DeveRetornarFalse_QuandoNaoExistir()
    {
        var alunoId = 999;

        _alunoServiceMock.Setup(s => s.RemoverAsync(alunoId)).ReturnsAsync(false);
        var result = await _alunoService.RemoverAsync(alunoId);
        Assert.False(result);

        _alunoServiceMock.Verify(s => s.RemoverAsync(alunoId), Times.Once);
    }
    [Fact]
    public async Task ObterPorCpfAsync_DeveRetornarAluno_QuandoExistir()
    {

        var cpf = "12345678901";
        var alunoDto = CriarAlunoPadrao(1);

        alunoDto.Cpf = cpf;
        _alunoServiceMock.Setup(s => s.ObterPorCpfAsync(cpf)).ReturnsAsync(alunoDto);
        var result = await _alunoService.ObterPorCpfAsync(cpf);

        Assert.NotNull(result);
        Assert.Equal(cpf, result.Cpf);

        _alunoServiceMock.Verify(s => s.ObterPorCpfAsync(cpf), Times.Once);
    }

    [Fact]
    public async Task ObterPorCpfAsync_DeveRetornarNull_QuandoNaoExistir()
    {
        var cpf = "99999999999";
        _alunoServiceMock.Setup(s => s.ObterPorCpfAsync(cpf)).ReturnsAsync((AlunoDTO)null!);
        var result = await _alunoService.ObterPorCpfAsync(cpf);
        Assert.Null(result);
        _alunoServiceMock.Verify(s => s.ObterPorCpfAsync(cpf), Times.Once);
    }
    [Fact]
    public async Task TrocarSenhaAsync_DeveRetornarTrue_QuandoSucesso()
    {
        var alunoId = 1;
        var novaSenha = "NovaSenha@123";
        _alunoServiceMock.Setup(s => s.TrocarSenhaAsync(alunoId, novaSenha)).ReturnsAsync(true);
        var resultado = await _alunoService.TrocarSenhaAsync(alunoId, novaSenha);
        Assert.True(resultado);
        _alunoServiceMock.Verify(s => s.TrocarSenhaAsync(alunoId, novaSenha), Times.Once);
    }
}