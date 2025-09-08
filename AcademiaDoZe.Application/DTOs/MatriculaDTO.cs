//Henrique Churkin Correia Alberton
using AcademiaDoZe.Application.Enums;
namespace AcademiaDoZe.Application.DTOs
{
    public class MatriculaDTO
    {
        public int Id { get; set; }
        public required AlunoDTO Aluno { get; set; }
        public required EAppMatriculaPlano Plano { get; set; }
        public required DateOnly DataInicio { get; set; }
        public required DateOnly DataFinal { get; set; }
        public required string Objetivo { get; set; }
        public required EAppMatriculaRestricoes Restricoes { get; set; }
        public string? ObservacoesRestricoes { get; set; }
        public ArquivoDTO? LaudoMedico { get; set; }
    }
}
