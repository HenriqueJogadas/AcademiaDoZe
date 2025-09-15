// Rayssa Romaniuk

using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AcademiaDoZe.Application.Tests;

public class MatriculaApplicationTests
{
    const string connectionString = "Server=localhost;Database=db_academia_do_ze;User Id=root;Password=Rayssa;";
    const EAppDatabaseType databaseType = EAppDatabaseType.MySql;

    [Fact(Timeout = 60000)]
    public async Task MatriculaService_Integracao_Adicionar_Obter_Atualizar_Remover()
    {
        var services = DependencyInjection.ConfigureServices(connectionString, databaseType);
        var provider = DependencyInjection.BuildServiceProvider(services);

        var alunoService = provider.GetRequiredService<IAlunoService>();
        var logradouroService = provider.GetRequiredService<ILogradouroService>();
        var matriculaService = provider.GetRequiredService<IMatriculaService>();

        var _cpf = GerarCpfFake();

        var logradouro = await logradouroService.ObterPorIdAsync(5);
        Assert.NotNull(logradouro);
        Assert.Equal(5, logradouro!.Id);

        var caminhoFoto = Path.Combine("..", "..", "..", "foto_teste.png");
        ArquivoDTO foto = new();
        if (File.Exists(caminhoFoto))
        {
            foto.Conteudo = await File.ReadAllBytesAsync(caminhoFoto);
        }
        else
        {
            foto.Conteudo = null;
            Assert.Fail("Foto de teste não encontrada.");
        }

        var dtoAluno = new AlunoDTO
        {
            Nome = "Aluno Teste",
            Cpf = _cpf,
            DataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)),
            Telefone = "11999999999",
            Email = "aluno@teste.com",
            Endereco = logradouro,
            Numero = "100",
            Complemento = "Apto 1",
            Senha = "Senha@1",
            Foto = foto
        };

        AlunoDTO? alunoCriado = null;
        MatriculaDTO? matriculaCriada = null;

        try
        {
            alunoCriado = await alunoService.AdicionarAsync(dtoAluno);
            Assert.NotNull(alunoCriado);
            Assert.True(alunoCriado!.Id > 0);

            var laudo = new ArquivoDTO { Conteudo = new byte[] { 1, 2, 3 } };

            var dtoMatricula = new MatriculaDTO
            {
                Id = 1000,
                AlunoMatricula = alunoCriado,
                Plano = Enums.EAppMatriculaPlano.Mensal,
                DataInicio = new DateOnly(2025, 05, 22),
                DataFim = new DateOnly(2025, 06, 22),
                Objetivo = "Emagrecer",
                RestricoesMedicas = Enums.EAppMatriculaRestricoes.Alergias,
                ObservacoesRestricoes = "Nenhuma",
                LaudoMedico = laudo
            };

            matriculaCriada = await matriculaService.AdicionarAsync(dtoMatricula);
            Assert.NotNull(matriculaCriada);
            Assert.True(matriculaCriada!.Id > 0);

            var obtida = await matriculaService.ObterPorIdAsync(matriculaCriada.Id);
            Assert.NotNull(obtida);
            Assert.Equal("Emagrecer", obtida!.Objetivo);

            var atualizarDto = new MatriculaDTO
            {
                Id = obtida.Id,
                AlunoMatricula = obtida.AlunoMatricula,
                Plano = Enums.EAppMatriculaPlano.Trimestral,
                DataInicio = obtida.DataInicio,
                DataFim = obtida.DataFim,
                Objetivo = "Hipertrofia",
                RestricoesMedicas = Enums.EAppMatriculaRestricoes.Diabetes,
                ObservacoesRestricoes = "Nenhuma",
                LaudoMedico = obtida.LaudoMedico 
            };

            var atualizado = await matriculaService.AtualizarAsync(atualizarDto);
            Assert.NotNull(atualizado);
            Assert.Equal("Hipertrofia", atualizado!.Objetivo);

            var removido = await matriculaService.RemoverAsync(atualizado.Id);
            Assert.True(removido);

            var aposRemocao = await matriculaService.ObterPorIdAsync(atualizado.Id);
            Assert.Null(aposRemocao);
        }
        finally
        {
            if (matriculaCriada is not null)
            {
                try { await matriculaService.RemoverAsync(matriculaCriada.Id); } catch { }
            }

            if (alunoCriado is not null)
            {
                try { await alunoService.RemoverAsync(alunoCriado.Id); } catch { }
            }
        }
    }

    private static string GerarCpfFake()
    {
        var rnd = new Random();
        return string.Concat(Enumerable.Range(0, 11).Select(_ => rnd.Next(0, 10).ToString()));
    }
}
