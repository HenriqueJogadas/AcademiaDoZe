// Rayssa Romaniuk
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AcademiaDoZe.Application.Tests;

public class LogradouroApplicationTests
{
    const string connectionString = "Server=localhost;Database=db_academia_do_ze;User Id=root;Password=Rayssa;";
    const EAppDatabaseType databaseType = EAppDatabaseType.MySql;
    [Fact(Timeout = 60000)]
    public async Task LogradouroService_Integracao_Adicionar_Obter_Atualizar_Remover()
    {
        var services = DependencyInjection.ConfigureServices(connectionString, databaseType);

        var provider = DependencyInjection.BuildServiceProvider(services);

        var logradouroService = provider.GetRequiredService<ILogradouroService>();

        var _cep = "99500001";

        var dto = new LogradouroDTO
        {
            Cep = _cep,
            Nome = "Rua Teste",
            Bairro = "Centro",
            Cidade = "Cidade X",
            Estado = "SP",
            Pais = "Brasil"
        };
        LogradouroDTO? criado = null;

        try
        {
            criado = await logradouroService.AdicionarAsync(dto);
            Assert.NotNull(criado);
            Assert.True(criado!.Id > 0);
            var obtido = await logradouroService.ObterPorCepAsync(_cep);
            Assert.NotNull(obtido);
            Assert.Equal("Rua Teste", obtido!.Nome);


            var atualizarDto = new LogradouroDTO

            {
                Id = criado.Id,
                Cep = criado.Cep,
                Nome = "Rua Atualizada",
                Bairro = criado.Bairro,
                Cidade = criado.Cidade,
                Estado = "RJ",
                Pais = criado.Pais
            };
            var atualizado = await logradouroService.AtualizarAsync(atualizarDto);

            Assert.NotNull(atualizado);
            Assert.Equal("Rua Atualizada", atualizado.Nome);
            Assert.Equal("RJ", atualizado.Estado);

            var removido = await logradouroService.RemoverAsync(criado.Id);
            Assert.True(removido);
            var aposRemocao = await logradouroService.ObterPorIdAsync(criado.Id);
            Assert.Null(aposRemocao);

        }
        finally
        {
            if (criado is not null)

            {
                try { await logradouroService.RemoverAsync(criado.Id); } catch { }
            }
        }
    }
}