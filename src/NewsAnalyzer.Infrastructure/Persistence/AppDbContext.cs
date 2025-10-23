using Microsoft.EntityFrameworkCore;
using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Application.Entities;

namespace NewsAnalyzer.Infrastructure.Persistence;

public sealed class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<IngestionLog> IngestionLogs => Set<IngestionLog>();
    public DbSet<News> News => Set<News>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    
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
        
        modelBuilder.Entity<News>(b =>
        {
            b.ToTable("news");
            b.HasKey(x => x.Id);
            b.Property(x => x.PublishedAt).IsRequired();
            b.Property(x => x.Headline).HasMaxLength(500).IsRequired();
            b.Property(x => x.Summary).HasMaxLength(2000).IsRequired();
            b.Property(x => x.Url).HasMaxLength(1000).IsRequired();
            b.Property(x => x.CreatedAt).IsRequired();
            b.Property(x => x.Analyzed).IsRequired();
            b.Property(x => x.ImpactPercentage);
            b.Property(x => x.MarketTrend).HasMaxLength(100);
            b.Property(x => x.ReasonForMarketTrend).HasMaxLength(2000);
        });
        
        modelBuilder.Entity<OutboxMessage>(b =>
        {
            b.ToTable("outbox_messages");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedOnAdd();
            b.Property(x => x.Type).HasMaxLength(256).IsRequired();
            b.Property(x => x.Payload).IsRequired();
            b.Property(x => x.OccurredAt).IsRequired();
            b.Property(x => x.ProcessedAt);
        });
    }
}