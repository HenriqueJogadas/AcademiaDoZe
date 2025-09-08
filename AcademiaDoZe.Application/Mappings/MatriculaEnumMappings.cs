//Henrique Churkin Correia Alberton
using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Domain.Enums;
namespace AcademiaDoZe.Application.Mappings
{
    public static class MatriculaEnumMappings
    {
        public static EnumPlano ToDomain(this EAppMatriculaPlano appPlano)
        {
            return (EnumPlano)appPlano;
        }
        public static EAppMatriculaPlano ToApp(this EnumPlano domainPlano)
        {
            return (EAppMatriculaPlano)domainPlano;
        }
        public static EnumRestricoes ToDomain(this EAppMatriculaRestricoes appRestricoes)
        {
            return (EnumRestricoes)appRestricoes;
        }
        public static EAppMatriculaRestricoes ToApp(this EnumRestricoes domainRestricoes)
        {
            return (EAppMatriculaRestricoes)domainRestricoes;
        }
    }
}
