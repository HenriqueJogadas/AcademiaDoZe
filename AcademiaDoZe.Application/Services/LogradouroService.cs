//Henrique Churkin Correia Alberton
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using AcademiaDoZe.Application.Mappings;
using AcademiaDoZe.Domain.Repositories;
namespace AcademiaDoZe.Application.Services
{
    public class LogradouroService : ILogradouroService
    {
        // Func que cria instâncias do repositório quando necessário, garantindo freshness e evitando problemas de ciclo de vida
        // freshness indica que cada chamada cria uma nova instância do repositório, evitando problemas de ciclo de vida
        private readonly Func<ILogradouroRepository> _repoFactory;
        public LogradouroService(Func<ILogradouroRepository> repoFactory)
        {
            _repoFactory = repoFactory ?? throw new ArgumentNullException(nameof(repoFactory));
        }
        public async Task<LogradouroDTO> AdicionarAsync(LogradouroDTO logradouroDto)
        {
            // Verifica se já existe um logradouro com o mesmo CEP

            var cepExistente = await _repoFactory().ObterPorCep(logradouroDto.Cep);
            if (cepExistente != null)

            {
                throw new InvalidOperationException($"Logradouro com ID {cepExistente.Id}, já cadastrado com o CEP {cepExistente.CEP}.");
            }
            // Cria a entidade de domínio a partir do DTO
            var logradouro = logradouroDto.ToEntity();
            // Salva no repositório
            await _repoFactory().Adicionar(logradouro);
            // converte e retorna o DTO - já com o ID gerado, setado pelo repositório
            return logradouro.ToDto();

        }
        public async Task<LogradouroDTO> AtualizarAsync(LogradouroDTO logradouroDto)
        {
            // Verifica se o logradouro existe

            var logradouroExistente = await _repoFactory().ObterPorId(logradouroDto.Id) ?? throw new KeyNotFoundException($"Logradouro ID {logradouroDto.Id} não encontrado.");

            // Verifica se o novo CEP já está em uso por outro logradouro

            if (!string.Equals(logradouroExistente.CEP, logradouroDto.Cep, StringComparison.OrdinalIgnoreCase))

            {
                var cepExistente = await _repoFactory().ObterPorCep(logradouroDto.Cep);
                if (cepExistente != null && cepExistente.Id != logradouroDto.Id)

                {
                    throw new InvalidOperationException($"Logradouro com ID {cepExistente.Id}, já cadastrado com o CEP {cepExistente.CEP}.");
                }
            }
            // a partir dos dados do dto e do existente, cria uma nova instância com os valores atualizados

            // respeitando o principio de Imutabilidade, onde a entidade original não é modificada, mas sim uma nova instância é criada com os dados atualizados
            var logradouroAtualizado = logradouroExistente.UpdateFromDto(logradouroDto);
            // Atualiza no repositório
            await _repoFactory().Atualizar(logradouroAtualizado);
            return logradouroAtualizado.ToDto();

        }

        public async Task<LogradouroDTO> ObterPorIdAsync(int id)
        {
            var logradouro = await _repoFactory().ObterPorId(id);
            return (logradouro != null) ? logradouro.ToDto() : null!;

        }
        public async Task<IEnumerable<LogradouroDTO>> ObterTodosAsync()
        {
            var logradouros = await _repoFactory().ObterTodos();

            return [.. logradouros.Select(l => l.ToDto())]; // expressão de interpolação para criar uma nova lista de DTOs

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
            // Remove formatação do CEP, mantendo apenas dígitos
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
    }
}
