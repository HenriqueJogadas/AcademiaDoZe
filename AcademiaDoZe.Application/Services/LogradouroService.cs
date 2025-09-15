// Henrique Churkin Correia Alberton

using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using AcademiaDoZe.Application.Mappings;
using AcademiaDoZe.Domain.Repositories;
namespace AcademiaDoZe.Application.Services;

public class LogradouroService : ILogradouroService
{
    private readonly Func<ILogradouroRepository> _repoFactory;
    public LogradouroService(Func<ILogradouroRepository> repoFactory)
    {
        _repoFactory = repoFactory ?? throw new ArgumentNullException(nameof(repoFactory));
    }
    public async Task<LogradouroDTO> ObterPorIdAsync(int id)
    {
        var logradouro = await _repoFactory().ObterPorId(id);
        return (logradouro != null) ? logradouro.ToDto() : null!;

    }
    public async Task<IEnumerable<LogradouroDTO>> ObterTodosAsync()
    {
        var logradouros = await _repoFactory().ObterTodos();

        return [.. logradouros.Select(l => l.ToDto())];

    }
    public async Task<bool> RemoverAsync(int id)
    {
        var logradouro = await _repoFactory().ObterPorId(id);

        if (logradouro == null)

        {
            return false;
        }
        await _repoFactory().Remover(id);

        return true;

    }
    public async Task<LogradouroDTO> ObterPorCepAsync(string cep)
    {
        if (string.IsNullOrWhiteSpace(cep))
            throw new ArgumentException("CEP não pode ser vazio.", nameof(cep));
        cep = new string([.. cep.Where(char.IsDigit)]);
        var logradouro = await _repoFactory().ObterPorCep(cep);
        return (logradouro != null) ? logradouro.ToDto() : null!;

    }
    public async Task<IEnumerable<LogradouroDTO>> ObterPorCidadeAsync(string cidade)
    {
        if (string.IsNullOrWhiteSpace(cidade))
            throw new ArgumentException("Cidade não pode ser vazia.", nameof(cidade));
        var logradouros = await _repoFactory().ObterPorCidade(cidade.Trim());

        return [.. logradouros.Select(l => l.ToDto())];

    }
    public async Task<LogradouroDTO> AdicionarAsync(LogradouroDTO logradouroDto)
    {

        var cepExistente = await _repoFactory().ObterPorCep(logradouroDto.Cep);
        if (cepExistente != null)

        {
            throw new InvalidOperationException($"Logradouro com ID {cepExistente.Id}, já cadastrado com o CEP {cepExistente.Cep}.");
        }
        var logradouro = logradouroDto.ToEntity();
        await _repoFactory().Adicionar(logradouro);
        return logradouro.ToDto();

    }
    public async Task<LogradouroDTO> AtualizarAsync(LogradouroDTO logradouroDto)
    {

        var logradouroExistente = await _repoFactory().ObterPorId(logradouroDto.Id) ?? throw new KeyNotFoundException($"Logradouro ID {logradouroDto.Id} não encontrado.");


        if (!string.Equals(logradouroExistente.Cep, logradouroDto.Cep, StringComparison.OrdinalIgnoreCase))

        {
            var cepExistente = await _repoFactory().ObterPorCep(logradouroDto.Cep);
            if (cepExistente != null && cepExistente.Id != logradouroDto.Id)

            {
                throw new InvalidOperationException($"Logradouro com ID {cepExistente.Id}, já cadastrado com o CEP {cepExistente.Cep}.");
            }
        }

        var logradouroAtualizado = logradouroExistente.UpdateFromDto(logradouroDto);
        await _repoFactory().Atualizar(logradouroAtualizado);
        return logradouroAtualizado.ToDto();

    }
}