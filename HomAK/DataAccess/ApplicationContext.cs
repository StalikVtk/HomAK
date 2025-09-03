using HomAK.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HomAK.DataAccess
{
  /// <summary>
  /// База данных.
  /// </summary>
  internal class ApplicationContext : DbContext
  {

    #region Поля

    public DbSet<Count> Counts { get; set; }

    public DbSet<Operation> Operations { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Envelope> Envelopes { get; set; }

    public DbSet<EnvelopeDeposit> envelopeDeposits { get; set; }

    #endregion

    #region Методы

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .SetBasePath(Directory.GetCurrentDirectory())
        .Build();

      optionsBuilder.UseSqlite(config.GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
    }

    #endregion

    #region Контсрукторы

    public ApplicationContext()
      : base()
    {
      Database.EnsureCreated();
    }

    #endregion

  }
}
