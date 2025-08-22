using Contracts.Domain.Entities;
using Contracts.Application.DTOs;

namespace Contracts.Application.Interfaces;

/// <summary>
/// Interface para o Serviço de Contratos.
/// </summary>
public interface IContractService
{
    /// <summary>
    /// Adiciona um novo contrato.
    /// </summary>
    /// <param name="contract">A entidade de contrato a ser adicionada.</param>
    Task<Contract> CreateAsync(Contract contract);

    /// <summary>
    /// Busca todos os contratos cadastradss.
    /// </summary>
    /// <returns>Uma lista de contratos.</returns>
    Task<IEnumerable<ContractDto>> GetAllAsync();

    /// <summary>
    /// Busca um contrato pelo seu ID.
    /// </summary>
    /// <param name="id">O ID do contrato a ser buscado.</param>
    /// <returns>Os dados do contrato.</returns>
    Task<Contract?> GetByIdAsync(Guid id);
}
