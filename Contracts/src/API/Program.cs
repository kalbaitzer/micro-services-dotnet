// Importação dos namespaces necessários para as configurações.
// Camadas de Application e Infrastructure são referenciadas aqui para configurar a injeção de dependência.
using Microsoft.EntityFrameworkCore;
using Contracts.Application.Interfaces;
using Contracts.Application.Services;
using Contracts.Infrastructure.Data;
using Contracts.Infrastructure.Messaging;
using Contracts.Infrastructure.Repositories;

// Criação do construtor da aplicação web.
var builder = WebApplication.CreateBuilder(args);

// --- Seção 1: Configuração dos Serviços (Injeção de Dependência) ---

// Adiciona os serviços essenciais para uma API, como controllers e suporte ao Swagger.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura o Entity Framework Core para usar o SQL Server.
// A string de conexão é lida do arquivo appsettings.json.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ContractsDbContext>(options => options.UseSqlServer(connectionString));

// Configura a injeção de dependência para as interfaces e suas implementações concretas.
// Isso desacopla as camadas da aplicação, seguindo o Princípio da Inversão de Dependência.

// Registra o serviço de aplicação. 'Scoped' significa que uma nova instância será criada para cada requisição HTTP.
builder.Services.AddScoped<IContractService, ContractService>();

// Registra o serviço de mensageria. 'Singleton' garante que haverá apenas uma instância
// deste serviço durante todo o ciclo de vida da aplicação, o que é ideal para gerenciar a conexão com o RabbitMQ.
builder.Services.AddSingleton<IMessageBusService, RabbitMQService>();

// Registra o repositório. Sempre que uma classe pedir por 'IContractRepository',
// o container de DI fornecerá uma instância de 'ContractRepository'.
builder.Services.AddScoped<IContractRepository, ContractRepository>();

// --- Seção 2: Construção e Configuração do Pipeline HTTP ---

// Constrói a aplicação com os serviços configurados acima.
var app = builder.Build();

// Configura o pipeline de requisições HTTP. A ordem dos middlewares é importante.
// Em ambiente de desenvolvimento, habilita a interface do Swagger para facilitar testes.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redireciona requisições HTTP para HTTPS para maior segurança.
app.UseHttpsRedirection();

// Adiciona o middleware de autorização ao pipeline.
app.UseAuthorization();

// Mapeia as rotas definidas nos controllers para que a API saiba como responder às requisições.
app.MapControllers();

// --- Seção 3: Execução da Aplicação ---

// Inicia a aplicação e a faz ouvir por requisições HTTP.
app.Run();