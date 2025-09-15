// Henrique Churkin Correia Alberton

using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Domain.Enums;

namespace AcademiaDoZe.Application.Mappings;

public static class MatriculaEnumMappings
{
    public static EMatriculaPlano ToDomain(this EAppMatriculaPlano appPlano)
    {
        return (EMatriculaPlano)appPlano;
    }
    public static EAppMatriculaPlano ToApp(this EMatriculaPlano domainPlano)
    {
        return (EAppMatriculaPlano)domainPlano;
    }
    public static EMatriculaRestricoes ToDomain(this EAppMatriculaRestricoes appRestricoes)
    {
        return (EMatriculaRestricoes)appRestricoes;
    }
    public static EAppMatriculaRestricoes ToApp(this EMatriculaRestricoes domainRestricoes)
    {
        return (EAppMatriculaRestricoes)domainRestricoes;
    }
}