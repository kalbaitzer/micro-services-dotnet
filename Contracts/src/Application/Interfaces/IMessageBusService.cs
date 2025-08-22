namespace Contracts.Application.Interfaces;

/// <summary>
/// Define a interface para um serviço de mensageria (Message Bus).
/// Esta abstração é responsável por desacoplar a lógica de publicação de mensagens
/// da implementação concreta de uma tecnologia de fila (como RabbitMQ, Azure Service Bus, etc.).
/// </summary>
public interface IMessageBusService
{
    /// <summary>
    /// Publica uma mensagem em uma fila específica.
    /// </summary>
    /// <param name="queue">O nome da fila de destino para a qual a mensagem será enviada.</param>
    /// <param name="message">A mensagem a ser publicada, serializada em um array de bytes.</param>
    void Publish(string queue, byte[] message);
}
