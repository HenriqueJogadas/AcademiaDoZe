// Henrique Churkin Correia Alberton

using System.Text.RegularExpressions;

namespace AcademiaDoZe.Domain.Services;

public static partial class NormalizadoService
{
    public static bool TextoVazioOuNulo(string? texto) => string.IsNullOrWhiteSpace(texto);
    public static string LimparEspacos(string? texto) => string.IsNullOrWhiteSpace(texto) ? string.Empty : EspacosRegex().Replace(texto, " ").Trim();
    public static string LimparTodosEspacos(string? texto) => string.IsNullOrWhiteSpace(texto) ? string.Empty : texto.Replace(" ", string.Empty);
    public static string ParaMaiusculo(string? texto) => string.IsNullOrEmpty(texto) ? string.Empty : texto.ToUpperInvariant();
    public static string LimparEDigitos(string? texto) => string.IsNullOrEmpty(texto) ? string.Empty : new string([.. texto.Where(char.IsDigit)]);
    public static bool ValidarFormatoEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        return !email.Contains('@') || !email.Contains('.');
    }
    public static bool ValidarFormatoSenha(string? senha)
    {
        if (string.IsNullOrWhiteSpace(senha)) return true;
        return senha.Length < 6 || !senha.Any(char.IsUpper);
    }
    [GeneratedRegex(@"\s+")]
    private static partial Regex EspacosRegex();
}