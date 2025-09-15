// Henrique Churkin Correia Alberton

using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.Services;


namespace AcademiaDoZe.Domain.Entities;

public sealed class Logradouro : Entity
{
    public string Cep { get; }
    public string Nome { get; }
    public string Bairro { get; }
    public string Cidade { get; }
    public string Estado { get; }
    public string Pais { get; }
    private Logradouro(int id, string cep, string nome, string bairro, string cidade, string estado, string pais) : base(id)
    {
        Id = id;
        Cep = cep;
        Nome = nome;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        Pais = pais;
    }
    public static Logradouro Criar(int id, string cep, string nome, string bairro, string cidade, string estado, string pais)
    {

        if (NormalizadoService.TextoVazioOuNulo(cep)) throw new DomainException("CEP_OBRIGATORIO");

        cep = NormalizadoService.LimparEDigitos(cep);
        if (cep.Length != 8) throw new DomainException("CEP_DIGITOS");
        if (NormalizadoService.TextoVazioOuNulo(nome)) throw new DomainException("NOME_OBRIGATORIO");
        nome = NormalizadoService.LimparEspacos(nome);
        if (NormalizadoService.TextoVazioOuNulo(bairro)) throw new DomainException("BAIRRO_OBRIGATORIO");
        bairro = NormalizadoService.LimparEspacos(bairro);
        if (NormalizadoService.TextoVazioOuNulo(cidade)) throw new DomainException("CIDADE_OBRIGATORIO");
        cidade = NormalizadoService.LimparEspacos(cidade);
        if (NormalizadoService.TextoVazioOuNulo(estado)) throw new DomainException("ESTADO_OBRIGATORIO");
        estado = NormalizadoService.ParaMaiusculo(NormalizadoService.LimparTodosEspacos(estado));
        if (NormalizadoService.TextoVazioOuNulo(pais)) throw new DomainException("PAIS_OBRIGATORIO");
        pais = NormalizadoService.LimparEspacos(pais);
        return new Logradouro(id, cep, nome, bairro, cidade, estado, pais);
    }
}