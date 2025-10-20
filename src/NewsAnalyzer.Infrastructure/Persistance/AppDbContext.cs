using Microsoft.EntityFrameworkCore;

namespace NewsAnalyzer.Infrastructure.Persistance;

public sealed class AppDbContext : DbContext
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

public sealed class IngestionLog
{
    public int Id { get; set; }
    public string Source { get; set; } = default!;
    public DateTime CreatedUtc { get; set; }
    public string? Message { get; set; }
}