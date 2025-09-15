// Henrique Churkin Correia Alberton

using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Tests;

public class ColaboradorDomainTests
{
    private Logradouro GetValidLogradouro() => Logradouro.Criar(1, "12345678", "Rua A", "Centro", "Cidade", "SP", "Brasil");
    private Arquivo GetValidArquivo() => Arquivo.Criar(new byte[1]);

    [Fact]
    public void CriarColaborador_ComDadosValidos_DeveCriarObjeto()
    {
        var id = 1;  var nome = "João da Silva"; var cpf = "12345678901"; var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)); var telefone = "11999999999";
        var email = "joao@email.com"; var logradouro = GetValidLogradouro(); var numero = "123"; var complemento = "Apto 1"; var senha = "Senha@1"; var foto = GetValidArquivo();
        var dataAdmissao = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)); 
        var tipo = Enums.EColaboradorTipo.Administrador; var vinculo = Enums.EColaboradorVinculo.CLT;
        var colaborador = Colaborador.Criar(id, nome, cpf, dataNascimento, telefone, email, logradouro, numero, complemento, senha, foto, dataAdmissao, tipo, vinculo);
        Assert.NotNull(colaborador);
    }

    [Fact]
    public void CriarColaborador_ComNomeVazio_DeveLancarExcecao()
    {
        var exception = Assert.Throws<DomainException>(() => Colaborador.Criar(
            1,
            "", 
            "111.111.111-11",
            DateOnly.MinValue,
            "11999999999",
            "Teste@gmail.com",
            GetValidLogradouro(),
            "44",
            "casa",
            "senha123",
            GetValidArquivo(),
            DateOnly.MaxValue,
            Enums.EColaboradorTipo.Administrador,
            Enums.EColaboradorVinculo.Estagio
        ));
        Assert.Equal("NOME_OBRIGATORIO", exception.Message);
    }
}