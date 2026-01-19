using System.Globalization;

namespace MsiaPropertyTransaction.Models;

public class CsvPropertyTransaction
{
    public string? PropertyType { get; set; }
    public string? District { get; set; }
    public string? Mukim { get; set; }
    public string? SchemeNameArea { get; set; }
    public string? RoadName { get; set; }
    public string? Tenure { get; set; }
    public string? LandParcelArea { get; set; }
    public string? Unit { get; set; }
    public string? MainFloorArea { get; set; }
    public string? UnitLevel { get; set; }
    public string? PropertyTypeStrata { get; set; }
    public string? Sector { get; set; }
    public string? State { get; set; }
    public string? TransactionPrice { get; set; }
    public string? TransactionDate { get; set; }
}