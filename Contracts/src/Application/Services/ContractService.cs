using System.Text;
using System.Text.Json;
using Contracts.Application.DTOs;
using Contracts.Application.Interfaces;
using Contracts.Domain.Entities;

namespace Contracts.Application.Services;

/// <summary>
/// Implementação da Interface para o Serviço de Contratos.
/// </summary>
public class ContractService : IContractService
{
    private readonly IContractRepository _contractRepository;
    private readonly IMessageBusService _messageBus;

    /// <summary>
    /// Construtor do Serviço de Contratos.
    /// </summary>
    /// <param name="contractRepository">Repositório de contratos.</param>
    /// <param name="messageBus">Serviço der mensagens via RabbitMQ.</param>
    public ContractService(IContractRepository contractRepository, IMessageBusService messageBus)
    {
        _contractRepository = contractRepository;
        _messageBus = messageBus;
    }

    /// <summary>
    /// Adiciona um novo contrato.
    /// </summary>
    /// <param name="contract">A entidade de contrato a ser adicionada.</param>
     public async Task<Contract> CreateAsync(Contract contract)
    {
        // Adiciona o contrato através do repositório.
        await _contractRepository.AddAsync(contract);

        // Persiste as mudanças no banco de dados.
        await _contractRepository.SaveChangesAsync();

        // Prepara o evento com os dados da entidade que foi salva.
        var eventDto = new ContractCreatedEventDto(
            contract.Id,
            contract.Type,
            contract.VolumeMwm,
            contract.Price,
            contract.StartDate,
            contract.EndDate
        );

        var message = JsonSerializer.Serialize(eventDto);
        var body = Encoding.UTF8.GetBytes(message);

        // Publica o evento para a fila de posição/risco.
        _messageBus.Publish("posicao_queue", body);

        // Retorna o contrato recém-criado
        return contract;
    }

    /// <summary>
    /// Busca todos os contratos cadastradss.
    /// </summary>
    /// <returns>Uma lista de contratos.</returns>
    public async Task<IEnumerable<ContractDto>> GetAllAsync()
    {
        // Busca as entidades de domínio do repositório
        var contracts = await _contractRepository.GetAllAsync();

        // Mapeia a lista de entidades para uma lista de DTOs
        return contracts.Select(contract => new ContractDto
        {
            Id = contract.Id,
            Counterparty = contract.Counterparty,
            Type = contract.Type,
            VolumeMwm = contract.VolumeMwm,
            Price = contract.Price,
            Status = contract.Status
        });
    }

    /// <summary>
    /// Busca um contrato pelo seu ID.
    /// </summary>
    /// <param name="id">O ID do contrato a ser buscado.</param>
    /// <returns>Os dados do contrato.</returns>
    public async Task<Contract?> GetByIdAsync(Guid id)
    {
        return await _contractRepository.GetByIdAsync(id);
    }        
}
