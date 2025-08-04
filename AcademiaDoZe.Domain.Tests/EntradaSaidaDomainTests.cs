//Henrique Churkin Correia Alberton
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.ValueObjects;
namespace AcademiaDoZe.Domain.Tests
{
    public class EntradaSaidaDomainTests
    {

        private Aluno GetValidAluno() => Aluno.Criar("Zé Colmeia", "06917136024", DateOnly.FromDateTime(DateTime.Today.AddYears(-20)), "04940028922", "zecolmeia@email.com", "Senhaforte123", GetValidArquivo(), GetValidLogradouro(), "71", "Perto do mercado");
        private Colaborador GetValidColaborador() => Colaborador.Criar("Catatau", "06917136025", DateOnly.FromDateTime(DateTime.Today.AddYears(-18)), "04940022500", "catataufortnite@email.com", "Catatau25", GetValidArquivo(), GetValidLogradouro(), "71", "Perto do mercado", DateOnly.FromDateTime(DateTime.Today.AddYears(-5)), EnumCargo.Atendente, EnumVinculo.Estagiario);
        private Logradouro GetValidLogradouro() => Logradouro.Criar("12345678", "Rua A", "Centro", "Cidade", "SP", "Brasil");
        private Arquivo GetValidArquivo() => Arquivo.Criar(new byte[1], ".jpg");
        [Fact]
        public void CriarEntradaSaida_ComDadosValidos_DeveCriarObjeto()
        {

            var pessoatipo = EnumPessoaTipo.Aluno; var alunocolaborador = GetValidAluno();
            var datahora = DateTime.Now;

            var entradasaida = EntradaSaida.Criar(pessoatipo, alunocolaborador, datahora);

            Assert.NotNull(entradasaida);
        }
    }
}