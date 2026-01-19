using Microsoft.EntityFrameworkCore;
using MsiaPropertyTransaction.Data;
using MsiaPropertyTransaction.Models;

namespace MsiaPropertyTransaction.Services;

public class PropertyTransactionService
{
    private readonly AppDbContext _context;
    private const int BatchSize = 1000; // Process in batches of 1000 records

    public PropertyTransactionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<InsertResult> InsertTransactionsAsync(IEnumerable<PropertyTransaction> transactions)
    {
        var result = new InsertResult();
        var batches = transactions.Chunk(BatchSize);

        foreach (var batch in batches)
        {
            try
            {
                await _context.PropertyTransactions.AddRangeAsync(batch);
                await _context.SaveChangesAsync();
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

public class InsertResult
{
    public int RecordsInserted { get; set; }
    public List<string> Errors { get; set; } = new();
}
