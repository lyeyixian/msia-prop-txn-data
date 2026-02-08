using MsiaPropertyTransaction.Application.Interfaces;
using MsiaPropertyTransaction.Domain.Entities;

namespace MsiaPropertyTransaction.Application.Services;

public class CsvProcessingService
{
    private readonly IS3StorageService _s3StorageService;
    private readonly ICsvParsingService _csvParsingService;
    private readonly ICsvValidationService _csvValidationService;
    private readonly IPropertyTransactionService _propertyTransactionService;

    public CsvProcessingService(
        IS3StorageService s3StorageService,
        ICsvParsingService csvParsingService,
        ICsvValidationService csvValidationService,
        IPropertyTransactionService propertyTransactionService)
    {
        _s3StorageService = s3StorageService ?? throw new ArgumentNullException(nameof(s3StorageService));
        _csvParsingService = csvParsingService ?? throw new ArgumentNullException(nameof(csvParsingService));
        _csvValidationService = csvValidationService ?? throw new ArgumentNullException(nameof(csvValidationService));
        _propertyTransactionService = propertyTransactionService ?? throw new ArgumentNullException(nameof(propertyTransactionService));
    }

    public async Task<CsvProcessingResult> ProcessFromS3Async(
        string bucketName, 
        string fileKey, 
        CancellationToken cancellationToken = default)
    {
        // Validate S3 path parameters
        S3PathValidator.ValidateS3Path(bucketName, fileKey);

        // Check if file exists in S3
        var fileExists = await _s3StorageService.FileExistsAsync(bucketName, fileKey, cancellationToken);
        if (!fileExists)
        {
            throw new FileNotFoundException($"File '{fileKey}' not found in bucket '{bucketName}'");
        }

        // Get file stream from S3
        using var stream = await _s3StorageService.GetFileStreamAsync(bucketName, fileKey, cancellationToken);

        // Parse CSV from stream
        var csvTransactions = _csvParsingService.ParseCsv(stream).ToList();

        if (!csvTransactions.Any())
        {
            throw new InvalidOperationException("CSV file must contain at least one data row");
        }

        // Validate and convert data
        var validEntities = new List<PropertyTransaction>();
        var validationErrors = new List<string>();

        foreach (var csvTransaction in csvTransactions)
        {
            var errors = _csvValidationService.ValidateCsvTransaction(csvTransaction);
            if (errors.Any())
            {
                validationErrors.AddRange(errors);
                continue;
            }

            try
            {
                var entity = _csvValidationService.ConvertToEntity(csvTransaction);
                validEntities.Add(entity);
            }
            catch (Exception ex)
            {
                validationErrors.Add($"Error converting row: {ex.Message}");
            }
        }

        if (validationErrors.Any() && !validEntities.Any())
        {
            throw new InvalidOperationException($"All rows contain validation errors: {string.Join(", ", validationErrors.Take(10))}");
        }

        // Insert into database
        var insertResult = await _propertyTransactionService.InsertTransactionsAsync(validEntities);

        return new CsvProcessingResult
        {
            RecordsProcessed = validEntities.Count,
            RecordsInserted = insertResult.RecordsInserted,
            ValidationErrors = validationErrors,
            InsertErrors = insertResult.Errors.ToList()
        };
    }
}

public class CsvProcessingResult
{
    public int RecordsProcessed { get; set; }
    public int RecordsInserted { get; set; }
    public List<string> ValidationErrors { get; set; } = new();
    public List<string> InsertErrors { get; set; } = new();
    public bool HasErrors => ValidationErrors.Any() || InsertErrors.Any();
}
