using PositionRisk.Domain.Entities;

namespace PositionRisk.Application.Interfaces;

/// <summary>
/// Define o contrato para o repositório da entidade 'MonthlyPosition'.
/// Esta interface abstrai as operações de persistência de dados, permitindo que a camada de aplicação
/// interaja com os dados sem conhecer os detalhes da implementação do banco de dados.
/// </summary>
public interface IPositionRepository
{
    /// <summary>
    /// Busca uma entidade 'MonthlyPosition' por ano e mês. Se nenhuma for encontrada,
    /// cria uma nova instância em memória, a adiciona ao contexto e a retorna.
    /// </summary>
    /// <param name="year">O ano da posição a ser buscada ou criada.</param>
    /// <param name="month">O mês da posição a ser buscada ou criada.</param>
    /// <returns>A instância da posição mensal, seja ela existente ou recém-criada.</returns>
    Task<MonthlyPosition> GetOrCreateAsync(int year, int month);

    /// <summary>
    /// Persiste no banco de dados todas as alterações rastreadas pelo DbContext.
    /// </summary>
    /// <returns>O número de registros de estado afetados no banco de dados.</returns>
    Task<int> SaveChangesAsync();
}