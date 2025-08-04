//Henrique Churkin Correia Alberton
using System.ComponentModel.DataAnnotations;

namespace AcademiaDoZe.Domain.Enums
{
    public enum EnumPessoaTipo
    {
        [Display(Name = "Colaborador")]
        Colaborador = 0,
        [Display(Name = "Aluno")]
        Aluno = 1
    }
}
