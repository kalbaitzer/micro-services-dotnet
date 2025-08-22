using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PositionRisk.Domain.Entities;

/// <summary>
/// Representa a posição consolidada de energia para um determinado mês/ano.
/// Esta é a entidade agregadora deste microserviço, responsável por manter o estado
/// consolidado das negociações de energia.
/// </summary>
public class MonthlyPosition
{
    /// <summary>
    /// Identificador único (Chave Primária) do registro da posição mensal.
    /// </summary>
    [Key]
    public Guid Id { get; private set; }

    /// <summary>
    /// O ano ao qual a posição se refere.
    /// </summary>
    public int Year { get; private set; }

    /// <summary>
    /// O mês (1-12) ao qual a posição se refere.
    /// </summary>
    public int Month { get; private set; }

    /// <summary>
    /// Volume total de energia comprada para o mês, em MWm.
    /// </summary>
    [Column(TypeName = "decimal(18, 4)")]
    public decimal TotalVolumePurchased { get; private set; }

    /// <summary>
    /// Volume total de energia vendida para o mês, em MWm.
    /// </summary>
    [Column(TypeName = "decimal(18, 4)")]
    public decimal TotalVolumeSold { get; private set; }

    /// <summary>
    /// Propriedade computada que representa a posição líquida para o mês (Comprado - Vendido).
    /// Este campo não é mapeado no banco de dados.
    /// </summary>
    [Column(TypeName = "decimal(18, 4)")]
    public decimal NetPosition => TotalVolumePurchased - TotalVolumeSold;

    /// <summary>
    /// Construtor privado para uso exclusivo do Entity Framework Core.
    /// </summary>
    private MonthlyPosition() {}

    /// <summary>
    /// Construtor público para criar uma nova instância de posição mensal,
    /// inicializando os valores padrão.
    /// </summary>
    /// <param name="year">O ano da nova posição.</param>
    /// <param name="month">O mês da nova posição.</param>
    public MonthlyPosition(int year, int month)
    {
        Id = Guid.NewGuid();
        Year = year;
        Month = month;
        TotalVolumePurchased = 0;
        TotalVolumeSold = 0;
    }

    /// <summary>
    /// Atualiza os volumes de compra ou venda com base nos dados de um contrato.
    /// Este método contém a regra de negócio central para a agregação de posições.
    /// </summary>
    /// <param name="contractType">O tipo do contrato ("Purchase" ou "Sale").</param>
    /// <param name="volumeMwm">O volume de energia do contrato a ser somado.</param>
    public void UpdatePosition(string contractType, decimal volumeMwm)
    {
        if (contractType.Equals("Compra", StringComparison.OrdinalIgnoreCase))
        {
            // Atualiza o total de volume comprado
            TotalVolumePurchased += volumeMwm;
        }
        else if (contractType.Equals("Venda", StringComparison.OrdinalIgnoreCase))
        {
            // Atualiza o total de volume vendido
            TotalVolumeSold += volumeMwm;
        }
    }
}