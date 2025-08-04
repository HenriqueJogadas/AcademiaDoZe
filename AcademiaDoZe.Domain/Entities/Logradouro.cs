//Henrique Churkin Correia Alberton
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.Services;

namespace AcademiaDoZe.Domain.Entities
{
    public sealed class Logradouro : Entity
    {
        public string CEP { get; }
        public string NomeLogradouro { get; }
        public string Bairro { get; }
        public string Cidade { get; }
        public string Estado { get; }
        public string Pais { get; }

        private Logradouro(string CEP, string NomeLogradouro, string Bairro, string Cidade, string Estado, string Pais) : base()
        {
            this.CEP = CEP;
            this.NomeLogradouro = NomeLogradouro;
            this.Bairro = Bairro;
            this.Cidade = Cidade;
            this.Estado = Estado;
            this.Pais = Pais;
        }

        public static Logradouro Criar(string CEP, string NomeLogradouro, string Bairro, string Cidade, string Estado, string Pais)
        {

            if (string.IsNullOrWhiteSpace(CEP)) throw new DomainException("CEP_OBRIGATORIO");
            CEP = NormalizadoService.LimparEDigitos(CEP);
            if (string.IsNullOrWhiteSpace(NomeLogradouro)) throw new DomainException("NOME_OBRIGATORIO");
            NomeLogradouro = NormalizadoService.LimparEspacos(NomeLogradouro);
            if (string.IsNullOrWhiteSpace(Bairro)) throw new DomainException("BAIRRO_OBRIGATORIA");
            Bairro = NormalizadoService.LimparEspacos(Bairro);
            if (string.IsNullOrWhiteSpace(Cidade)) throw new DomainException("CIDADE_OBRIGATORIA");
            Cidade = NormalizadoService.LimparEspacos(Cidade);
            if (string.IsNullOrWhiteSpace(Estado)) throw new DomainException("ESTADO_OBRIGATORIO");
            Estado = NormalizadoService.ParaMaiusculo(NormalizadoService.LimparTodosEspacos(Estado));
            if (CEP.Length != 8) throw new DomainException("CEP_DIGITOS");
            if (string.IsNullOrWhiteSpace(Pais)) throw new DomainException("PAIS_OBRIGATORIO");
            Pais = NormalizadoService.LimparEspacos(Pais);
            
            return new Logradouro(CEP, NomeLogradouro, Bairro, Cidade, Estado, Pais);
        }
    }
}