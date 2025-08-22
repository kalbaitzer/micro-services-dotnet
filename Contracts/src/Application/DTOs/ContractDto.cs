namespace Contracts.Application.DTOs;

/// <summary>
/// DTO (Data Transfer Object) para retornar os dados de um contrato para o cliente.
/// </summary>
public class ContractDto
{
    /// <summary>
    /// Identificador único para o contrato.
    /// </summary>
	public Guid Id { get; set; }

    /// <summary>
    /// Nome ou identificador da outra parte no contrato.
    /// </summary>
	public required string Counterparty { get; set; }

    /// <summary>
    /// Define o tipo da transação: "Compra" ou "Venda".
    /// </summary>
	public required string Type { get; set; }

    /// <summary>
    /// Quantidade de energia contratada, medido em MW médio (MWm).
    /// </summary>
	public decimal VolumeMwm { get; set; }

    /// <summary>
    /// Preço acordado para a energia, geralmente em R$/MWh.
    /// </summary>
	public decimal Price { get; set; }

    /// <summary>
    /// O status atual do contrato: "Ativo", "Cancelado", "Interrompido".
    /// </summary>
	public required string Status { get; set; }
}
