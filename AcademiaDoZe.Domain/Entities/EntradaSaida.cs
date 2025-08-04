//Henrique Churkin Correia Alberton
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Exceptions;

namespace AcademiaDoZe.Domain.Entities
{
    public class EntradaSaida
    {
        public EnumPessoaTipo Tipo { get; private set; }
        public Pessoa AlunoColaborador { get; private set; }
        public DateTime DataHora { get; private set; }

        private EntradaSaida(EnumPessoaTipo Tipo, Pessoa Pessoa, DateTime DataHora) : base()
        {
            this.Tipo = Tipo;
            this.AlunoColaborador = Pessoa;
            this.DataHora = DataHora;
        }

        public static EntradaSaida Criar(EnumPessoaTipo Tipo, Pessoa Pessoa, DateTime DataHora)
        {
            if (!Enum.IsDefined(Tipo)) throw new DomainException("TIPO_OBRIGATORIO");
            if (Pessoa == null) throw new DomainException("PESSOA_OBRIGATORIA");
            if (DataHora < DateTime.Now.AddSeconds(-1)) throw new DomainException("DATAHORA_INVALIDA");
            if (DataHora.TimeOfDay <= new TimeSpan(6, 0, 0) || DataHora.TimeOfDay > new TimeSpan(22, 0, 0))
                throw new DomainException("DATAHORA_INTERVALO");

            if (Pessoa is Aluno aluno)

            {
                // Validar se possui matrícula ativa - depende da persistência de aluno e matrícula.

                // Na entrada, mostrar quanto tempo ainda tem de plano - depende da persistência de aluno e matrícula.
                // Na saída, mostrar o tempo que permaneceu na academia - depende da persistência de aluno e matrícula.

            }
            else if (Pessoa is Colaborador colaborador)
            {
                // Validar se já não ultrapassa o limite de: 8 horas se for ctl, 6 horas se for estágio.

                // Na saída, mostrar o tempo que permaneceu na academia, devendo ser somado todos os registros do dia.

            }

            return new EntradaSaida(Tipo, Pessoa, DataHora);
        }
    }
}