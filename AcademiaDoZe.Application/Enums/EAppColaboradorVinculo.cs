// Henrique Churkin Correia Alberton
using System.ComponentModel.DataAnnotations;
namespace AcademiaDoZe.Application.Enums;

public enum EAppColaboradorVinculo
{
    [Display(Name = "CLT")]
    CLT = 0,
    [Display(Name = "Estagiário")]
    Estagio = 1
}