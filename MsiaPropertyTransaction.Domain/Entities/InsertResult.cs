namespace MsiaPropertyTransaction.Domain.Entities;

public class InsertResult
{
    public int RecordsInserted { get; set; }
    public List<string> Errors { get; set; } = new();
}
