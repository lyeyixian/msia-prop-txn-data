using MsiaPropertyTransaction.Domain.Entities;

namespace MsiaPropertyTransaction.Application.Interfaces;

public interface IPropertyTransactionRepository
{
    Task AddRangeAsync(IEnumerable<PropertyTransaction> transactions);
    Task SaveChangesAsync();
}
