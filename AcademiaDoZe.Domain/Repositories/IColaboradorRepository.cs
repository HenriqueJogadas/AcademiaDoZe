//Henrique Churkin Correia Alberton
using AcademiaDoZe.Domain.Entities;
namespace AcademiaDoZe.Domain.Repositories
{

    public interface IColaboradorRepository : IRepository<Colaborador>
    {
        Task<Colaborador?> ObterPorCpf(string cpf);
        Task<bool> CpfJaExiste(string cpf, int? id = null);
        Task<bool> TrocarSenha(int id, string novaSenha);
    }
}