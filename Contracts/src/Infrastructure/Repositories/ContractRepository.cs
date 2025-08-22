using Microsoft.EntityFrameworkCore;
using Contracts.Application.Interfaces;
using Contracts.Domain.Entities;
using Contracts.Infrastructure.Data;

namespace Contracts.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de contratos usando Entity Framework Core.
/// Esta classe lida diretamente com o DbContext para persistir os dados.
/// </summary>
public class ContractRepository : IContractRepository
{
    private readonly ContractsDbContext _context;

    public ContractRepository(ContractsDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Adiciona uma nova entidade de contrato ao repositório.
    /// </summary>
    /// <param name="contract">A entidade de contrato a ser adicionada.</param>
    public async Task AddAsync(Contract contract)
    {
        await _context.Contracts.AddAsync(contract);
    }

    /// <summary>
    /// Busca todos os contratos cadastrados.
    /// </summary>
    /// <returns>Uma lista de contratos cadastrados.</returns>
    public async Task<IEnumerable<Contract>> GetAllAsync()
    {
        // Obtém a lista de contratos cadastrados ordenados pelo campo counterparty.
        return await _context.Contracts
            .OrderBy(c => c.Counterparty)
            .ToListAsync();
    }

    /// <summary>
    /// Busca um contrato pelo seu identificador único (ID).
    /// </summary>
    /// <param name="id">O ID do contrato.</param>
    /// <returns>A entidade do contrato ou nulo se não for encontrado.</returns>
    public async Task<Contract?> GetByIdAsync(Guid id)
    {
        // FindAsync é otimizado para buscar uma entidade pela sua chave primária.
        return await _context.Contracts.FindAsync(id);
    }

    /// <summary>
    /// Salva todas as mudanças feitas no contexto de dados.
    /// <returns>O número de entradas de estado salvas no banco de dados.</returns>
    /// </summary>
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
