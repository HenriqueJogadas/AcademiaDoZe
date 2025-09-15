// Henrique Churkin Correia Alberton

using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using AcademiaDoZe.Application.Mappings;
using AcademiaDoZe.Domain.Repositories;

namespace AcademiaDoZe.Application_.Services;

public class MatriculaService : IMatriculaService
{
    private readonly Func<IMatriculaRepository> _repoFactory;
    public MatriculaService(Func<IMatriculaRepository> repoFactory)
    {
        _repoFactory = repoFactory ?? throw new ArgumentNullException(nameof(repoFactory));
    }

    public async Task<MatriculaDTO> AdicionarAsync(MatriculaDTO matriculaDto)
    {

        var idExistente = await _repoFactory().ObterPorId(matriculaDto.Id);
        if (idExistente != null)

        {
            throw new InvalidOperationException($"Matricula com ID {idExistente.Id}, já cadastrado com o Id {idExistente.Id}.");
        }
        var matricula = matriculaDto.ToEntity();
        await _repoFactory().Adicionar(matricula);
        return matricula.ToDto();

    }

    public async Task<MatriculaDTO> AtualizarAsync(MatriculaDTO matriculaDto)
    {

        var matriculaExistente = await _repoFactory().ObterPorId(matriculaDto.Id) ?? throw new KeyNotFoundException($"Matricula ID {matriculaDto.Id} não encontrado.");


        if (!string.Equals(matriculaExistente.Id.ToString(), matriculaDto.Id.ToString(), StringComparison.OrdinalIgnoreCase))

        {
            var idExistente = await _repoFactory().ObterPorId(matriculaDto.Id);
            if (idExistente != null && idExistente.Id != matriculaDto.Id)

            {
                throw new InvalidOperationException($"Matricula com ID {idExistente.Id}, já cadastrado com o Id {idExistente.Id}.");
            }
        }

        var matriculaAtualizado = matriculaExistente.UpdateFromDto(matriculaDto);
        await _repoFactory().Atualizar(matriculaAtualizado);
        return matriculaAtualizado.ToDto();

    }

    public async Task<MatriculaDTO> ObterPorIdAsync(int id)
    {
        var matricula = await _repoFactory().ObterPorId(id);
        return (matricula != null) ? matricula.ToDto() : null!;

    }

    public async Task<IEnumerable<MatriculaDTO>> ObterTodasAsync()
    {
        var matriculas = await _repoFactory().ObterTodos();

        return [.. matriculas.Select(l => l.ToDto())]; 

    }

    public async Task<IEnumerable<MatriculaDTO>> ObterPorAlunoIdAsync(int alunoId)
    {
        var matriculas = await _repoFactory().ObterPorAluno(alunoId);
        return matriculas.Select(m => m.ToDto());
    }

    public async Task<IEnumerable<MatriculaDTO>> ObterAtivasAsync(int alunoId = 0)
    {
        var matriculas = await _repoFactory().ObterAtivas(alunoId);
        return matriculas.Select(m => m.ToDto());
    }

    public async Task<IEnumerable<MatriculaDTO>> ObterVencendoEmDiasAsync(int dias)
    {
        var matriculas = await _repoFactory().ObterVencendoEmDias(dias);
        return matriculas.Select(m => m.ToDto());
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var matricula = await _repoFactory().ObterPorId(id);

        if (matricula == null)

        {
            return false;
        }
        await _repoFactory().Remover(id);

        return true;

    }
}