// Rayssa Romaniuk
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AcademiaDoZe.Application.Tests;

public class AlunoApplicationTests
{
    const string connectionString = "Server=localhost;Database=db_academia_do_ze;User Id=root;Password=Rayssa;";
    const EAppDatabaseType databaseType = EAppDatabaseType.MySql;
    [Fact(Timeout = 60000)]
    public async Task AlunoService_Integracao_Adicionar_Obter_Atualizar_Remover()
    {
        var services = DependencyInjection.ConfigureServices(connectionString, databaseType);
        var provider = DependencyInjection.BuildServiceProvider(services);
        var alunoService = provider.GetRequiredService<IAlunoService>();
        var logradouroService = provider.GetRequiredService<ILogradouroService>();
        var _cpf = GerarCpfFake();
        var logradouro = await logradouroService.ObterPorIdAsync(5);
        Assert.NotNull(logradouro);
        Assert.Equal(5, logradouro!.Id);
        var caminhoFoto = Path.Combine("..", "..", "..", "foto_teste.png");
        ArquivoDTO foto = new();

        if (File.Exists(caminhoFoto)) { foto.Conteudo = await File.ReadAllBytesAsync(caminhoFoto); }

        else { foto.Conteudo = null; Assert.Fail("Foto de teste não encontrada."); }
        var dto = new AlunoDTO
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
        AlunoDTO? criado = null;

        try
        {
            criado = await alunoService.AdicionarAsync(dto);
            Assert.NotNull(criado);
            Assert.True(criado!.Id > 0);
            Assert.Equal(_cpf, criado.Cpf);
            var obtido = await alunoService.ObterPorCpfAsync(criado.Cpf);
            Assert.NotNull(obtido);
            Assert.Equal(criado.Id, obtido!.Id);
            Assert.Equal(_cpf, obtido.Cpf);
            var atualizar = new AlunoDTO
            {
                Id = criado.Id,
                Nome = "Aluno Atualizado",
                Cpf = criado.Cpf,
                DataNascimento = criado.DataNascimento,
                Telefone = criado.Telefone,
                Email = criado.Email,
                Endereco = criado.Endereco,
                Numero = criado.Numero,
                Complemento = criado.Complemento,
                Senha = criado.Senha,
                Foto = criado.Foto
            };
            var atualizado = await alunoService.AtualizarAsync(atualizar);
            Assert.NotNull(atualizado);
            Assert.Equal("Aluno Atualizado", atualizado.Nome);
            var removido = await alunoService.RemoverAsync(criado.Id);
            Assert.True(removido);
            var aposRemocao = await alunoService.ObterPorIdAsync(criado.Id);
            Assert.Null(aposRemocao);
        }
        finally
        {
            if (criado is not null)
            {
                try { await alunoService.RemoverAsync(criado.Id); } catch { }
            }
        }
    }
    private static string GerarCpfFake()
    {
        var rnd = new Random();
        return string.Concat(Enumerable.Range(0, 11).Select(_ => rnd.Next(0, 10).ToString()));
    }
}