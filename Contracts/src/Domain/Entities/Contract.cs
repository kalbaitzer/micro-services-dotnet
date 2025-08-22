using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contracts.Domain.Entities;

/// <summary>
/// Representa um contrato de comércio de energia no sistema.
/// Esta é a entidade central do domínio Contratos.
/// </summary>
public class Contract
{
    /// <summary>
    /// Identificador único para o contrato.
    /// Primary Key.
    /// </summary>
    [Key]
    public Guid Id { get; private set; }

    /// <summary>
    /// Nome ou identificador da outra parte no contrato.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Counterparty { get; private set; }

    /// <summary>
    /// Define o tipo da transação: "Compra" ou "Venda".
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Type { get; private set; }

    /// <summary>
    /// Quantidade de energia contratada, medido em MW médio (MWm).
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18, 4)")]
    public decimal VolumeMwm { get; private set; }

    /// <summary>
    /// Preço acordado para a energia, geralmente em R$/MWh.
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; private set; }

    /// <summary>
    /// Data e hora UTC em que o período do contrato começa.
    /// </summary>
    [Required]
    public DateTime StartDate { get; private set; }

    /// <summary>
    /// Data e hora UTC em que o período do contrato termina.
    /// </summary>
    [Required]
    public DateTime EndDate { get; private set; }

    /// <summary>
    /// Data e hora UTC em que o contrato foi criado no sistema.
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// O status atual do contrato: "Ativo", "Cancelado", "Interrompido".
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Status { get; private set; }

    /// <summary>
    /// Construtor privado sem parâmetros exigido pelo EF Core para materialização.
    /// </summary>
    private Contract()
    {
        this.Type = "";
        this.Status = "";
        this.Counterparty = "";
    }

    /// <summary>
    /// Construtor primário para criar uma nova instância de contrato válida.
    /// Ele inicializa a entidade com todos os dados necessários e define seu estado inicial.
    /// </summary>
    /// <param name="counterparty">A outra parte do contrato.</param>
    /// <param name="type">O tipo de contrato ("Compra" ou "Venda").</param>
    /// <param name="volumeMwm">O volume de energia contratado.</param>
    /// <param name="price">O preço acordado.</param>
    /// <param name="startDate">Data de início do contrato.</param>
    /// <param name="endDate">Data final do contrato.</param>
    public Contract(string counterparty, string type, decimal volumeMwm, decimal price, DateTime startDate, DateTime endDate)
    {
        Id = Guid.NewGuid();
        Counterparty = counterparty;
        Type = type;
        VolumeMwm = volumeMwm;
        Price = price;
        StartDate = startDate.ToUniversalTime();
        EndDate = endDate.ToUniversalTime();
        CreatedAt = DateTime.UtcNow;
        Status = "Ativo"; // Default status na criação
    }
}
