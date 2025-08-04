//Henrique Churkin Correia Alberton
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.ValueObjects;
namespace AcademiaDoZe.Domain.Tests
{
    public class MatriculaDomainTests
    {
        private Aluno GetValidAluno() => Aluno.Criar("Zé Colmeia", "06917136024", DateOnly.FromDateTime(DateTime.Today.AddYears(-20)), "04940028922", "zecolmeia@email.com", "Senhaforte123", GetValidArquivo(), GetValidLogradouro(), "71", "Perto do mercado");
        private Arquivo GetValidArquivo() => Arquivo.Criar(new byte[1], ".jpg");
        private Logradouro GetValidLogradouro() => Logradouro.Criar("12345678", "Rua A", "Centro", "Cidade", "SP", "Brasil");
        [Fact] 
        public void CriarMatricula_ComDadosValidos_DeveCriarObjeto() 
        {
            
            var aluno = GetValidAluno(); var plano = EnumPlano.Mensal; var datainicio = DateOnly.FromDateTime(DateTime.Today.AddDays(-15)); var datafinal = DateOnly.FromDateTime(DateTime.Today.AddMonths(3));
            var objetivo = "Ficar maromba"; var restricoes = EnumRestricoes.PressaoAlta; var observacoesrestricoes = ""; var laudomedico = GetValidArquivo();
            
            var matricula = Matricula.Criar(aluno, plano, datainicio, datafinal, objetivo, restricoes, observacoesrestricoes, laudomedico);
            
            Assert.NotNull(matricula);
        }
        [Fact]
        public void CriarMatricula_ComObjetivoVazio_DeveLancarExcecao()
        {

            var aluno = GetValidAluno(); var plano = EnumPlano.Mensal; var datainicio = DateOnly.FromDateTime(DateTime.Today.AddDays(-15)); var datafinal = DateOnly.FromDateTime(DateTime.Today.AddMonths(3));
            var restricoes = EnumRestricoes.PressaoAlta; var observacoesrestricoes = ""; var laudomedico = GetValidArquivo();

            var ex = Assert.Throws<DomainException>(() =>
            Matricula.Criar(
            aluno,
            plano,
            datainicio,
            datafinal,
            "",
            restricoes,
            observacoesrestricoes,
            laudomedico
            ));
            Assert.Equal("OBJETIVO_OBRIGATORIO", ex.Message);
        }
    }
}