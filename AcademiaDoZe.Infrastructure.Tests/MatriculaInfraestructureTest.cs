// Henrique Churkin Correia Alberton
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.ValueObjects;
using AcademiaDoZe.Infrastructure.Repositories;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AcademiaDoZe.Infrastructure.Tests
{
    public class MatriculaInfrastructureTests : TestBase
    {

        [Fact]
        public async Task Matricula_Adicionar_ObterPorAluno_Atualizar_Remover()
        {
            var alunoRepo = new AlunoRepository(ConnectionString, DatabaseType);
            var aluno = Aluno.Criar(
                "Teste Aluno",
                "12345678901",
                new DateOnly(2005, 1, 1),
                "49999999999",
                "aluno@teste.com",
                "senha",
                Arquivo.Criar(new byte[] { 1, 2, 3 }, "arquivo"),
                Logradouro.Criar("00000000", "Rua Teste", "Bairro Teste", "Cidade Teste", "ST", "Pais"),
                "123",
                "Apto 1"
            );
            var alunoInserido = await alunoRepo.Adicionar(aluno);
            Assert.NotNull(alunoInserido);

            var matriculaRepo = new MatriculaRepository(ConnectionString, DatabaseType);
            var matricula = Matricula.Criar(
                alunoInserido,
                EnumPlano.Mensal,
                DateOnly.FromDateTime(DateTime.Today),
                DateOnly.FromDateTime(DateTime.Today.AddMonths(1)),
                "Objetivo Teste",
                EnumRestricoes.None,
                null,
                null
            );

            var matriculaInserida = await matriculaRepo.Adicionar(matricula);
            Assert.NotNull(matriculaInserida);
            Assert.True(matriculaInserida.Id > 0);

            var matriculasDoAluno = await matriculaRepo.ObterPorAluno(alunoInserido.Id);
            Assert.NotEmpty(matriculasDoAluno);
            
            var matriculaAtualizada = Matricula.Criar(
                alunoInserido,
                EnumPlano.Mensal,
                matriculaInserida.DataInicio,
                matriculaInserida.DataFinal,
                "Objetivo Atualizado",
                matriculaInserida.Restricoes,
                null,
                matriculaInserida.LaudoMedico
            );
            typeof(Entity).GetProperty("Id")?.SetValue(matriculaAtualizada, matriculaInserida.Id);

            var resultadoAtualizacao = await matriculaRepo.Atualizar(matriculaAtualizada);
            Assert.NotNull(resultadoAtualizacao);
            Assert.Equal("Objetivo Atualizado", resultadoAtualizacao.Objetivo);

            var resultadoRemover = await matriculaRepo.Remover(matriculaInserida.Id);
            Assert.True(resultadoRemover);
            var matriculaRemovida = await matriculaRepo.ObterPorId(matriculaInserida.Id);
            Assert.Null(matriculaRemovida);
        }
    }
}
