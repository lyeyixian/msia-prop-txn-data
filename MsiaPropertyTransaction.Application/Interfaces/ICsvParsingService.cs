using MsiaPropertyTransaction.Domain.Entities;

namespace MsiaPropertyTransaction.Application.Interfaces;

public interface ICsvParsingService
{
    IEnumerable<CsvPropertyTransaction> ParseCsv(Stream csvStream);
}
