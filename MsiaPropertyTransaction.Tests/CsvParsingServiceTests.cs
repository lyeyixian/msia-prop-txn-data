using System.Text;
using MsiaPropertyTransaction.Services;
using Xunit;

namespace MsiaPropertyTransaction.Tests;

public class CsvParsingServiceTests
{
    private readonly CsvParsingService _service = new();

    [Fact]
    public void ParseCsv_WithValidHeadersAndData_ReturnsTransactions()
    {
        // Arrange
        var csvContent = @"Property Type,District,Mukim,Scheme Name/Area,Road Name,Tenure,Land/Parcel Area,Unit,Main Floor Area,Unit Level,Property Type (strata),Sector,State,Transaction Price,Transaction Date
Apartment,Kuala Lumpur,Bukit Bintang,KLCC,Jalan Ampang,Freehold,100.50,Unit 1,200.00,Level 10,Strata,Commercial,Selangor,500000.00,01/01/2023";

        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));

        // Act
        var transactions = _service.ParseCsv(stream).ToList();

        // Assert
        Assert.Single(transactions);
        var transaction = transactions[0];
        Assert.Equal("Apartment", transaction.PropertyType);
        Assert.Equal("Kuala Lumpur", transaction.District);
        Assert.Equal("Bukit Bintang", transaction.Mukim);
        Assert.Equal("KLCC", transaction.SchemeNameArea);
        Assert.Equal("Jalan Ampang", transaction.RoadName);
        Assert.Equal("Freehold", transaction.Tenure);
        Assert.Equal("100.50", transaction.LandParcelArea);
        Assert.Equal("Unit 1", transaction.Unit);
        Assert.Equal("200.00", transaction.MainFloorArea);
        Assert.Equal("Level 10", transaction.UnitLevel);
        Assert.Equal("Strata", transaction.PropertyTypeStrata);
        Assert.Equal("Commercial", transaction.Sector);
        Assert.Equal("Selangor", transaction.State);
        Assert.Equal("500000.00", transaction.TransactionPrice);
        Assert.Equal("01/01/2023", transaction.TransactionDate);
    }

    [Fact]
    public void ParseCsv_WithDuplicateColumns_ResolvesCorrectly()
    {
        // Arrange - CSV with duplicate columns
        var csvContent = @"Property Type,District,Unit,Unit,Transaction Price,Transaction Price,Land/Parcel Area,Land Area,Transaction Date,Month, Year of Transaction Date,Main Floor Area,Main Floor Area
Apartment,Kuala Lumpur,Unit A,Unit B,1000000.00,2000000.00,150.00,250.00,01/01/2023,01/2023,02/01/2023,300.00,400.00";

        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));

        // Act
        var transactions = _service.ParseCsv(stream).ToList();

        // Assert
        Assert.Single(transactions);
        var transaction = transactions[0];
        Assert.Equal("Unit A", transaction.Unit); // Should pick first Unit column
        Assert.Equal("1000000.00", transaction.TransactionPrice); // Should pick first Transaction Price column
        Assert.Equal("150.00", transaction.LandParcelArea); // Should pick Land/Parcel Area over Land Area
        Assert.Equal("01/01/2023", transaction.TransactionDate); // Should pick Transaction Date over Month Year
        Assert.Equal("300.00", transaction.MainFloorArea); // Should pick first Main Floor Area
    }

    [Fact]
    public void ParseCsv_WithEmptyFile_ThrowsException()
    {
        // Arrange
        var csvContent = "";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _service.ParseCsv(stream).ToList());
    }

    [Fact]
    public void ParseCsv_WithOnlyHeaders_ThrowsException()
    {
        // Arrange
        var csvContent = "Property Type,District";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _service.ParseCsv(stream).ToList());
    }

    [Fact]
    public void ParseCsv_WithQuotedFields_HandlesCorrectly()
    {
        // Arrange
        var csvContent = @"Property Type,District,""Scheme Name/Area"",Road Name
Apartment,Kuala Lumpur,""KLCC, Tower"",Jalan Ampang";

        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));

        // Act
        var transactions = _service.ParseCsv(stream).ToList();

        // Assert
        Assert.Single(transactions);
        var transaction = transactions[0];
        Assert.Equal("KLCC, Tower", transaction.SchemeNameArea);
    }
}