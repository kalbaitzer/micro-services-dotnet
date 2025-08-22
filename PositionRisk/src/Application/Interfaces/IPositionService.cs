using PositionRisk.Application.DTOs;

namespace PositionRisk.Application.Interfaces;

/// <summary>
/// Define o contrato para o serviço de lógica de negócio relacionado à posição de risco.
/// Esta interface orquestra as operações, atuando como uma fachada entre a camada de apresentação
/// (API, Workers) e a camada de acesso a dados (Repositórios).
/// </summary>
public interface IPositionService
{
    /// <summary>
    /// Busca a posição consolidada para um ano e mês específicos.
    /// </summary>
    /// <param name="year">O ano da posição desejada.</param>
    /// <param name="month">O mês da posição desejada.</param>
    /// <returns>
    /// Um DTO com os dados da posição mensal ou 'nulo' se nenhuma posição for encontrada.
    /// </returns>
    Task<MonthlyPositionDto?> GetByMonthAsync(int year, int month);

    /// <summary>
    /// Processa um evento de criação de contrato.
    /// Esta operação lê os dados do evento, itera sobre os meses de vigência do contrato
    /// e atualiza a posição consolidada para cada mês correspondente.
    /// </summary>
    /// <param name="eventDto">Dados do evento de criação de contrato a ser processado.</param>
    Task ProcessContractEventAsync(ContractCreatedEventDto eventDto);
}