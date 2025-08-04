//Henrique Churkin Correia Alberton
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.Services;
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Entities
{
    public class Aluno : Pessoa
    {
        private Aluno(string Nome, string Cpf, DateOnly DataNascimento, string Telefone, string Email, string Senha, Arquivo Foto, Logradouro Endereco, string Numero, string Complemento) : base(Nome, Cpf, DataNascimento, Telefone, Email, Senha, Foto, Endereco, Numero, Complemento)
        {
            this.Nome = Nome;
            this.Cpf = Cpf;
            this.DataNascimento = DataNascimento;
            this.Telefone = Telefone;
            this.Email = Email;
            this.Senha = Senha;
            this.Foto = Foto;
            this.Endereco = Endereco;
            this.Numero = Numero;
            this.Complemento = Complemento;
        }

        public static Aluno Criar(string Nome, string Cpf, DateOnly DataNascimento, string Telefone, string Email, string Senha, Arquivo Foto, Logradouro Endereco, string Numero, string Complemento)
        {

            if (string.IsNullOrWhiteSpace(Nome)) throw new DomainException("NOME_OBRIGATORIO");
            Nome = NormalizadoService.LimparEspacos(Nome);
            if (string.IsNullOrWhiteSpace(Cpf)) throw new DomainException("CPF_OBRIGATORIO");
            Cpf = NormalizadoService.LimparEDigitos(Cpf);
            if (Cpf.Length != 11) throw new DomainException("CPF_DIGITOS");
            if (string.IsNullOrWhiteSpace(DataNascimento.ToString())) throw new DomainException("DATADENASCIMENTO_OBRIGATORIA");
            if (DataNascimento > DateOnly.FromDateTime(DateTime.Today.AddYears(-12))) throw new DomainException("DATA_NASCIMENTO_MINIMA_INVALIDA");
            if (string.IsNullOrWhiteSpace(Telefone)) throw new DomainException("TELEFONE_OBRIGATORIO");
            Telefone = NormalizadoService.LimparEDigitos(Telefone);
            if (Telefone.Length != 11) throw new DomainException("TELEFONE_DIGITOS");
            if (string.IsNullOrWhiteSpace(Email)) throw new DomainException("EMAIL_OBRIGATORIO");
            Email = NormalizadoService.LimparEspacos(Email);
            if (string.IsNullOrWhiteSpace(Senha)) throw new DomainException("SENHA_OBRIGATORIA");
            Senha = NormalizadoService.LimparEspacos(Senha);
            if (NormalizadoService.ValidarFormatoSenha(Senha)) throw new DomainException("SENHA_FORMATO");
            if (Foto == null) throw new DomainException("FOTO_OBRIGATORIA");
            if (string.IsNullOrWhiteSpace(Endereco.ToString())) throw new DomainException("LOGRADOURO_OBRIGATORIO");
            if (string.IsNullOrWhiteSpace(Numero)) throw new DomainException("NUMERO_OBRIGATORIO");
            Numero = NormalizadoService.LimparEspacos(Numero);
            Complemento = NormalizadoService.LimparEspacos(Complemento);

            return new Aluno(Nome, Cpf, DataNascimento, Telefone, Email, Senha, Foto, Endereco, Numero, Complemento);
        }
    }
}