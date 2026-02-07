using System.Globalization;
using MsiaPropertyTransaction.Domain.Entities;
using MsiaPropertyTransaction.Services;
using Xunit;

namespace MsiaPropertyTransaction.Tests;

public class CsvValidationServiceTests
{
    private readonly CsvValidationService _service = new();

    [Fact]
    public void ValidateCsvTransaction_WithValidData_ReturnsNoErrors()
    {
        // Arrange
        var transaction = new CsvPropertyTransaction
        {
            PropertyType = "Apartment",
            District = "Kuala Lumpur",
            Mukim = "Bukit Bintang",
            SchemeNameArea = "KLCC",
            RoadName = "Jalan Ampang",
            Tenure = "Freehold",
            LandParcelArea = "100.50",
            Unit = "Unit 1",
            MainFloorArea = "200.00",
            UnitLevel = "Level 10",
            PropertyTypeStrata = "Strata",
            Sector = "Commercial",
            State = "Selangor",
            TransactionPrice = "500000.00",
            TransactionDate = "01/01/2023"
        };

        // Act
        var errors = _service.ValidateCsvTransaction(transaction);

        // Assert
        Assert.Empty(errors);
    }

    [Fact]
    public void ValidateCsvTransaction_WithMissingRequiredFields_ReturnsErrors()
    {
        // Arrange
        var transaction = new CsvPropertyTransaction
        {
            PropertyType = "", // Missing
            District = "Kuala Lumpur",
            Mukim = "Bukit Bintang",
            SchemeNameArea = "KLCC",
            Tenure = "Freehold",
            MainFloorArea = "200.00",
            PropertyTypeStrata = "Strata",
            Sector = "Commercial",
            State = "Selangor",
            TransactionPrice = "500000.00",
            TransactionDate = "01/01/2023"
        };

        // Act
        var errors = _service.ValidateCsvTransaction(transaction);

        // Assert
        Assert.Contains("Property Type is required", errors);
    }

    [Fact]
    public void ValidateCsvTransaction_WithInvalidNumericData_ReturnsErrors()
    {
        // Arrange
        var transaction = new CsvPropertyTransaction
        {
            PropertyType = "Apartment",
            District = "Kuala Lumpur",
            Mukim = "Bukit Bintang",
            SchemeNameArea = "KLCC",
            Tenure = "Freehold",
            LandParcelArea = "invalid_number",
            MainFloorArea = "200.00",
            PropertyTypeStrata = "Strata",
            Sector = "Commercial",
            State = "Selangor",
            TransactionPrice = "500000.00",
            TransactionDate = "01/01/2023"
        };

        // Act
        var errors = _service.ValidateCsvTransaction(transaction);

        // Assert
        Assert.Contains("Land/Parcel Area must be a valid number", errors);
    }

    [Fact]
    public void ValidateCsvTransaction_WithInvalidDateFormat_ReturnsErrors()
    {
        // Arrange
        var transaction = new CsvPropertyTransaction
        {
            PropertyType = "Apartment",
            District = "Kuala Lumpur",
            Mukim = "Bukit Bintang",
            SchemeNameArea = "KLCC",
            Tenure = "Freehold",
            MainFloorArea = "200.00",
            PropertyTypeStrata = "Strata",
            Sector = "Commercial",
            State = "Selangor",
            TransactionPrice = "500000.00",
            TransactionDate = "2023-01-01" // Wrong format
        };

        // Act
        var errors = _service.ValidateCsvTransaction(transaction);

        // Assert
        Assert.Contains("Transaction Date must be in dd/MM/yyyy format", errors);
    }

    [Fact]
    public void ValidateCsvTransaction_WithNullableFieldsAsEmptyOrDash_ValidatesCorrectly()
    {
        // Arrange
        var transaction = new CsvPropertyTransaction
        {
            PropertyType = "Apartment",
            District = "Kuala Lumpur",
            Mukim = "Bukit Bintang",
            SchemeNameArea = "KLCC",
            RoadName = "", // Empty string - valid for nullable
            Tenure = "Freehold",
            LandParcelArea = "-", // Dash - valid for nullable
            Unit = "-", // Dash - valid for nullable
            MainFloorArea = "200.00",
            UnitLevel = "", // Empty string - valid for nullable
            PropertyTypeStrata = "Strata",
            Sector = "Commercial",
            State = "Selangor",
            TransactionPrice = "500000.00",
            TransactionDate = "01/01/2023"
        };

        // Act
        var errors = _service.ValidateCsvTransaction(transaction);

        // Assert
        Assert.Empty(errors);
    }

    [Fact]
    public void ConvertToEntity_WithValidData_ReturnsEntity()
    {
        // Arrange
        var csvTransaction = new CsvPropertyTransaction
        {
            PropertyType = "Apartment",
            District = "Kuala Lumpur",
            Mukim = "Bukit Bintang",
            SchemeNameArea = "KLCC",
            RoadName = "Jalan Ampang",
            Tenure = "Freehold",
            LandParcelArea = "100.50",
            Unit = "Unit 1",
            MainFloorArea = "200.00",
            UnitLevel = "Level 10",
            PropertyTypeStrata = "Strata",
            Sector = "Commercial",
            State = "Selangor",
            TransactionPrice = "500000.00",
            TransactionDate = "01/01/2023"
        };

        // Act
        var entity = _service.ConvertToEntity(csvTransaction);

        // Assert
        Assert.Equal("Apartment", entity.PropertyType);
        Assert.Equal("Kuala Lumpur", entity.District);
        Assert.Equal("Bukit Bintang", entity.Mukim);
        Assert.Equal("KLCC", entity.SchemeNameArea);
        Assert.Equal("Jalan Ampang", entity.RoadName);
        Assert.Equal("Freehold", entity.Tenure);
        Assert.Equal(100.50m, entity.LandParcelArea);
        Assert.Equal("Unit 1", entity.Unit);
        Assert.Equal(200.00m, entity.MainFloorArea);
        Assert.Equal("Level 10", entity.UnitLevel);
        Assert.Equal("Strata", entity.PropertyTypeStrata);
        Assert.Equal("Commercial", entity.Sector);
        Assert.Equal("Selangor", entity.State);
        Assert.Equal(500000.00m, entity.TransactionPrice);
        Assert.Equal(new DateTime(2023, 1, 1), entity.TransactionDate);
    }

    [Fact]
    public void ConvertToEntity_WithNullableFieldsAsNullOrEmpty_ConvertsCorrectly()
    {
        // Arrange
        var csvTransaction = new CsvPropertyTransaction
        {
            PropertyType = "Apartment",
            District = "Kuala Lumpur",
            Mukim = "Bukit Bintang",
            SchemeNameArea = "KLCC",
            RoadName = "", // Empty string -> null
            Tenure = "Freehold",
            LandParcelArea = "-", // Dash -> null
            Unit = "-", // Dash -> null
            MainFloorArea = "200.00",
            UnitLevel = "", // Empty string -> null
            PropertyTypeStrata = "Strata",
            Sector = "Commercial",
            State = "Selangor",
            TransactionPrice = "500000.00",
            TransactionDate = "01/01/2023"
        };

        // Act
        var entity = _service.ConvertToEntity(csvTransaction);

        // Assert
        Assert.Null(entity.RoadName);
        Assert.Null(entity.LandParcelArea);
        Assert.Null(entity.Unit);
        Assert.Null(entity.UnitLevel);
    }
}