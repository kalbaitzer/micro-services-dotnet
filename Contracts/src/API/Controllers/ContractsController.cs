using Microsoft.AspNetCore.Mvc;
using Contracts.Application.DTOs;
using Contracts.Application.Interfaces;
using Contracts.Domain.Entities;

namespace Contracts.API.Controllers;

/// <summary>
/// Controller responsável por gerenciar os endpoints relacionados a Contratos.
/// </summary>
[ApiController]
[Route("api/[controller]")]  // Rota base: /api/contracts
public class ContractsController : ControllerBase
{
    private readonly IContractService _contractService;

    /// <summary>
    /// Construtor do Controller de Contratos.
    /// </summary>
    /// <param name="contractService">Serviço de Contratos.</param>
    public ContractsController(IContractService contractService)
    {
        _contractService = contractService;
    }

    /// <summary>
    /// Cria um novo contrato em banco de dados.
    /// </summary>
    /// <param name="dto">Os dados do novo contrato.</param>
    /// <returns>Os detalhes do contrato recém-criado.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Contract), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateContract([FromBody] CreateContractDto dto)
    {
        try
        {
            // Validação de negócio básica no controller.
            if (dto.EndDate <= dto.StartDate)
            {
                return BadRequest("A data de fim deve ser posterior à data de início.");
            }

            // Mapeamento do DTO para a entidade de domínio.
            var contract = new Contract(
                dto.Counterparty,
                dto.Type,
                dto.VolumeMwm,
                dto.Price,
                dto.StartDate,
                dto.EndDate
            );

            // Novo contrato criado no banco de dados
            var createdContract = await _contractService.CreateAsync(contract);

            // Retorna o status 201 Created com a localização do novo recurso.
            return CreatedAtAction(nameof(GetContractById), new { id = createdContract.Id }, createdContract);
        }
        catch (Exception e)
        {
            return StatusCode(500,new { error = e.Message });
        }
    }

    /// <summary>
    /// Lista todos os contratos cadastrados.
    /// </summary>
    /// <returns>Uma lista de contratos.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ContractDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetContracts()
    {
        try
        {
            // Chama a camada de serviço para buscar todos os contratos cadastrados
            var contractDtos = await _contractService.GetAllAsync();

            // Verifica se a lista é nula
            if (contractDtos == null)
            {
                // Se for retorna HTTP 404 Not Found
                return NotFound();
            }

            // Retorna HTTP 200 OK com a lista de contratos.
            // Se não houver contratos, retornará uma lista vazia [], o que é o comportamento correto.
            return Ok(contractDtos);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }

    /// <summary>
    /// Busca um contrato pelo seu ID.
    /// </summary>
    /// <param name="id">O ID do contrato a ser buscado.</param>
    /// <returns>Os dados do contrato.</returns>
    [HttpGet("{id:guid}")] // Adicionamos a restrição de tipo :guid
    [ProducesResponseType(typeof(Contract), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetContractById(Guid id)
    {
        try
        {
            // Chama a camada de serviço para buscar o contrato
            var contract = await _contractService.GetByIdAsync(id);

            // Verifica se o contrato foi encontrado
            if (contract == null)
            {
                // Se não foi, retorna HTTP 404 Not Found
                return NotFound();
            }

            // Se foi encontrado, retorna HTTP 200 OK com os dados do contrato no corpo da resposta
            return Ok(contract);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }
}
