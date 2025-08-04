using System.Text.RegularExpressions;
namespace AcademiaDoZe.Domain.Services
{
    public static partial class NormalizadoService
    {
        // verifica se o texto é nulo ou vazio
        public static bool TextoVazioOuNulo(string? texto) => string.IsNullOrWhiteSpace(texto);
        // remove espaços repetidos e espaços no início e no final do texto
        public static string LimparEspacos(string? texto) => string.IsNullOrWhiteSpace(texto) ? string.Empty : EspacosRegex().Replace(texto, " ").Trim();
        // limpa todos os espaços
        public static string LimparTodosEspacos(string? texto) => string.IsNullOrWhiteSpace(texto) ? string.Empty : texto.Replace(" ", string.Empty);
        // converte o texto para maiúsculo
        public static string ParaMaiusculo(string? texto) => string.IsNullOrEmpty(texto) ? string.Empty : texto.ToUpperInvariant();
        // manter somente digitos numericos
        public static string LimparEDigitos(string? texto) => string.IsNullOrEmpty(texto) ? string.Empty : new string([.. texto.Where(char.IsDigit)]);
        // validar se email contém @ e ponto
        public static bool ValidarFormatoEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return !email.Contains('@') || !email.Contains('.');
        }
        // validar formato da senha - mínimo 6 caracteres, pelo menos uma letra maiúscula
        public static bool ValidarFormatoSenha(string? senha)
        {
            if (string.IsNullOrWhiteSpace(senha)) return true;
            return senha.Length < 6 || !senha.Any(char.IsUpper);
        }
        [GeneratedRegex(@"\s+")]
        private static partial Regex EspacosRegex();
    }
}
