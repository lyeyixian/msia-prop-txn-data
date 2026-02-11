using Microsoft.EntityFrameworkCore;
using MsiaPropertyTransaction.Domain.Entities;
using MsiaPropertyTransaction.Infrastructure.Data;
using Xunit;

namespace MsiaPropertyTransaction.Infrastructure.Tests;

public class PropertyTransactionModelTests
{
    [Fact]
    public void PropertyTransaction_HasCorrectProperties()
    {
        // Arrange
        var model = new PropertyTransaction
        {
            PropertyType = "Apartment",
            District = "Kuala Lumpur",
            Mukim = "Bukit Bintang",
            SchemeNameArea = "KLCC",
            RoadName = "Jalan Ampang",
            Tenure = "Freehold",
            LandParcelArea = 100.50m,
            Unit = "Unit 1",
            MainFloorArea = 200.00m,
            UnitLevel = "Level 10",
            PropertyTypeStrata = "Strata",
            Sector = "Commercial",
            State = "Selangor",
            TransactionPrice = 500000.00m,
            TransactionDate = new DateTime(2023, 1, 1)
        };

        // Assert
        Assert.Equal("Apartment", model.PropertyType);
        Assert.Equal("Kuala Lumpur", model.District);
        Assert.Equal("Bukit Bintang", model.Mukim);
        Assert.Equal("KLCC", model.SchemeNameArea);
        Assert.Equal("Jalan Ampang", model.RoadName);
        Assert.Equal("Freehold", model.Tenure);
        Assert.Equal(100.50m, model.LandParcelArea);
        Assert.Equal("Unit 1", model.Unit);
        Assert.Equal(200.00m, model.MainFloorArea);
        Assert.Equal("Level 10", model.UnitLevel);
        Assert.Equal("Strata", model.PropertyTypeStrata);
        Assert.Equal("Commercial", model.Sector);
        Assert.Equal("Selangor", model.State);
        Assert.Equal(500000.00m, model.TransactionPrice);
        Assert.Equal(new DateTime(2023, 1, 1), model.TransactionDate);
    }

    [Fact]
    public void DbContext_ConfiguresTableName()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        using var context = new AppDbContext(options);

        // Act
        var entityType = context.Model.FindEntityType(typeof(PropertyTransaction));

        // Assert
        Assert.NotNull(entityType);
        Assert.Equal("property_transactions", entityType.GetTableName());
    }

    [Fact]
    public void DbContext_ConfiguresColumnNames()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        using var context = new AppDbContext(options);

        // Act
        var entityType = context.Model.FindEntityType(typeof(PropertyTransaction));
        var propertyTypeProperty = entityType?.FindProperty("PropertyType");

        // Assert
        Assert.NotNull(entityType);
        Assert.NotNull(propertyTypeProperty);
        Assert.Equal("property_type", propertyTypeProperty.GetColumnName());
    }
}
