namespace NewsAnalyzer.Application.Entities;

/// <summary>
/// Domain entity.
/// Database table: OutboxMessages
/// </summary>
public sealed class OutboxMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public required string Type { get; set; } = default!;
    
    public required string Payload { get; set; } = default!;
    
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? ProcessedAt { get; set; }
    
    public int Attempts { get; set; }
    
    public string? Error { get; set; }
    
    public DateTime? LockedAt { get; set; }
    
    public string? LockedBy { get; set; }
    
    // Methods to update the state of the outbox message
    public void MarkProcessed() => ProcessedAt = DateTime.UtcNow;
    
    public void IncrementAttempts() => Attempts++;
    
    public void SetError(string error) => Error = error;

}