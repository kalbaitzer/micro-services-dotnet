using PositionRisk.Application.DTOs;
using PositionRisk.Application.Interfaces;

namespace PositionRisk.Application.Services;

/// <summary>
/// Implementação da Interface para o Serviço de Posição e Risco.
/// </summary>
public class PositionService : IPositionService
{
    private readonly IPositionRepository _repository;

    /// <summary>
    /// Construtor do Serviço de Posição e Risco.
    /// </summary>
    /// <param name="repository">Repositório de posição e risco.</param>
    public PositionService(IPositionRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Busca a posição consolidada para um ano e mês específicos.
    /// </summary>
    /// <param name="year">O ano da posição desejada.</param>
    /// <param name="month">O mês da posição desejada.</param>
    /// <returns>
    /// Um DTO com os dados da posição mensal ou 'nulo' se nenhuma posição for encontrada.
    /// </returns>
    public async Task<MonthlyPositionDto?> GetByMonthAsync(int year, int month)
    {
        // Busca a posição referente ao ano e mês solicitado no repositório
        var position = await _repository.GetOrCreateAsync(year, month);

        // Retorna a posição
        return new MonthlyPositionDto
        {
            Id = position.Id,
            Year = position.Year,
            Month = position.Month,
            TotalVolumePurchased = position.TotalVolumePurchased,
            TotalVolumeSold = position.TotalVolumeSold,
            NetPosition = position.NetPosition
        };
    }

    /// <summary>
    /// Processa um evento de criação de contrato.
    /// Esta operação lê os dados do evento, itera sobre os meses de vigência do contrato
    /// e atualiza a posição consolidada para cada mês correspondente.
    /// </summary>
    /// <param name="eventDto">Dados do evento de criação de contrato a ser processado.</param>
    public async Task ProcessContractEventAsync(ContractCreatedEventDto eventDto)
    {
        var contract = eventDto.ContractData;
        var currentDate = contract.StartDate;

        // Itera por cada mês de vigência do contrato
        while (currentDate < contract.EndDate)
        {
            var year = currentDate.Year;
            var month = currentDate.Month;

            // Busca a posição do mês no banco. Se não existir, cria uma nova.
            var position = await _repository.GetOrCreateAsync(year, month);

            // Atualiza a posição com os dados do contrato
            position.UpdatePosition(contract.Type, contract.VolumeMwm);

            // Avança a data atual para o próximo mês
            currentDate = currentDate.AddMonths(1);
        }

        // Salva todas as alterações de uma vez
        await _repository.SaveChangesAsync();
    }
}