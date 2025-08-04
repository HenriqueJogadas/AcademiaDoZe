//Henrique Churkin Correia Alberton
using System.ComponentModel.DataAnnotations;

namespace AcademiaDoZe.Domain.Enums
{
    public enum EnumPlano
    {
        [Display(Name = "Mensal")]
        Mensal = 0,

        [Display(Name = "Trimestral")]
        Trimestral = 1,

        [Display(Name = "Semestral")]
        Semestral = 2,

        [Display(Name = "Anual")]
        Anual = 3,
    }
}
