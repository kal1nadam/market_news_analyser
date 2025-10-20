namespace NewsAnalyzer.Application.Entities;

public sealed class IngestionLog
{
    public int Id { get; set; }
    public string Source { get; set; } = default!;
    public DateTime CreatedUtc { get; set; }
    public string? Message { get; set; }
}