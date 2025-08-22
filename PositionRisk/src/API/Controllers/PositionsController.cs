using Microsoft.AspNetCore.Mvc;
using PositionRisk.Application.DTOs;
using PositionRisk.Application.Interfaces;

namespace PositionRisk.API.Controllers;

/// <summary>
/// Controller responsável por gerenciar os endpoints relacionados a Posição e Risco.
/// </summary>
[ApiController]
[Route("api/[controller]")]  // Rota base: /api/positions/{ano}/{mês}
public class PositionsController : ControllerBase
{
    private readonly IPositionService _positionService;

    /// <summary>
    /// Construtor do Controller de Posição e Risco.
    /// </summary>
    /// <param name="positionService">Serviço de Posição e Risco.</param>
    public PositionsController(IPositionService positionService)
    {
        _positionService = positionService;
    }

    /// <summary>
    /// Obtém a posição consolidada entre contratos comprados e vendidos para o mês solicitado.
    /// </summary>
    /// <param name="year">Ano referente a posição consolidada.</param>
    /// <param name="month">Mês referente a posição consolidada.</param>
    /// <returns>A posição consolidada para o mês.</returns>
    [HttpGet("{year}/{month}")]
    [ProducesResponseType(typeof(MonthlyPositionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByMonth(int year, int month)
    {
        try
        {
            var position = await _positionService.GetByMonthAsync(year, month);

            // Verifica se a posição foi encontrada
            if (position == null)
            {
                // Se não foi, retorna HTTP 404 Not Found
                return NotFound();
            }

            // Se foi encontrada, retorna HTTP 200 OK com os dados da posição
            return Ok(position);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }
}