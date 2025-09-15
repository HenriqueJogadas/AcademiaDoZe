// Henrique Churkin Correia Alberton
namespace AcademiaDoZe.Application.DTOs;

public class LogradouroDTO
{
    public int Id { get; set; }
    public required string Cep { get; set; }
    public required string Nome { get; set; }
    public required string Bairro { get; set; }
    public required string Cidade { get; set; }
    public required string Estado { get; set; }
    public required string Pais { get; set; }
}