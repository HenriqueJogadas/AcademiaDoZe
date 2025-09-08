//Henrique Churkin Correia Alberton
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.Services;
namespace AcademiaDoZe.Domain.ValueObjects
{
    public record Arquivo
    {
        public byte[] Conteudo { get; }
        private Arquivo(byte[] conteudo)
        {
            Conteudo = conteudo;
        }

        public string Caminho { get; }

        public Arquivo(string caminho)
        {
            Caminho = caminho;
        }



        public static Arquivo Criar(byte[] conteudo, string tipoArquivo)
        {
            if (conteudo == null || conteudo.Length == 0)
                throw new DomainException("ARQUIVO_VAZIO");
            if (NormalizadoService.TextoVazioOuNulo(tipoArquivo))
                throw new DomainException("ARQUIVO_TIPO_OBRIGATORIO");
            var tiposPermitidos = new[] { ".jpg", ".jpeg", ".png", ".pdf", ".docx" };
            if (!tiposPermitidos.Contains(tipoArquivo.ToLower()))
                throw new DomainException("ARQUIVO_TIPO_INVALIDO");
            const int tamanhoMaximoBytes = 5 * 1024 * 1024; // 5MB
            if (conteudo.Length > tamanhoMaximoBytes)
                throw new DomainException("ARQUIVO_TIPO_TAMANHO");
            // cria e retorna o objeto

            return new Arquivo(conteudo);

        }
    }
}
