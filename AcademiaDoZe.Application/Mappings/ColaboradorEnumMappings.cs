//Henrique Churkin Correia Alberton
using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Domain.Enums;
namespace AcademiaDoZe.Application.Mappings
{
    public static class ColaboradorEnumMappings
    {
        public static EnumCargo ToDomain(this EAppColaboradorCargo appTipo)
        {
            return (EnumCargo)appTipo;
        }
        public static EAppColaboradorCargo ToApp(this EnumCargo domainTipo)
        {
            return (EAppColaboradorCargo)domainTipo;
        }
        public static EnumVinculo ToDomain(this EAppColaboradorVinculo appVinculo)
        {
            return (EnumVinculo)appVinculo;
        }
        public static EAppColaboradorVinculo ToApp(this EnumVinculo domainVinculo)
        {
            return (EAppColaboradorVinculo)domainVinculo;
        }
    }
}