using Microsoft.EntityFrameworkCore;
using PositionRisk.Application.Interfaces;
using PositionRisk.Domain.Entities;
using PositionRisk.Infrastructure.Data;

namespace PositionRisk.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório da entidade 'MonthlyPosition'. usando Entity Framework Core.
/// Esta classe lida diretamente com o DbContext para persistir os dados.
/// </summary>
public class PositionRepository : IPositionRepository
{
    private readonly PositionRiskDbContext _context;

    public PositionRepository(PositionRiskDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Busca uma entidade 'MonthlyPosition' por ano e mês. Se nenhuma for encontrada,
    /// cria uma nova instância em memória, a adiciona ao contexto e a retorna.
    /// </summary>
    /// <param name="year">O ano da posição a ser buscada ou criada.</param>
    /// <param name="month">O mês da posição a ser buscada ou criada.</param>
    /// <returns>A instância da posição mensal, seja ela existente ou recém-criada.</returns>
    public async Task<MonthlyPosition> GetOrCreateAsync(int year, int month)
    {
        var position = await _context.MonthlyPositions
            .FirstOrDefaultAsync(p => p.Year == year && p.Month == month);

        if (position == null)
        {
            position = new MonthlyPosition(year, month);
            await _context.MonthlyPositions.AddAsync(position);
        }

        return position;
    }

    /// <summary>
    /// Persiste no banco de dados todas as alterações rastreadas pelo DbContext.
    /// </summary>
    /// <returns>O número de registros de estado afetados no banco de dados.</returns>
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}