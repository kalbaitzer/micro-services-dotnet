# Microsserviços com .NET e RabbitMQ

![.NET](https://img.shields.io/badge/.NET-9-blueviolet)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2019-blue)
![RabbitMQ 3](https://img.shields.io/badge/RabbitMQ-3-orange)
![Docker](https://img.shields.io/badge/Docker-Ready-blue)
![Arquitetura](https://img.shields.io/badge/Arquitetura-Clean-green)
![Visual Studio Code](https://custom-icon-badges.demolab.com/badge/Visual%20Studio%20Code-0078d7.svg?logo=vsc&logoColor=white)

Este repositório contém um sistema de exemplo construído com dois microsserviços que se comunicam de forma assíncrona utilizando .NET e RabbitMQ.

## Arquitetura

- **Contracts Service**: Responsável por gerenciar o cadastro e o ciclo de vida de contratos de energia. Ao criar um novo contrato, ele publica um evento na fila.
- **Position & Risk Service**: Consome os eventos de criação de contrato da fila, processa esses dados e calcula uma posição de risco consolidada por mês.

## Tecnologias Utilizadas

- Backend: .NET 9, ASP.NET Core 9
- Persistência de Dados: Entity Framework Core 9
- Banco de Dados: SQL Server 2019
- Mensageria: RabbitMQ 3
- Arquitetura: Clean Architecture
- Conteinerização: Docker
- Versionamento de Código: Git e GitHub
- Teste da API: Rest Client for Visual Studio Code

## Diagrama Simplificado da Arquitetura

```
                              [API Gateway]
                                    |
+----------------------------------------------------------------------------------+
|                                                                                  |
|               1. POST /api/contracts                                             |
|               V                                                                  |
| [Microserviço de Contratos (.NET Core)] ---> [Banco SQL Server (Contracts)]      |
|               |                                                                  |
|               | 2. Publica evento "ContratoCriado"                               |
|               V                                                                  |
| +-------------------------- RabbitMQ --------------------------+                 |
| |             Exchange: 'contratos_exchange'                   |                 |
| |             |                                                |                 |
| |             Queue: 'posicao_queue'                           |                 |
| +--------------------------------------------------------------+                 |
|               |                                                                  |
|               | 3. Consome evento                                                |
|               V                                                                  |
| [Microserviço de Posição e Risco (.NET Core)] --> [Banco SQL Server (Position)]  |
|               ^                                                                  |
|               | 4. GET /api/positions/{ano}/{mes}                                |
|               |                                                                  |
| [Dashboards / Frontend]                                                          |
+----------------------------------------------------------------------------------+
```

## Como Executar

1.  **Infraestrutura:** Suba os contêineres do SQL Server e RabbitMQ com o Docker Compose:
    ```bash
    # (Dentro da pasta Contracts)
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

## Documentação das APIs

Ambos os serviços expõem uma documentação interativa via Swagger (OpenAPI), que permite visualizar e testar os endpoints diretamente pelo navegador.

- **Contracts Service Swagger UI:** `https://localhost:5000/swagger` (Ex: `https://localhost:7001/swagger`)
- **Position & Risk Service Swagger UI:** `https://localhost:5010/swagger` (Ex: `https://localhost:7002/swagger`)

*(Observação: A porta de cada serviço é definida no arquivo `launchSettings.json` de cada projeto API).*

### 1. Contracts Service (Serviço de Contratos)

URL Base: `/api/contracts`

---

#### `POST /api/contracts`

Cria um novo contrato de energia. Após a criação, publica um evento na fila `posicao_queue` para ser consumido pelo serviço de Posição e Risco.

**Request Body:**

```json
{
  "counterparty": "Nome da Contraparte",
  "type": "Compra",
  "volumeMwm": 50.5,
  "price": 250.75,
  "startDate": "2026-01-01T00:00:00Z",
  "endDate": "2026-12-31T23:59:59Z"
}
```
