// Henrique Churkin Correia Alberton

using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Entities;

public abstract class Pessoa : Entity
{
    public string Nome { get; protected set; }
    public string Cpf { get; protected set; }
    public DateOnly DataNascimento { get; protected set; }
    public string Telefone { get; protected set; }
    public string Email { get; protected set; }
    public Logradouro Endereco { get; protected set; }
    public string Numero { get; protected set; }
    public string Complemento { get; protected set; }
    public string Senha { get; protected set; }
    public Arquivo Foto { get; protected set; }
    protected Pessoa(int id, string nome, string cpf, DateOnly dataNascimento, string telefone,  string email,
        Logradouro endereco, string numero, string complemento, string senha, Arquivo foto) : base(id)
    {
        Id = Id;
        Nome = nome;
        Cpf = cpf;
        DataNascimento = dataNascimento;
        Telefone = telefone;
        Email = email;
        Endereco = endereco;
        Numero = numero;
        Complemento = complemento;
        Senha = senha;
        Foto = foto;
    }
}