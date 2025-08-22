using Microsoft.EntityFrameworkCore;
using Contracts.Domain.Entities;

namespace Contracts.Infrastructure.Data;

/// <summary>
/// Representa a sessão com o banco de dados para a aplicação de Contratos.
/// Esta classe é a ponte principal entre as entidades de domínio e o banco de dados,
/// permitindo a consulta e o salvamento de dados.
/// </summary>
public class ContractsDbContext : DbContext
{
    /// <summary>
    /// Construtor da classe ContractsDbContext.
    /// É utilizado pelo sistema de injeção de dependência para passar as configurações do contexto,
    /// como a string de conexão, para a classe base do DbContext.
    /// </summary>
    /// <param name="options">As opções de configuração para este contexto.</param>
    public ContractsDbContext(DbContextOptions<ContractsDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Representa a coleção de entidades 'Contract' no contexto.
    /// Mapeia para a tabela de 'Contracts' no banco de dados e é usada para executar
    /// operações de CRUD (Criar, Ler, Atualizar, Excluir) nos contratos.
    /// </summary>
    public DbSet<Contract> Contracts { get; set; }
}
