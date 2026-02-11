using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MsiaPropertyTransaction.Application.Services;
using MsiaPropertyTransaction.Domain.Entities;
using MsiaPropertyTransaction.Infrastructure.Data;
using MsiaPropertyTransaction.Infrastructure.Repositories;
using Xunit;

namespace MsiaPropertyTransaction.Infrastructure.Tests;

public class PropertyTransactionServiceTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly PropertyTransactionService _service;

    public PropertyTransactionServiceTests()
    {
        var serviceProvider = new ServiceCollection()
            .AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("TestDb_PropertyTransaction"))
            .BuildServiceProvider();

        _context = serviceProvider.GetRequiredService<AppDbContext>();
        var repository = new PropertyTransactionRepository(_context);
        _service = new PropertyTransactionService(repository);

        // Ensure database is created
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task InsertTransactions_WithValidData_InsertsSuccessfully()
    {
        // Arrange
        var transactions = new List<PropertyTransaction>
        {
            new PropertyTransaction
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
            }
        };

        // Act
        var result = await _service.InsertTransactionsAsync(transactions);

        // Assert
        Assert.Equal(1, result.RecordsInserted);
        Assert.Empty(result.Errors);
        Assert.Equal(1, await _context.PropertyTransactions.CountAsync());

        var inserted = await _context.PropertyTransactions.FirstAsync();
        Assert.Equal("Apartment", inserted.PropertyType);
        Assert.Equal(500000.00m, inserted.TransactionPrice);
    }

    [Fact]
    public async Task InsertTransactions_WithNullableFields_InsertsSuccessfully()
    {
        // Arrange
        var transactions = new List<PropertyTransaction>
        {
            new PropertyTransaction
            {
                PropertyType = "House",
                District = "Petaling Jaya",
                Mukim = "Petaling",
                SchemeNameArea = "Taman ABC",
                RoadName = null, // nullable field
                Tenure = "Leasehold",
                LandParcelArea = null, // nullable field
                Unit = null, // nullable field
                MainFloorArea = 150.00m,
                UnitLevel = null, // nullable field
                PropertyTypeStrata = "Non-Strata",
                Sector = "Residential",
                State = "Selangor",
                TransactionPrice = 300000.00m,
                TransactionDate = new DateTime(2023, 2, 1)
            }
        };

        // Act
        var result = await _service.InsertTransactionsAsync(transactions);

        // Assert
        Assert.Equal(1, result.RecordsInserted);
        Assert.Empty(result.Errors);

        var inserted = await _context.PropertyTransactions.FirstAsync();
        Assert.Null(inserted.RoadName);
        Assert.Null(inserted.LandParcelArea);
        Assert.Null(inserted.Unit);
        Assert.Null(inserted.UnitLevel);
    }

    [Fact]
    public async Task InsertTransactions_WithLargeBatch_ProcessesInBatches()
    {
        // Arrange - Create a large batch (more than typical batch size)
        var transactions = new List<PropertyTransaction>();
        for (int i = 0; i < 1500; i++) // More than default batch size of 1000
        {
            transactions.Add(new PropertyTransaction
            {
                PropertyType = $"Type{i}",
                District = $"District{i}",
                Mukim = $"Mukim{i}",
                SchemeNameArea = $"Scheme{i}",
                Tenure = "Freehold",
                MainFloorArea = 100.00m,
                PropertyTypeStrata = "Strata",
                Sector = "Residential",
                State = "Selangor",
                TransactionPrice = 100000.00m + i,
                TransactionDate = new DateTime(2023, 1, 1).AddDays(i % 365)
            });
        }

        // Act
        var result = await _service.InsertTransactionsAsync(transactions);

        // Assert
        Assert.Equal(1500, result.RecordsInserted);
        Assert.Empty(result.Errors);
        Assert.Equal(1500, await _context.PropertyTransactions.CountAsync());
    }

    [Fact]
    public async Task InsertTransactions_WithDuplicateData_HandlesGracefully()
    {
        // Arrange - Insert same data twice (if we had unique constraints, this would test)
        var transactions1 = new List<PropertyTransaction>
        {
            new PropertyTransaction
            {
                PropertyType = "Apartment",
                District = "Kuala Lumpur",
                Mukim = "Bukit Bintang",
                SchemeNameArea = "KLCC",
                Tenure = "Freehold",
                MainFloorArea = 200.00m,
                PropertyTypeStrata = "Strata",
                Sector = "Commercial",
                State = "Selangor",
                TransactionPrice = 500000.00m,
                TransactionDate = new DateTime(2023, 1, 1)
            }
        };

        var transactions2 = new List<PropertyTransaction>
        {
            new PropertyTransaction
            {
                PropertyType = "Apartment",
                District = "Kuala Lumpur",
                Mukim = "Bukit Bintang",
                SchemeNameArea = "KLCC",
                Tenure = "Freehold",
                MainFloorArea = 200.00m,
                PropertyTypeStrata = "Strata",
                Sector = "Commercial",
                State = "Selangor",
                TransactionPrice = 500000.00m,
                TransactionDate = new DateTime(2023, 1, 1)
            }
        };

        // Act - Insert twice with different instances
        await _service.InsertTransactionsAsync(transactions1);
        var result = await _service.InsertTransactionsAsync(transactions2);

        // Assert - Should handle duplicates gracefully (in current schema, no unique constraints)
        Assert.Equal(1, result.RecordsInserted);
        Assert.Empty(result.Errors);
        Assert.Equal(2, await _context.PropertyTransactions.CountAsync());
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
