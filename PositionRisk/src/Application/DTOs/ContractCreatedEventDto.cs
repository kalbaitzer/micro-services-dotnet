namespace PositionRisk.Application.DTOs;

/// <summary>
/// DTO (Data Transfer Object) para a mensagem que será recebida na fila do RabbitMQ.
/// </summary>
public class ContractCreatedEventDto
{
    /// <summary>
    /// Identificador único do evento.
    /// </summary>
    public Guid EventId { get; set; }
  
    /// <summary>
    /// Data e horário em que o evento foi criado.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Tipo do evento.
    /// </summary>
    public required string EventType { get; set; }

    /// <summary>
    /// Detalhes do contrato associado ao evento.
    /// </summary>
    public required ContractDetails ContractData { get; set; }
}

/// <summary>
/// DTO (Data Transfer Object) para os detalhes de um contrato.
/// </summary>
public class ContractDetails
{
    /// <summary>
    /// Identificador único para o contrato.
    /// </summary>
    public Guid ContractId { get; set; }

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
    /// Data e hora UTC em que o período do contrato começa.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Data e hora UTC em que o período do contrato termina.
    /// </summary>
    public DateTime EndDate { get; set; }
}