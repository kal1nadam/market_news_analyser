using Microsoft.EntityFrameworkCore;
using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Application.Entities;

namespace NewsAnalyzer.Infrastructure.Persistence;

public sealed class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<IngestionLog> IngestionLogs => Set<IngestionLog>();
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IngestionLog>(b =>
        {
            b.ToTable("ingestion_logs");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedOnAdd();
            b.Property(x => x.Source).HasMaxLength(256).IsRequired();
            b.Property(x => x.CreatedUtc).IsRequired();
            b.Property(x => x.Message).HasMaxLength(2000);
        });
    }
}