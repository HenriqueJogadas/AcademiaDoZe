//Henrique Churkin Correia Alberton
namespace AcademiaDoZe.Application.DTOs
{
    public class ArquivoDTO
    {
        // Conteúdo bruto do arquivo

        public byte[]? Conteudo { get; set; }

        // MIME type detectado/atribuído (ex.: image/png, application/pdf)

        public string? ContentType { get; set; }

    }
}
