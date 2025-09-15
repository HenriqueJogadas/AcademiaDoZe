// Henrique Churkin Correia Alberton

using System.ComponentModel.DataAnnotations;

namespace AcademiaDoZe.Domain.Enums;

public enum EColaboradorVinculo
{
    [Display(Name = "CLT")]
    CLT = 0,
    [Display(Name = "Estagiário")]
    Estagio = 1
}