//Henrique Churkin Correia Alberton
using System.ComponentModel.DataAnnotations;

namespace AcademiaDoZe.Domain.Enums
{
    public enum EnumCargo
    {
        [Display(Name = "Administrador")]
        Administrador = 0,

        [Display(Name = "Atendente")]
        Atendente = 1,

        [Display(Name = "Instrutor")]
        Instrutor = 2,
    }
}
