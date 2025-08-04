//Henrique Churkin Correia Alberton
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.ValueObjects;
namespace AcademiaDoZe.Domain.Tests
{
    public class AlunoDomainTests
    {

        private Logradouro GetValidLogradouro() => Logradouro.Criar("12345678", "Rua A", "Centro", "Cidade", "SP", "Brasil");
        private Arquivo GetValidArquivo() => Arquivo.Criar(new byte[1], ".jpg");
        [Fact] 
        public void CriarAluno_ComDadosValidos_DeveCriarObjeto() 
        {
            
            var nome = "João da Silva"; var cpf = "12345678901"; var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)); var telefone = "11999999999";
            var email = "joao@email.com"; var endereco = GetValidLogradouro(); var numero = "123"; var complemento = "Apto 1"; var senha = "Senha@1"; var foto = GetValidArquivo();
            
            var aluno = Aluno.Criar(nome, cpf, dataNascimento, telefone, email,senha, foto, endereco, numero, complemento);
            
            Assert.NotNull(aluno);
        }
        [Fact]
        public void CriarAluno_ComNomeVazio_DeveLancarExcecao()
        {
            
            var cpf = "12345678901"; var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)); var telefone = "11999999999";
            var email = "joao@email.com"; var endereco = GetValidLogradouro(); var numero = "123"; var complemento = "Apto 1"; var senha = "Senha@123"; var foto = GetValidArquivo();
            
            var ex = Assert.Throws<DomainException>(() =>
            Aluno.Criar(
            "",
            cpf,
            dataNascimento,
            telefone,
            email,
            senha,
            foto,
            endereco,
            numero,
            complemento
            ));
            Assert.Equal("NOME_OBRIGATORIO", ex.Message);
        }
    }
}