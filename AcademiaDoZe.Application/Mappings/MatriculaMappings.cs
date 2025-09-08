//Henrique Churkin Correia Alberton
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.ValueObjects;
namespace AcademiaDoZe.Application.Mappings
{
    public static class MatriculaMappings
    {
        public static MatriculaDTO ToDto(this Matricula matricula)
        {
            return new MatriculaDTO
            {
                Id = matricula.Id,
                Aluno = matricula.Aluno.ToDto(),
                Plano = matricula.Plano.ToApp(),
                DataInicio = matricula.DataInicio,
                DataFinal = matricula.DataFinal,
                Objetivo = matricula.Objetivo,
                Restricoes = matricula.Restricoes.ToApp(),
                ObservacoesRestricoes = matricula.ObservacoesRestricoes,
                LaudoMedico = matricula.LaudoMedico != null ? new ArquivoDTO { Conteudo = matricula.LaudoMedico.Conteudo } : null, // Mapeia laudo para DTO
            };
        }
        public static Matricula ToEntity(this MatriculaDTO matriculaDto)
        {
            return Matricula.Criar(
            matriculaDto.Id,
            matriculaDto.Aluno.ToEntityMatricula(), // Mapeia aluno do DTO para a entidade, resolvendo o caso da senha null
            matriculaDto.Plano.ToDomain(),
            matriculaDto.DataInicio,
            matriculaDto.DataFinal,
            matriculaDto.Objetivo,
            matriculaDto.Restricoes.ToDomain(),
            (matriculaDto.LaudoMedico?.Conteudo != null) ? Arquivo.Criar(matriculaDto.LaudoMedico.Conteudo) : null!, // Mapeia laudo do DTO para a entidade
            matriculaDto.ObservacoesRestricoes!
            );
        }
        public static Matricula UpdateFromDto(this Matricula matricula, MatriculaDTO matriculaDto)
        {
            return Matricula.Criar(
            matricula.Id, // Mantém o ID original
            matriculaDto.Aluno.ToEntityMatricula() ?? matricula.Aluno,
            matriculaDto.Plano != default ? matriculaDto.Plano.ToDomain() : matricula.Plano,
            matriculaDto.DataInicio != default ? matriculaDto.DataInicio : matricula.DataInicio,
            matriculaDto.DataFinal != default ? matriculaDto.DataFinal : matricula.DataFinal,
            matriculaDto.Objetivo ?? matricula.Objetivo,
            matriculaDto.Restricoes != default ? matriculaDto.Restricoes.ToDomain() : matricula.Restricoes,
            (matriculaDto.LaudoMedico?.Conteudo != null) ? Arquivo.Criar(matriculaDto.LaudoMedico.Conteudo) : matricula.LaudoMedico, // Atualiza laudo se fornecido
            matriculaDto.ObservacoesRestricoes ?? matricula.ObservacoesRestricoes
            );
        }
    }
}
