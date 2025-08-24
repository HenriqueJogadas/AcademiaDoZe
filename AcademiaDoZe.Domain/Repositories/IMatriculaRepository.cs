//Henrique Churkin Correia Alberton
using AcademiaDoZe.Domain.Entities;
namespace AcademiaDoZe.Domain.Repositories
{

    public interface IMatriculaRepository : IRepository<Matricula>
    {
        Task<IEnumerable<Matricula>> ObterPorAluno(int alunoId);
        Task<IEnumerable<Matricula>> ObterAtivas();
        Task<IEnumerable<Matricula>> ObterVencendoEmDias(int dias);
    }
}