using Microsoft.EntityFrameworkCore;
using MsiaPropertyTransaction.Application.Services;
using MsiaPropertyTransaction.Domain.Entities;
using MsiaPropertyTransaction.Infrastructure.Data;
using MsiaPropertyTransaction.Infrastructure.Repositories;
using MsiaPropertyTransaction.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register infrastructure services
builder.Services.AddScoped<IPropertyTransactionRepository, PropertyTransactionRepository>();

// Register application services
builder.Services.AddScoped<ICsvParsingService, CsvParsingService>();
builder.Services.AddScoped<ICsvValidationService, CsvValidationService>();
builder.Services.AddScoped<IPropertyTransactionService, PropertyTransactionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/api/upload-csv", async (HttpContext context, ICsvParsingService parsingService, ICsvValidationService validationService, IPropertyTransactionService transactionService) =>
{
    try
    {
        IFormFile? file;
        try
        {
            var form = await context.Request.ReadFormAsync();
            file = form.Files.GetFile("file");
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { error = "Invalid form data: " + ex.Message });
        }

        if (file == null || file.Length == 0)
        {
            return Results.BadRequest(new { error = "No file uploaded or file is empty" });
        }

        // Validate file type
        if (!file.ContentType.Equals("text/csv", StringComparison.OrdinalIgnoreCase) &&
            !file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
        {
            return Results.BadRequest(new { error = "File must be a CSV file" });
        }

        // Parse CSV
        await using var stream = file.OpenReadStream();
        var csvTransactions = parsingService.ParseCsv(stream).ToList();

        if (!csvTransactions.Any())
        {
            return Results.BadRequest(new { error = "CSV file must contain at least one data row" });
        }

        // Validate and convert data
        var validEntities = new List<PropertyTransaction>();
        var validationErrors = new List<string>();

        foreach (var csvTransaction in csvTransactions)
        {
            var errors = validationService.ValidateCsvTransaction(csvTransaction);
            if (errors.Any())
            {
                validationErrors.AddRange(errors);
                continue; // Skip invalid rows for now, or we could stop processing
            }

            try
            {
                var entity = validationService.ConvertToEntity(csvTransaction);
                validEntities.Add(entity);
            }
            catch (Exception ex)
            {
                validationErrors.Add($"Error converting row: {ex.Message}");
            }
        }

        if (validationErrors.Any() && !validEntities.Any())
        {
            return Results.BadRequest(new
            {
                error = "All rows contain validation errors",
                details = validationErrors.Take(10).ToArray() // Limit error details
            });
        }

        // Insert into database
        var insertResult = await transactionService.InsertTransactionsAsync(validEntities);

        // Return response
        if (insertResult.Errors.Any())
        {
            return Results.BadRequest(new
            {
                error = "Some records failed to insert",
                recordsInserted = insertResult.RecordsInserted,
                errors = insertResult.Errors
            });
        }

        return Results.Ok(new
        {
            message = "CSV uploaded successfully",
            recordsInserted = insertResult.RecordsInserted,
            validationErrors = validationErrors.Any() ? validationErrors : null
        });
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Internal server error: {ex.Message}");
    }
});

app.Run();
