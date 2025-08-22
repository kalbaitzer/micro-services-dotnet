// Importação dos namespaces necessários para as configurações.
// Camadas de Application e Infrastructure são referenciadas aqui para configurar a injeção de dependência.
using Microsoft.EntityFrameworkCore;
using PositionRisk.API.Workers;
using PositionRisk.Application.Interfaces;
using PositionRisk.Application.Services;
using PositionRisk.Infrastructure.Data;
using PositionRisk.Infrastructure.Repositories;

// Criação do construtor da aplicação web.
var builder = WebApplication.CreateBuilder(args);

// --- Seção 1: Configuração dos Serviços (Injeção de Dependência) ---

// Adiciona os serviços essenciais para uma API, como controllers e suporte ao Swagger.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// A string de conexão é lida do arquivo appsettings.json.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PositionRiskDbContext>(options => options.UseSqlServer(connectionString));

// Configura a injeção de dependência para as interfaces e suas implementações concretas.
builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<IPositionService, PositionService>();

// Configura o Background Service (RabbitMQ Consumer)
builder.Services.AddHostedService<RabbitMQConsumerService>();

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
