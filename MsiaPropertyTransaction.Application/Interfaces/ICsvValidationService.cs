using MsiaPropertyTransaction.Domain.Entities;

namespace MsiaPropertyTransaction.Application.Interfaces;

public interface ICsvValidationService
{
    List<string> ValidateCsvTransaction(CsvPropertyTransaction transaction);
    PropertyTransaction ConvertToEntity(CsvPropertyTransaction csvTransaction);
}
