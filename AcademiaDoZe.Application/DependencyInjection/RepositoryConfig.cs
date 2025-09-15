// Henrique Churkin Correia Alberton
using AcademiaDoZe.Application.Enums;
namespace AcademiaDoZe.Application.DependencyInjection;

public class RepositoryConfig
{
    public required string ConnectionString { get; set; }
    public required EAppDatabaseType DatabaseType { get; set; }
}