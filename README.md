# msia-prop-txn-data

Malaysian Property Transaction Data System - Extracts data from NAPIC and stores in PostgreSQL database.

## Prerequisites

- .NET 10 SDK
- Docker and Docker Compose
- PostgreSQL (via Docker)

## Quick Start

### 1. Start Infrastructure

```bash
docker-compose up -d
```

This starts:
- PostgreSQL on port 5432
- LocalStack (S3-compatible storage) on port 4566

### 2. Run the Application

```bash
cd src/MsiaPropertyTransaction
dotnet run
```

The API will be available at `https://localhost:5001`

## API Endpoints

### Upload CSV via File Upload (Legacy)

**POST** `/api/upload-csv`

Upload a CSV file directly to the API.

**Request:**
- Method: POST
- Content-Type: multipart/form-data
- Body: Form data with `file` field containing CSV file

**Response:**
```json
{
  "message": "CSV uploaded successfully",
  "recordsInserted": 150,
  "validationErrors": null
}
```

### Process CSV from S3 (New)

**POST** `/api/process-s3-csv`

Process a CSV file stored in an S3-compatible bucket.

**Request:**
- Method: POST
- Content-Type: application/json
- Body:
```json
{
  "bucketName": "property-transactions",
  "fileKey": "data/january-2024.csv"
}
```

**Response (Success):**
```json
{
  "message": "CSV processed successfully from S3",
  "recordsProcessed": 150,
  "recordsInserted": 150
}
```

**Response (With Errors):**
```json
{
  "error": "Processing completed with errors",
  "recordsProcessed": 150,
  "recordsInserted": 145,
  "validationErrors": ["Row 10: Invalid date format"],
  "insertErrors": null
}
```

**Error Responses:**
- `400 Bad Request`: Invalid bucket name or file key format
- `404 Not Found`: File not found in bucket
- `500 Internal Server Error`: S3 connectivity issues or processing errors

## LocalStack Setup (S3 for Local Development)

LocalStack provides an S3-compatible storage service for local development.

### Default Configuration

LocalStack is pre-configured in `appsettings.Development.json`:

```json
{
  "S3Storage": {
    "ServiceUrl": "http://localhost:4566",
    "Region": "us-east-1",
    "AccessKey": "test",
    "SecretKey": "test",
    "BucketName": "property-transactions"
  }
}
```

### Automatic Bucket Creation

The application automatically creates the S3 bucket on startup in development mode.

### Uploading Test Files

Use the AWS CLI to upload test CSV files:

```bash
# Configure AWS CLI for LocalStack
aws configure set aws_access_key_id test
aws configure set aws_secret_access_key test
aws configure set region us-east-1

# Upload a file
aws --endpoint-url=http://localhost:4566 s3 cp test-data/sample.csv s3://property-transactions/

# List bucket contents
aws --endpoint-url=http://localhost:4566 s3 ls s3://property-transactions/
```

## LocalStack Quickstart for New Developers

1. **Start Docker Compose:**
   ```bash
   docker-compose up -d
   ```

2. **Verify LocalStack is running:**
   ```bash
   curl http://localhost:4566/_localstack/health
   ```
   Should return: `{"services": {"s3": "running"}}`

3. **Upload sample CSV:**
   ```bash
   aws --endpoint-url=http://localhost:4566 s3 cp test-data/sample.csv s3://property-transactions/
   ```

4. **Process the file:**
   ```bash
   curl -X POST https://localhost:5001/api/process-s3-csv \
     -H "Content-Type: application/json" \
     -d '{"bucketName":"property-transactions","fileKey":"sample.csv"}' \
     -k
   ```

## Railway Production Deployment

### Environment Variables

Set these environment variables on Railway:

```
# Database
ConnectionStrings__DefaultConnection=<Railway PostgreSQL connection string>

# S3 Storage (Railway Storage)
S3Storage__ServiceUrl=<Railway Storage endpoint>
S3Storage__Region=us-east-1
S3Storage__AccessKey=<Railway Storage access key>
S3Storage__SecretKey=<Railway Storage secret key>
S3Storage__BucketName=<Your bucket name>

# Optional
ASPNETCORE_ENVIRONMENT=Production
```

### Production Configuration

The application reads S3 configuration from environment variables in production.

## Migration Guide: File Upload → S3 Processing

### For API Consumers

**Old approach (file upload):**
```bash
curl -X POST https://api.example.com/api/upload-csv \
  -F "file=@data.csv"
```

**New approach (S3-based):**
1. Upload CSV to S3 bucket
2. Call process endpoint with bucket/file reference:
```bash
curl -X POST https://api.example.com/api/process-s3-csv \
  -H "Content-Type: application/json" \
  -d '{"bucketName":"property-transactions","fileKey":"data.csv"}'
```

### Benefits of S3 Approach

- **Scalability**: No file size limits (was limited by HTTP request size)
- **Reliability**: Files persist for retry if processing fails
- **Performance**: Async processing, no timeouts on large files
- **Separation**: Upload and processing can happen independently

## Troubleshooting

### S3 Connectivity Issues

**Problem:** `Failed to retrieve file from S3` error

**Solutions:**
1. Verify LocalStack is running:
   ```bash
   docker-compose ps
   ```

2. Check S3 configuration in appsettings:
   ```bash
   dotnet run --check-config
   ```

3. Verify bucket exists:
   ```bash
   aws --endpoint-url=http://localhost:4566 s3 ls
   ```

4. Check application logs for detailed error messages

### File Not Found Errors

**Problem:** `File 'x' not found in bucket 'y'`

**Solutions:**
1. Verify file exists:
   ```bash
   aws --endpoint-url=http://localhost:4566 s3 ls s3://bucket-name/
   ```

2. Check file key matches exactly (case-sensitive)
3. Ensure file was uploaded successfully

### Configuration Validation Errors

**Problem:** `S3 Storage configuration error: X is required`

**Solutions:**
1. Check all required environment variables are set
2. Verify appsettings.json has valid S3Storage section
3. For production, ensure Railway Storage credentials are configured

### Docker Issues

**Problem:** LocalStack container won't start

**Solutions:**
1. Check port 4566 is not in use:
   ```bash
   lsof -i :4566
   ```

2. Remove and recreate container:
   ```bash
   docker-compose down
   docker-compose up -d
   ```

3. Check Docker logs:
   ```bash
   docker-compose logs localstack
   ```

## Project Structure

```
.
├── src/
│   ├── MsiaPropertyTransaction/              # Web API project
│   │   ├── Settings/                         # Configuration classes
│   │   ├── Program.cs                        # App entry point
│   │   └── appsettings*.json                 # Configuration files
│   ├── MsiaPropertyTransaction.Application/  # Business logic
│   │   ├── Interfaces/                       # Service interfaces
│   │   ├── Services/                         # Business services
│   │   └── Settings/                         # Settings classes
│   ├── MsiaPropertyTransaction.Domain/       # Domain models
│   └── MsiaPropertyTransaction.Infrastructure/# Data access & external services
│       ├── Services/                         # S3 storage service
│       └── Validation/                       # Configuration validators
├── tests/
│   └── MsiaPropertyTransaction.Tests/        # Unit & integration tests
├── docker-compose.yml                        # Local infrastructure
└── test-data/                                # Sample CSV files
```
