//Henrique Churkin Correia Alberton
using AcademiaDoZe.Application.DTOs;
namespace AcademiaDoZe.Application.Interfaces
{
    public interface IMatriculaService
    {
        Task<MatriculaDTO> ObterPorIdAsync(int id);
        Task<IEnumerable<MatriculaDTO>> ObterTodasAsync();
        Task<MatriculaDTO> AdicionarAsync(MatriculaDTO matriculaDto);
        Task<MatriculaDTO> AtualizarAsync(MatriculaDTO matriculaDto);
        Task<bool> RemoverAsync(int id);
        Task<IEnumerable<MatriculaDTO>> ObterPorAlunoIdAsync(int alunoId);
        Task<IEnumerable<MatriculaDTO>> ObterAtivasAsync(int alunoId = 0);
        Task<IEnumerable<MatriculaDTO>> ObterVencendoEmDiasAsync(int dias);
    }
}