// Henrique Churkin Correia Alberton

using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Tests;

public class AlunoDomainTests
{
    private Logradouro GetValidLogradouro() => Logradouro.Criar(6, "12345678", "Rua A", "Centro", "Cidade", "SP", "Brasil");
    private Arquivo GetValidArquivo() => Arquivo.Criar(new byte[1]);
    [Fact] 
    public void CriarAluno_ComDadosValidos_DeveCriarObjeto() 
    {
        var id = 1;  var nome = "João da Silva"; var cpf = "12345678901"; var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)); var telefone = "11999999999";
        var email = "joao@email.com"; var logradouro = GetValidLogradouro(); var numero = "123"; var complemento = "Apto 1"; var senha = "Senha@1"; var foto = GetValidArquivo();
        var aluno = Aluno.Criar(id, nome, cpf, dataNascimento, telefone, email, logradouro, numero, complemento, senha, foto);
        Assert.NotNull(aluno);
    }
    [Fact]
    public void CriarAluno_ComNomeVazio_DeveLancarExcecao()
    {
        var id = 1;  var cpf = "12345678901"; var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)); var telefone = "11999999999";
        var email = "joao@email.com"; var logradouro = GetValidLogradouro(); var numero = "123"; var complemento = "Apto 1"; var senha = "Senha@123"; var foto = GetValidArquivo();
        var ex = Assert.Throws<DomainException>(() =>
        Aluno.Criar(
        id, 
        "",
        cpf,
        dataNascimento,
        telefone,
        email,
        logradouro,
        numero,
        complemento,
        senha,
        foto
        ));
        Assert.Equal("NOME_OBRIGATORIO", ex.Message);
    }
}