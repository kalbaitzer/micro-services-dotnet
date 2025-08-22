using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using PositionRisk.Application.DTOs;
using PositionRisk.Application.Interfaces;

namespace PositionRisk.API.Workers;

/// <summary>
/// Um serviço de background que consome mensagens da fila do RabbitMQ de forma contínua.
/// Ele herda de 'BackgroundService', garantindo que seja iniciado e parado junto com a aplicação.
/// </summary>
public class RabbitMQConsumerService : BackgroundService
{
    /// <summary>
    /// A conexão persistente com o servidor RabbitMQ.
    /// </summary>
    private readonly IConnection _connection;

    /// <summary>
    /// O canal de comunicação para executar operações AMQP.
    /// </summary>
    private readonly IModel _channel;

    /// <summary>
    /// Factory para criar escopos de injeção de dependência. Essencial para resolver serviços com tempo de vida 'Scoped'
    /// (como o DbContext e o PositionService) dentro de um serviço Singleton como este.
    /// </summary>
    private readonly IServiceScopeFactory _scopeFactory;

    /// <summary>
    /// O nome da fila que este consumidor irá ouvir.
    /// </summary>
    private const string QueueName = "posicao_queue";

    /// <summary>
    /// Construtor do serviço consumidor.
    /// É responsável por receber as dependências e estabelecer a conexão com o RabbitMQ.
    /// </summary>
    /// <param name="configuration">Para ler as configurações de conexão do RabbitMQ.</param>
    /// <param name="scopeFactory">Injetado para permitir a criação de escopos de serviço para cada mensagem.</param>
    public RabbitMQConsumerService(IConfiguration configuration, IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        var factory = new ConnectionFactory()
        {
            HostName = configuration["RabbitMQ:HostName"],
            UserName = configuration["RabbitMQ:UserName"],
            Password = configuration["RabbitMQ:Password"]
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Garante que a fila existe no RabbitMQ antes de começar a consumir.
        _channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
    }

    /// <summary>
    /// Método principal do serviço de background. É executado quando o serviço é iniciado.
    /// Configura o consumidor para receber e processar mensagens da fila.
    /// </summary>
    /// <param name="stoppingToken">Um token que sinaliza quando o processo de desligamento da aplicação foi iniciado.</param>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        // Define o callback que será executado para cada mensagem recebida.
        consumer.Received += async (sender, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var eventDto = JsonSerializer.Deserialize<ContractCreatedEventDto>(message);

            if (eventDto != null)
            {
                // Criamos um 'escopo' de injeção de dependência para cada mensagem
                // Isso garante que os serviços (como o DbContext) tenham o tempo de vida correto
                using (var scope = _scopeFactory.CreateScope())
                {
                    var positionService = scope.ServiceProvider.GetRequiredService<IPositionService>();
                    await positionService.ProcessContractEventAsync(eventDto);
                }
            }

            // Confirma o recebimento e processamento da mensagem para removê-la da fila
            _channel.BasicAck(eventArgs.DeliveryTag, false);
        };

        // Inicia o consumo da fila. O 'autoAck' está como 'false' para garantirmos
        // que a mensagem só seja removida da fila após o processamento bem-sucedido.
        _channel.BasicConsume(QueueName, false, consumer);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Realiza a limpeza dos recursos (conexão e canal) quando a aplicação é encerrada.
    /// </summary>
    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}