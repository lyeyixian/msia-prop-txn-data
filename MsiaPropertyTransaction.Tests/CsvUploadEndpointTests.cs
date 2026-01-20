using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace MsiaPropertyTransaction.Tests;

public class CsvUploadEndpointTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public CsvUploadEndpointTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task UploadCsv_WithValidData_ReturnsSuccess()
    {
        // Arrange
        var csvContent = @"Property Type,District,Mukim,Scheme Name/Area,Road Name,Tenure,Land/Parcel Area,Unit,Main Floor Area,Unit Level,Property Type (strata),Sector,State,Transaction Price,Transaction Date
Apartment,Kuala Lumpur,Bukit Bintang,KLCC,Jalan Ampang,Freehold,100.50,Unit 1,200.00,Level 10,Strata,Commercial,Selangor,500000.00,01/01/2023";

        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(csvContent));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/csv");
        content.Add(fileContent, "file", "test.csv");

        // Act
        var response = await _client.PostAsync("/api/upload-csv", content);

        // Debug: Show response details
        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Content: {responseContent}");

        // Assert
        if (response.StatusCode != HttpStatusCode.OK)
        {
            Assert.Fail($"Expected OK but got {response.StatusCode}. Response: {responseContent}");
        }
        // Should contain success information
        Assert.Contains("records", responseContent.ToLower());
        Assert.Contains("success", responseContent.ToLower());
    }

    [Fact]
    public async Task UploadCsv_WithInvalidCsv_ReturnsBadRequest()
    {
        // Arrange - CSV with missing required columns
        var csvContent = @"Property Type,District
Apartment,Kuala Lumpur";

        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(csvContent));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/csv");
        content.Add(fileContent, "file", "invalid.csv");

        // Act
        var response = await _client.PostAsync("/api/upload-csv", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("error", responseContent.ToLower());
    }

    [Fact]
    public async Task UploadCsv_WithEmptyFile_ReturnsBadRequest()
    {
        // Arrange
        var csvContent = "";

        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(csvContent));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/csv");
        content.Add(fileContent, "file", "empty.csv");

        // Act
        var response = await _client.PostAsync("/api/upload-csv", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UploadCsv_WithNoFile_ReturnsBadRequest()
    {
        // Arrange
        var content = new MultipartFormDataContent();

        // Act
        var response = await _client.PostAsync("/api/upload-csv", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UploadCsv_WithWrongContentType_ReturnsBadRequest()
    {
        // Arrange
        var csvContent = "some text content";

        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(csvContent));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/plain"); // Wrong content type
        content.Add(fileContent, "file", "test.txt");

        // Act
        var response = await _client.PostAsync("/api/upload-csv", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UploadCsv_WithLargeValidFile_ReturnsSuccess()
    {
        // Arrange - Create a larger CSV with multiple rows
        var csvContent = new StringBuilder();
        csvContent.AppendLine("Property Type,District,Mukim,Scheme Name/Area,Road Name,Tenure,Land/Parcel Area,Unit,Main Floor Area,Unit Level,Property Type (strata),Sector,State,Transaction Price,Transaction Date");

        for (int i = 0; i < 100; i++)
        {
            csvContent.AppendLine($"Apartment{i},District{i},Mukim{i},Scheme{i},Road{i},Freehold,100.{i},Unit{i},200.{i},Level{i},Strata,Commercial,Selangor,50000{i}.00,01/01/2023");
        }

        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(csvContent.ToString()));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/csv");
        content.Add(fileContent, "file", "large.csv");

        // Act
        var response = await _client.PostAsync("/api/upload-csv", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("100", responseContent); // Should mention 100 records
    }
}