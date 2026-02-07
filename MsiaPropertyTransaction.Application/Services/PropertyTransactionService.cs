using MsiaPropertyTransaction.Application.Interfaces;
using MsiaPropertyTransaction.Domain.Entities;

namespace MsiaPropertyTransaction.Application.Services;

public class PropertyTransactionService : IPropertyTransactionService
{
    private readonly IPropertyTransactionRepository _repository;
    private const int BatchSize = 1000;

    public PropertyTransactionService(IPropertyTransactionRepository repository)
    {
        _repository = repository;
    }

    public async Task<InsertResult> InsertTransactionsAsync(IEnumerable<PropertyTransaction> transactions)
    {
        var result = new InsertResult();
        var batches = transactions.Chunk(BatchSize);

        foreach (var batch in batches)
        {
            try
            {
                await _repository.AddRangeAsync(batch);
                await _repository.SaveChangesAsync();
                result.RecordsInserted += batch.Length;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Batch insertion failed: {ex.Message}");
            }
        }

        return result;
    }
}
