//Henrique Churkin Correia Alberton
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Entities
{
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

        protected Pessoa(string Nome, string CPF, DateOnly DataNascimento, string Telefone, string Email, string Senha, Arquivo Foto, Logradouro Endereco, string Numero, string Complemento) : base()
        {
            this.Nome = Nome;
            this.Cpf = CPF;
            this.DataNascimento = DataNascimento;
            this.Telefone = Telefone;
            this.Email = Email;
            this.Senha = Senha;
            this.Foto = Foto;
            this.Endereco = Endereco;
            this.Numero = Numero;
            this.Complemento = Complemento;
        }
    }
}