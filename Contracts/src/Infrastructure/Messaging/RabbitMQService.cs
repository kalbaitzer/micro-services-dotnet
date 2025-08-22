using Microsoft.Extensions.Configuration;
using Contracts.Application.Interfaces;
using RabbitMQ.Client;

namespace Contracts.Infrastructure.Messaging;

/// <summary>
/// Implementação concreta do serviço de mensageria utilizando RabbitMQ.
/// Esta classe é responsável por estabelecer a conexão com o broker RabbitMQ
/// e publicar mensagens em uma fila.
/// </summary>
public class RabbitMQService : IMessageBusService
{
     /// <summary>
    /// A conexão persistente com o servidor RabbitMQ.
    /// </summary>
    private readonly IConnection _connection;

    /// <summary>
    /// O canal de comunicação para executar operações de AMQP (como publicar mensagens).
    /// </summary>
    private readonly IModel _channel;

    /// <summary>
    /// Construtor da classe RabbitMQService.
    /// É responsável por ler as configurações, criar a fábrica de conexões e
    /// estabelecer uma conexão e um canal com o RabbitMQ.
    /// </summary>
    /// <param name="configuration">A interface de configuração para acessar dados do appsettings.json.</param>
    /// <exception cref="Exception">Lança uma exceção se a conexão com o RabbitMQ falhar.</exception>
    public RabbitMQService(IConfiguration configuration)
    {
        var factory = new ConnectionFactory()
        {
            HostName = configuration["RabbitMQ:HostName"],
            UserName = configuration["RabbitMQ:UserName"],
            Password = configuration["RabbitMQ:Password"]
        };

        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        catch (Exception ex)
        {
            // Registra a falha em log.
            Console.WriteLine($"--> Não foi possível se conectar ao RabbitMQ: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Publica uma mensagem em uma fila específica no RabbitMQ.
    /// Se a fila não existir, ela será criada como durável.
    /// </summary>
    /// <param name="queue">O nome da fila de destino.</param>
    /// <param name="message">A mensagem a ser publicada, como um array de bytes.</param>
    public void Publish(string queue, byte[] message)
    {
        // Garante que a fila existe antes de publicar
        _channel.QueueDeclare(queue: queue,
                              durable: true,      // A fila sobrevive à reinicialização do RabbitMQ
                              exclusive: false,   // Pode ser acessada por múltiplas conexões
                              autoDelete: false,  // Não é deletada quando o último consumidor se desconecta
                              arguments: null);

        // Publica a mensagem no "default exchange" com a routing key igual ao nome da fila.
        _channel.BasicPublish(exchange: "",
                              routingKey: queue,
                              basicProperties: null,
                              body: message);

        Console.WriteLine($"--> Mensagem publicada na fila: {queue}");
    }
}
