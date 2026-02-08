using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MsiaPropertyTransaction.Application.Services;
using MsiaPropertyTransaction.Domain.Entities;
using MsiaPropertyTransaction.Infrastructure.Data;
using MsiaPropertyTransaction.Infrastructure.Repositories;
using MsiaPropertyTransaction.Infrastructure.Services;
using MsiaPropertyTransaction.Infrastructure.Validation;
using MsiaPropertyTransaction.Application.Interfaces;
using MsiaPropertyTransaction.Application.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure S3 Storage Settings
builder.Services.Configure<S3StorageSettings>(builder.Configuration.GetSection("S3Storage"));

// Validate S3 configuration
var s3Settings = builder.Configuration.GetSection("S3Storage").Get<S3StorageSettings>();
if (s3Settings != null)
{
    S3ConfigurationValidator.Validate(s3Settings);
}

// Register infrastructure services
builder.Services.AddScoped<IPropertyTransactionRepository, PropertyTransactionRepository>();
builder.Services.AddSingleton<IS3StorageService>(sp =>
{
    var s3Settings = sp.GetRequiredService<IOptions<S3StorageSettings>>().Value;
    return new S3StorageService(
        s3Settings.ServiceUrl,
        s3Settings.Region,
        s3Settings.AccessKey,
        s3Settings.SecretKey
    );
});

// Register application services
builder.Services.AddScoped<ICsvParsingService, CsvParsingService>();
builder.Services.AddScoped<ICsvValidationService, CsvValidationService>();
builder.Services.AddScoped<IPropertyTransactionService, PropertyTransactionService>();
builder.Services.AddScoped<CsvProcessingService>();

// Register LocalStack initializer for development
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<LocalStackInitializer>();
}

var app = builder.Build();

// Initialize LocalStack bucket in development
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var initializer = scope.ServiceProvider.GetRequiredService<LocalStackInitializer>();
    await initializer.InitializeAsync();
}

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

app.MapPost("/api/process-s3-csv", async (ProcessS3CsvRequest request, CsvProcessingService processingService) =>
{
    try
    {
        var result = await processingService.ProcessFromS3Async(request.BucketName, request.FileKey);

        if (result.HasErrors)
        {
            return Results.BadRequest(new
            {
                error = "Processing completed with errors",
                recordsProcessed = result.RecordsProcessed,
                recordsInserted = result.RecordsInserted,
                validationErrors = result.ValidationErrors.Any() ? result.ValidationErrors : null,
                insertErrors = result.InsertErrors.Any() ? result.InsertErrors : null
            });
        }

        return Results.Ok(new
        {
            message = "CSV processed successfully from S3",
            recordsProcessed = result.RecordsProcessed,
            recordsInserted = result.RecordsInserted
        });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
    catch (FileNotFoundException ex)
    {
        return Results.NotFound(new { error = ex.Message });
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

public record ProcessS3CsvRequest(string BucketName, string FileKey);
