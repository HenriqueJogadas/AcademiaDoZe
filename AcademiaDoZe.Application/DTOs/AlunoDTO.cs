// Henrique Churkin Correia Alberton

namespace AcademiaDoZe.Application.DTOs;

public class AlunoDTO
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string Cpf { get; set; }
    public required DateOnly DataNascimento { get; set; }
    public required string Telefone { get; set; }
    public string? Email { get; set; }
    public required LogradouroDTO Endereco { get; set; }
    public required string Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Senha { get; set; }
    public ArquivoDTO? Foto { get; set; }
}