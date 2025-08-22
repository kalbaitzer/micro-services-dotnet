namespace PositionRisk.Application.DTOs;

/// <summary>
/// DTO (Data Transfer Object) que representa a visão consolidada da posição de energia 
/// para um determinado mês. Esta classe é usada para transferir os dados da posição 
/// calculada para os clientes da API (como um frontend).
/// </summary>
public class MonthlyPositionDto
{
    /// <summary>
    /// Identificador único do registro da posição mensal.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// O ano ao qual a posição se refere.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// O mês (1-12) ao qual a posição se refere.
    /// </summary>
    public int Month { get; set; }

    /// <summary>
    /// Volume total de energia comprada para o mês, em MWm.
    /// </summary>
    public decimal TotalVolumePurchased { get; set; }

    /// <summary>
    /// Volume total de energia vendida para o mês, em MWm.
    /// </summary>
    public decimal TotalVolumeSold { get; set; }

    /// <summary>
    /// A posição líquida para o mês (Comprado - Vendido).
    /// Um valor positivo indica uma posição 'comprada', e um valor negativo indica uma posição 'vendida'.
    /// </summary>
    public decimal NetPosition { get; set; }
}