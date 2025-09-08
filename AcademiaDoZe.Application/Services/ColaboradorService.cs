//Henrique Churkin Correia Alberton
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using AcademiaDoZe.Application.Mappings;
using AcademiaDoZe.Domain.Repositories;
using AcademiaDoZe.Application.Security;
namespace AcademiaDoZe.Application.Services
{
    public class ColaboradorService : IColaboradorService
    {
        private readonly Func<IColaboradorRepository> _repoFactory;
        public ColaboradorService(Func<IColaboradorRepository> repoFactory)
        {
            _repoFactory = repoFactory ?? throw new ArgumentNullException(nameof(repoFactory));
        }
        public async Task<ColaboradorDTO> AdicionarAsync(ColaboradorDTO colaboradorDto)
        {
            // Verifica se já existe um colaborador com o mesmo CPF
            if (await _repoFactory().CpfJaExiste(colaboradorDto.Cpf))

            {
                throw new InvalidOperationException($"Já existe um colaborador cadastrado com o CPF {colaboradorDto.Cpf}.");
            }
            // Hash da senha

            if (!string.IsNullOrWhiteSpace(colaboradorDto.Senha))

            {
                colaboradorDto.Senha = PasswordHasher.Hash(colaboradorDto.Senha);
            }
            // Cria a entidade de domínio a partir do DTO
            var colaborador = colaboradorDto.ToEntity();
            // Salva no repositório
            await _repoFactory().Adicionar(colaborador);
            // Retorna o DTO atualizado com o ID gerado
            return colaborador.ToDto();

        }
        public async Task<ColaboradorDTO> AtualizarAsync(ColaboradorDTO colaboradorDto)
        {
            // Verifica se o colaborador existe

            var colaboradorExistente = await _repoFactory().ObterPorId(colaboradorDto.Id) ?? throw new KeyNotFoundException($"Colaborador ID {colaboradorDto.Id} não encontrado.");

            // Verifica se o novo CPF já está em uso por outro colaborador

            if (await _repoFactory().CpfJaExiste(colaboradorDto.Cpf, colaboradorDto.Id))

            {
                throw new InvalidOperationException($"Já existe outro colaborador cadastrado com o CPF {colaboradorDto.Cpf}.");
            }
            // Se nova senha informada, aplicar hash

            if (!string.IsNullOrWhiteSpace(colaboradorDto.Senha))

            {
                colaboradorDto.Senha = PasswordHasher.Hash(colaboradorDto.Senha);
            }
            // a partir dos dados do dto e do existente, cria uma nova instância com os valores atualizados

            var colaboradorAtualizado = colaboradorExistente.UpdateFromDto(colaboradorDto);
            // Atualiza no repositório
            await _repoFactory().Atualizar(colaboradorAtualizado);
            return colaboradorAtualizado.ToDto();

        }

        public async Task<ColaboradorDTO> ObterPorIdAsync(int id)
        {
            var colaborador = await _repoFactory().ObterPorId(id);
            return (colaborador != null) ? colaborador.ToDto() : null!;

        }
        public async Task<IEnumerable<ColaboradorDTO>> ObterTodosAsync()
        {
            var colaboradores = await _repoFactory().ObterTodos();
            return [.. colaboradores.Select(c => c.ToDto())];

        }
        public async Task<bool> RemoverAsync(int id)
        {
            var colaborador = await _repoFactory().ObterPorId(id);

            if (colaborador == null)

            {
                return false;
            }
            await _repoFactory().Remover(id);

            return true;

        }
        public async Task<ColaboradorDTO> ObterPorCpfAsync(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                throw new ArgumentException("CPF não pode ser vazio.", nameof(cpf));
            cpf = new string([.. cpf.Where(char.IsDigit)]);
            var colaborador = await _repoFactory().ObterPorCpf(cpf);
            return (colaborador != null) ? colaborador.ToDto() : null!;

        }
        public async Task<bool> CpfJaExisteAsync(string cpf, int? id = null)
        {
            return await _repoFactory().CpfJaExiste(cpf, id);
        }
        public async Task<bool> TrocarSenhaAsync(int id, string novaSenha)
        {
            if (string.IsNullOrWhiteSpace(novaSenha))
                throw new ArgumentException("Nova senha inválida.", nameof(novaSenha));
            var hash = PasswordHasher.Hash(novaSenha);
            return await _repoFactory().TrocarSenha(id, hash);

        }
    }
}