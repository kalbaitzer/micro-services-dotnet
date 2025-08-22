using Contracts.Domain.Entities;

namespace Contracts.Application.Interfaces;

/// <summary>
/// Define o contrato para operações de persistência da entidade Contract.
/// A camada de aplicação depende desta abstração, não de uma implementação concreta.
/// </summary>
public interface IContractRepository
{
    /// <summary>
    /// Adiciona uma nova entidade de contrato ao repositório.
    /// </summary>
    /// <param name="contract">A entidade de contrato a ser adicionada.</param>
    Task AddAsync(Contract contract);

    /// <summary>
    /// Busca todos os contratos cadastrados.
    /// </summary>
    /// <returns>Uma lista de contratos cadastrados.</returns>
    Task<IEnumerable<Contract>> GetAllAsync();

    /// <summary>
    /// Busca um contrato pelo seu identificador único (ID).
    /// </summary>
    /// <param name="id">O ID do contrato.</param>
    /// <returns>A entidade do contrato ou nulo se não for encontrado.</returns>
    Task<Contract?> GetByIdAsync(Guid id);

    /// <summary>
    /// Salva todas as mudanças feitas no contexto de dados.
    /// <returns>O número de entradas de estado salvas no banco de dados.</returns>
    /// </summary>
    Task<int> SaveChangesAsync();
}
