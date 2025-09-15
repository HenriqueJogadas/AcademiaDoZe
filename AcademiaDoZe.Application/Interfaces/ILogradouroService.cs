// Henrique Churkin Correia Alberton

using AcademiaDoZe.Application.DTOs;

namespace AcademiaDoZe.Application.Interfaces;

public interface ILogradouroService
{
    Task<LogradouroDTO> ObterPorIdAsync(int id);
    Task<IEnumerable<LogradouroDTO>> ObterTodosAsync();
    Task<LogradouroDTO> AdicionarAsync(LogradouroDTO logradouroDto);
    Task<LogradouroDTO> AtualizarAsync(LogradouroDTO logradouroDto);
    Task<bool> RemoverAsync(int id);
    Task<LogradouroDTO> ObterPorCepAsync(string cep);
    Task<IEnumerable<LogradouroDTO>> ObterPorCidadeAsync(string cidade);
}