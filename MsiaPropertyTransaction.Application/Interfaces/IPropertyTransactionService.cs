using MsiaPropertyTransaction.Domain.Entities;

namespace MsiaPropertyTransaction.Application.Interfaces;

public interface IPropertyTransactionService
{
    Task<InsertResult> InsertTransactionsAsync(IEnumerable<PropertyTransaction> transactions);
}
