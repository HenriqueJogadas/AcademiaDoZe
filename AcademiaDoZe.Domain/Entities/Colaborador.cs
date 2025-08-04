//Henrique Churkin Correia Alberton
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.Services;
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Entities
{
    public class Colaborador : Pessoa
    {
        public DateOnly DataAdmissao { get; private set; }
        public EnumCargo Cargo { get; private set; }
        public EnumVinculo Vinculo { get; private set; }

        private Colaborador(string Nome, string Cpf, DateOnly DataNascimento, string Telefone, string Email, string Senha, Arquivo Foto, Logradouro Endereco, string Numero, string Complemento, DateOnly DataAdmissao, EnumCargo Cargo, EnumVinculo Vinculo) : base(Nome, Cpf, DataNascimento, Telefone, Email, Senha, Foto, Endereco, Numero, Complemento)
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
            this.DataAdmissao = DataAdmissao;
            this.Cargo = Cargo;
            this.Vinculo = Vinculo;
        }

        public static Colaborador Criar(string Nome, string Cpf, DateOnly DataNascimento, string Telefone, string Email, string Senha, Arquivo Foto, Logradouro Endereco, string Numero, string Complemento, DateOnly DataAdmissao, EnumCargo Cargo, EnumVinculo Vinculo)
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
            if (string.IsNullOrWhiteSpace(Endereco.ToString())) throw new DomainException("ENDERECO_OBRIGATORIO");
            if (string.IsNullOrWhiteSpace(Numero)) throw new DomainException("NUMERO_OBRIGATORIO");
            Numero = NormalizadoService.LimparEspacos(Numero);
            Complemento = NormalizadoService.LimparEspacos(Complemento);
            if (DataAdmissao == default) throw new DomainException("DATADEADMISSAO_OBRIGATORIA");
            if (DataAdmissao > DateOnly.FromDateTime(DateTime.Today)) throw new DomainException("DATA_ADMISSAO_MAIOR_ATUAL");
            if (!Enum.IsDefined(Cargo)) throw new DomainException("CARGO_COLABORADOR_INVALIDO");
            if (!Enum.IsDefined(Vinculo)) throw new DomainException("VINCULO_COLABORADOR_INVALIDO");
            if (Cargo == EnumCargo.Administrador && Vinculo == EnumVinculo.CLT) throw new DomainException("ADMINISTRADOR_CLT_INVALIDO");


            return new Colaborador(Nome, Cpf, DataNascimento, Telefone, Email, Senha, Foto, Endereco, Numero, Complemento, DataAdmissao, Cargo, Vinculo);
        }
    }
}