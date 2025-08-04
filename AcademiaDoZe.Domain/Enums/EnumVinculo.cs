//Henrique Churkin Correia Alberton
using System.ComponentModel.DataAnnotations;

namespace AcademiaDoZe.Domain.Enums
{
    public enum EnumVinculo
    {
        [Display(Name = "CLT")]
        CLT = 0,

        [Display(Name = "Estagiário")]
        Estagiario = 1,
    }
}
