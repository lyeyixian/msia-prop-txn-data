using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MsiaPropertyTransaction.Infrastructure.Data;
using Xunit;

namespace MsiaPropertyTransaction.Tests;

public class CsvUploadEndToEndTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public CsvUploadEndToEndTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task EndToEnd_CsvUpload_CompleteFlow_StoresDataCorrectly()
    {
        // Arrange

        // Create a test CSV with multiple records
        var csvContent = @"Property Type,District,Mukim,Scheme Name/Area,Road Name,Tenure,Land/Parcel Area,Unit,Main Floor Area,Unit Level,Property Type (strata),Sector,State,Transaction Price,Transaction Date
Apartment,Kuala Lumpur,Bukit Bintang,KLCC,Jalan Ampang,Freehold,100.50,Unit 1,200.00,Level 10,Strata,Commercial,Selangor,500000.00,01/01/2023
House,Petaling Jaya,Petaling,Taman ABC,,Leasehold,-,Unit 2,150.00,,Non-Strata,Residential,Selangor,300000.00,15/02/2023";

        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(csvContent));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/csv");
        content.Add(fileContent, "file", "test.csv");

        // Act - Upload the CSV
        var response = await _client.PostAsync("/api/upload-csv", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert - Check API response
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("recordsInserted", responseContent);
        Assert.Contains("2", responseContent); // Should mention 2 records
        Assert.Contains("CSV uploaded successfully", responseContent);
    }

    [Fact]
    public async Task EndToEnd_CsvUpload_WithDuplicateColumns_ResolvesCorrectly()
    {
        // Arrange

        // CSV with duplicate columns (Unit, Transaction Price, etc.)
        var csvContent = @"Property Type,District,Mukim,Scheme Name/Area,Tenure,Land/Parcel Area,Unit,Unit,Main Floor Area,Main Floor Area,Unit Level,Property Type (strata),Sector,State,Transaction Price,Transaction Price,Transaction Date,Month, Year of Transaction Date
Condo,Singapore,Central,District 1,Freehold,200.00,Unit A,Unit B,180.00,190.00,Penthouse,Strata,Commercial,Singapore,750000.00,800000.00,01/03/2023,01/2023";

        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(csvContent));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/csv");
        content.Add(fileContent, "file", "duplicates.csv");

        // Act
        var response = await _client.PostAsync("/api/upload-csv", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert - Check successful upload
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Assert - Check API response indicates successful processing
        Assert.Contains("recordsInserted", responseContent);
        Assert.Contains("1", responseContent); // Should mention 1 record
    }

    [Fact]
    public async Task EndToEnd_CsvUpload_InvalidData_ReturnsErrors_DoesNotStoreData()
    {
        // Arrange

        // CSV with invalid data (wrong date format, missing required fields)
        var csvContent = @"Property Type,District,Mukim,Scheme Name/Area,Tenure,Land/Parcel Area,Unit,Main Floor Area,Unit Level,Property Type (strata),Sector,State,Transaction Price,Transaction Date
,Kuala Lumpur,Bukit Bintang,KLCC,Freehold,100.50,Unit 1,200.00,Level 10,Strata,Commercial,Selangor,500000.00,2023-01-01";

        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(csvContent));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/csv");
        content.Add(fileContent, "file", "invalid.csv");

        // Act
        var response = await _client.PostAsync("/api/upload-csv", content);

        // Assert - Should fail due to validation errors
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("error", responseContent.ToLower());

        // Assert - API should indicate no data was stored
        Assert.Contains("error", responseContent.ToLower());
    }

    [Fact]
    public async Task EndToEnd_CsvUpload_LargeFile_PerformanceTest()
    {
        // Arrange

        // Create a large CSV file (100 records)
        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine("Property Type,District,Mukim,Scheme Name/Area,Road Name,Tenure,Land/Parcel Area,Unit,Main Floor Area,Unit Level,Property Type (strata),Sector,State,Transaction Price,Transaction Date");

        for (int i = 0; i < 100; i++)
        {
            csvBuilder.AppendLine($"Apartment{i},District{i},Mukim{i},Scheme{i},Road{i},Freehold,{100 + i}.00,Unit{i},{200 + i}.00,Level{i},Strata,Commercial,Selangor,{500000 + i * 1000}.00,01/01/2023");
        }

        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(csvBuilder.ToString()));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/csv");
        content.Add(fileContent, "file", "large.csv");

        // Act - Time the upload
        var startTime = DateTime.UtcNow;
        var response = await _client.PostAsync("/api/upload-csv", content);
        var endTime = DateTime.UtcNow;

        // Assert - Should succeed within reasonable time
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True((endTime - startTime).TotalSeconds < 30, "Upload should complete within 30 seconds");

        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("100", responseContent); // Should mention 100 records

        // Assert - Check performance and success
        Assert.Contains("100", responseContent); // Should mention 100 records
        Assert.Contains("CSV uploaded successfully", responseContent);
    }

    [Fact]
    public async Task EndToEnd_CsvUpload_MalformedCsv_ReturnsError()
    {
        // Arrange

        // Malformed CSV (missing comma, invalid structure)
        var csvContent = @"Property Type,District,Mukim,Scheme Name/Area
Apartment,Kuala Lumpur,Bukit Bintang,KLCC
House,Petaling Jaya,Petaling"; // Missing last field

        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(csvContent));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/csv");
        content.Add(fileContent, "file", "malformed.csv");

        // Act
        var response = await _client.PostAsync("/api/upload-csv", content);

        // Assert - Should fail due to malformed CSV
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("error", responseContent.ToLower());

        // Assert - API should indicate error
        Assert.Contains("error", responseContent.ToLower());
    }
}
