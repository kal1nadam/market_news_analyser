namespace NewsAnalyzer.Application.Entities;

public sealed class OutboxMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Type { get; set; } = default!;
    
    public string Payload { get; set; } = default!;
    
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? ProcessedAt { get; set; }
    
    public int Attempts { get; set; }
    
    public string? Error { get; set; }
    
    public DateTime? LockedAt { get; set; }
    
    public string? LockedBy { get; set; }
}