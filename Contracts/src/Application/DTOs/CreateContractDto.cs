using System.ComponentModel.DataAnnotations;

namespace Contracts.Application.DTOs;

// DTO (Data Transfer Object) para receber os dados de criação de um novo contrato via API.
public class CreateContractDto
{
    /// <summary>
    /// Nome ou identificador da outra parte no contrato.
    /// </summary>
    [Required]
    public required string Counterparty { get; set; }
    
    /// <summary>
    /// Define o tipo da transação: "Compra" ou "Venda".
    /// </summary>
    [Required]
    public required string Type { get; set; } // "Compra" ou "Venda"
    
    /// <summary>
    /// Quantidade de energia contratada, medido em MW médio (MWm).
    /// </summary>
    [Required]
    [Range(0.1, double.MaxValue)]
    public decimal VolumeMwm { get; set; }
    
    /// <summary>
    /// Preço acordado para a energia, geralmente em R$/MWh.
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }
    
    /// <summary>
    /// Data e hora UTC em que o período do contrato começa.
    /// </summary>
    [Required]
    public DateTime StartDate { get; set; }
    
    /// <summary>
    /// Data e hora UTC em que o período do contrato termina.
    /// </summary>
    [Required]
    public DateTime EndDate { get; set; }
}
