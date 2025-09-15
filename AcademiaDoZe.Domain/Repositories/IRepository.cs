// Henrique Churkin Correia Alberton

using AcademiaDoZe.Domain.Entities;

namespace AcademiaDoZe.Domain.Repositories;
public interface IRepository<TEntity> where TEntity : Entity
{
    Task<TEntity?> ObterPorId(int id);
    Task<IEnumerable<TEntity>> ObterTodos();
    Task<TEntity> Adicionar(TEntity entity);
    Task<TEntity> Atualizar(TEntity entity);
    Task<bool> Remover(int id);
}