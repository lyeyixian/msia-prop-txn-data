namespace MsiaPropertyTransaction.Domain.Entities;

public class PropertyTransaction
{
    public int Id { get; set; } // Primary key
    public required string PropertyType { get; set; }
    public required string District { get; set; }
    public required string Mukim { get; set; }
    public required string SchemeNameArea { get; set; }
    public string? RoadName { get; set; }
    public required string Tenure { get; set; }
    public decimal? LandParcelArea { get; set; }
    public string? Unit { get; set; }
    public required decimal MainFloorArea { get; set; }
    public string? UnitLevel { get; set; }
    public required string PropertyTypeStrata { get; set; }
    public required string Sector { get; set; }
    public required string State { get; set; }
    public required decimal TransactionPrice { get; set; }
    public required DateTime TransactionDate { get; set; }
}
