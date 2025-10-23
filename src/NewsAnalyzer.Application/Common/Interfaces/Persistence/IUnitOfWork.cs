namespace NewsAnalyzer.Application.Common.Interfaces.Persistence;

/// <summary>
/// Unit of Work interface for managing database transactions.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Serving purpose of committing all changes made in a transaction to the database.
    /// </summary>
    /// <param name="ct"> Cancellation token. </param>
    /// <returns> Entities changed. </returns>
    Task<int> SaveChangesAsync(CancellationToken ct);
}