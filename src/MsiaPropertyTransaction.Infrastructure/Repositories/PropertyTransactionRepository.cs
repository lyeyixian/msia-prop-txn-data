using Microsoft.EntityFrameworkCore;
using MsiaPropertyTransaction.Application.Interfaces;
using MsiaPropertyTransaction.Domain.Entities;

namespace MsiaPropertyTransaction.Infrastructure.Repositories;

public class PropertyTransactionRepository : IPropertyTransactionRepository
{
    private readonly Data.AppDbContext _context;

    public PropertyTransactionRepository(Data.AppDbContext context)
    {
        _context = context;
    }

    public async Task AddRangeAsync(IEnumerable<PropertyTransaction> transactions)
    {
        await _context.PropertyTransactions.AddRangeAsync(transactions);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
