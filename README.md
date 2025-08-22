# Microsserviços com .NET e RabbitMQ

Este repositório contém um sistema de exemplo construído com dois microsserviços que se comunicam de forma assíncrona utilizando .NET e RabbitMQ.

## Arquitetura

- **Contracts Service**: Responsável por gerenciar o cadastro e o ciclo de vida de contratos de energia. Ao criar um novo contrato, ele publica um evento na fila.
- **Position & Risk Service**: Consome os eventos de criação de contrato da fila, processa esses dados e calcula uma posição de risco consolidada por mês.

## Tecnologias Utilizadas

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- RabbitMQ
- Docker (para a infraestrutura)

## Como Executar

1.  **Infraestrutura:** Suba os contêineres do SQL Server e RabbitMQ com o Docker Compose:
    ```bash
    # (Dentro da pasta Thunders.Contracts)
    docker-compose up -d
    ```

2.  **Serviço de Contratos:**
    ```bash
    cd Contracts
    dotnet run --project src/API/API.csproj
    ```

3.  **Serviço de Posição e Risco:**
    ```bash
    cd PositionRisk
    dotnet run --project src/API/API.csproj
    ```