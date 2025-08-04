//Henrique Churkin Correia Alberton
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.Services;
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Entities
{
    public class Matricula : Entity
    {
        public Aluno Aluno { get; private set; }
        public EnumPlano Plano { get; private set; }
        public DateOnly DataInicio { get; private set; }
        public DateOnly DataFinal { get; private set; }
        public string Objetivo { get; private set; }
        public EnumRestricoes Restricoes { get; private set; }
        public string ObservacoesRestricoes { get; private set; }
        public Arquivo LaudoMedico { get; private set; }

        private Matricula(Aluno Aluno, EnumPlano Plano, DateOnly DataInicio, DateOnly DataFinal, string Objetivo, EnumRestricoes Restricoes, string ObservacoesRestricoes, Arquivo LaudoMedico) : base()
        {
            this.Aluno = Aluno;
            this.Plano = Plano;
            this.DataInicio = DataInicio;
            this.DataFinal = DataFinal;
            this.Objetivo = Objetivo;
            this.Restricoes = Restricoes;
            this.ObservacoesRestricoes = ObservacoesRestricoes;
            this.LaudoMedico = LaudoMedico;
        }

        public static Matricula Criar(Aluno Aluno, EnumPlano Plano, DateOnly DataInicio, DateOnly DataFinal, string Objetivo, EnumRestricoes Restricoes, string ObservacoesRestricoes, Arquivo LaudoMedico)
        {

            if (Aluno == null) throw new DomainException("ALUNO_OBRIGATORIO");
            if (Aluno.DataNascimento > DateOnly.FromDateTime(DateTime.Today.AddYears(-16)) && LaudoMedico == null) throw new DomainException("MENOR16_LAUDO_OBRIGATORIO");
            if (!Enum.IsDefined(Plano)) throw new DomainException("PLANO_INVALIDO");
            if (DataInicio == default) throw new DomainException("DATA_INICIO_OBRIGATORIO");
            if (string.IsNullOrWhiteSpace(Objetivo)) throw new DomainException("OBJETIVO_OBRIGATORIO");
            Objetivo = NormalizadoService.LimparEspacos(Objetivo);
            if (Restricoes != EnumRestricoes.None && LaudoMedico == null) throw new DomainException("RESTRICOES_LAUDO_OBRIGATORIO");
            ObservacoesRestricoes = NormalizadoService.LimparEspacos(ObservacoesRestricoes);

            return new Matricula(Aluno, Plano, DataInicio, DataFinal, Objetivo, Restricoes, ObservacoesRestricoes, LaudoMedico);
        }
    }
}