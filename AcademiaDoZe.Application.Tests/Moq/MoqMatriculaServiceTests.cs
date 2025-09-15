// Rayssa Romaniuk

using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using Moq;

namespace AcademiaDoZe.Application.Tests;

public class MoqMatriculaServiceTests
{
    private readonly Mock<IMatriculaService> _matriculaServiceMock;
    private readonly IMatriculaService _matriculaService;
    public MoqMatriculaServiceTests()
    {
        _matriculaServiceMock = new Mock<IMatriculaService>();
        _matriculaService = _matriculaServiceMock.Object;
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

    private MatriculaDTO CriarMatriculaPadrao(int id = 1)
    {
        return new MatriculaDTO
        {
            Id = id,
            AlunoMatricula = CriarAlunoPadrao(),
            Plano = Enums.EAppMatriculaPlano.Mensal,
            DataInicio = new DateOnly(2025, 05, 22),
            DataFim = new DateOnly(2025, 06, 22),
            Objetivo = "Hipertrofia",
            RestricoesMedicas = Enums.EAppMatriculaRestricoes.Alergias,
            ObservacoesRestricoes = "Nenhuma"
        };
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarMatricula_QuandoExistir()
    {
        var matriculaId = 1;
        var logradouroDto = CriarMatriculaPadrao(matriculaId);

        _matriculaServiceMock.Setup(s => s.ObterPorIdAsync(matriculaId)).ReturnsAsync(logradouroDto);

        var result = await _matriculaService.ObterPorIdAsync(matriculaId);


        Assert.NotNull(result);
        Assert.Equal(logradouroDto.AlunoMatricula, result.AlunoMatricula);
        _matriculaServiceMock.Verify(s => s.ObterPorIdAsync(matriculaId), Times.Once);
    }
    [Fact]
    public async Task ObterTodosAsync_DeveRetornaMatriculas_QuandoExistirem()
    {
        var matriculasDto = new List<MatriculaDTO>

        {
        CriarMatriculaPadrao(1),
        CriarMatriculaPadrao(2)
        };
        _matriculaServiceMock.Setup(s => s.ObterTodasAsync()).ReturnsAsync(matriculasDto);

        var result = await _matriculaService.ObterTodasAsync();


        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        _matriculaServiceMock.Verify(s => s.ObterTodasAsync(), Times.Once);
    }
    [Fact]
    public async Task ObterTodosAsync_DeveRetornarListaVazia_QuandoNaoHouverMatriculas()
    {
        _matriculaServiceMock.Setup(s => s.ObterTodasAsync()).ReturnsAsync(new List<MatriculaDTO>());
        var result = await _matriculaService.ObterTodasAsync();

        Assert.NotNull(result);
        Assert.Empty(result);

        _matriculaServiceMock.Verify(s => s.ObterTodasAsync(), Times.Once);
    }
    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarNull_QuandoNaoExistir()
    {
        var id = 1;
        _matriculaServiceMock.Setup(s => s.ObterPorIdAsync(id)).ReturnsAsync((MatriculaDTO)null!);

        var result = await _matriculaService.ObterPorIdAsync(id);

        Assert.Null(result);

        _matriculaServiceMock.Verify(s => s.ObterPorIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task AdicionarAsync_DeveAdicionarMatricula_QuandoDadosValidos()
    {
        var matriculaDto = CriarMatriculaPadrao(1);
        var matriculaCriado = new MatriculaDTO
        {
            Id = 1,
            AlunoMatricula = matriculaDto.AlunoMatricula    ,
            DataInicio = matriculaDto.DataInicio,
            DataFim = matriculaDto.DataFim,
            Objetivo = matriculaDto.Objetivo,
            Plano = matriculaDto.Plano,
            RestricoesMedicas = matriculaDto.RestricoesMedicas,
            LaudoMedico = matriculaDto.LaudoMedico,
            ObservacoesRestricoes = matriculaDto.ObservacoesRestricoes
        };
        _matriculaServiceMock.Setup(s => s.AdicionarAsync(It.IsAny<MatriculaDTO>())).ReturnsAsync(matriculaCriado);
        var result = await _matriculaService.AdicionarAsync(matriculaDto);
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        _matriculaServiceMock.Verify(s => s.AdicionarAsync(It.IsAny<MatriculaDTO>()), Times.Once);
    }
    [Fact]
    public async Task ObterIdCepAsync_DeveRetornarMatricula_QuandoExistir()
    {
        var id = 1;
        var matriculaDto = CriarMatriculaPadrao(id);
        _matriculaServiceMock.Setup(s => s.ObterPorIdAsync(id)).ReturnsAsync(matriculaDto);
        var result = await _matriculaService.ObterPorIdAsync(id);
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        _matriculaServiceMock.Verify(s => s.ObterPorIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task AtualizarAsync_DeveAtualizarMatricula_QuandoDadosValidos()
    {
        var matriculaId = 1;
        var matriculaExistente = CriarMatriculaPadrao(matriculaId);
        var matriculaAtualizado = new MatriculaDTO
        {
            Id = matriculaId,
            AlunoMatricula = matriculaExistente.AlunoMatricula,
            DataInicio = matriculaExistente.DataInicio,
            DataFim = matriculaExistente.DataFim,
            Objetivo = "Emagrecer", 
            Plano = matriculaExistente.Plano,
            RestricoesMedicas = matriculaExistente.RestricoesMedicas,
            LaudoMedico = matriculaExistente.LaudoMedico,
            ObservacoesRestricoes = matriculaExistente.ObservacoesRestricoes
        };
        _matriculaServiceMock.Setup(s => s.AtualizarAsync(It.IsAny<MatriculaDTO>())).ReturnsAsync(matriculaAtualizado);
        var result = await _matriculaService.AtualizarAsync(matriculaAtualizado);
        Assert.NotNull(result);
        Assert.Equal("Emagrecer", result.Objetivo);
        _matriculaServiceMock.Verify(s => s.AtualizarAsync(It.IsAny<MatriculaDTO>()), Times.Once);
    }
    [Fact]
    public async Task RemoverAsync_DeveRemoverMatricula_QuandoExistir()
    {
        var matriculaId = 1;
        _matriculaServiceMock.Setup(s => s.RemoverAsync(matriculaId)).ReturnsAsync(true);
        var result = await _matriculaService.RemoverAsync(matriculaId);
        Assert.True(result);
        _matriculaServiceMock.Verify(s => s.RemoverAsync(matriculaId), Times.Once);
    }
}