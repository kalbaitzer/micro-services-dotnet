using Microsoft.EntityFrameworkCore;
using PositionRisk.Domain.Entities;

namespace PositionRisk.Infrastructure.Data;

/// <summary>
/// Representa a sessão com o banco de dados para a aplicação de Posição e Risco.
/// Esta classe é a ponte principal entre as entidades de domínio e o banco de dados,
/// permitindo a consulta e o salvamento de dados.
/// </summary>
public class PositionRiskDbContext : DbContext
{
    /// <summary>
    /// Construtor da classe ContractsDbContext.
    /// É utilizado pelo sistema de injeção de dependência para passar as configurações do contexto,
    /// como a string de conexão, para a classe base do DbContext.
    /// </summary>
    /// <param name="options">As opções de configuração para este contexto.</param>
    public PositionRiskDbContext(DbContextOptions<PositionRiskDbContext> options) : base(options) {}

    /// <summary>
    /// Representa a coleção de entidades 'MonthlyPositions' no contexto.
    /// Mapeia para a tabela de 'MonthlyPositions' no banco de dados e é usada para executar
    /// o processamento de eventos de criação de novos contratos.
    /// </summary>
    public DbSet<MonthlyPosition> MonthlyPositions { get; set; }

    /// <summary>
    /// Configura o modelo de dados para o DbContext usando a API Fluente do EF Core.
    /// Neste caso, é usado para definir um índice composto único.
    /// </summary>
    /// <param name="modelBuilder">O construtor usado para criar o modelo.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Garante que a combinação de Ano/Mês seja única
        modelBuilder.Entity<MonthlyPosition>()
            .HasIndex(p => new { p.Year, p.Month })
            .IsUnique();
    }
}