// Henrique Churkin Correia Alberton
using System.ComponentModel.DataAnnotations;
namespace AcademiaDoZe.Application.Enums;

public enum EAppPessoaTipo
{
    [Display(Name = "Colaborador")]
    Colaborador = 0,
    [Display(Name = "Aluno")]
    Aluno = 1
}