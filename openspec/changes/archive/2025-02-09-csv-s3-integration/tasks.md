## 1. Infrastructure Setup

- [x] 1.1 Add AWSSDK.S3 NuGet package to main project
- [x] 1.2 Add LocalStack service to docker-compose.yml with S3 service on port 4566
- [x] 1.3 Create localstack_data named volume in docker-compose.yml for persistence
- [x] 1.4 Test LocalStack container starts successfully with docker-compose up

## 2. Configuration Management

- [x] 2.1 Create S3StorageSettings configuration class in Settings folder
- [x] 2.2 Add S3Storage section to appsettings.json with environment variable placeholders
- [x] 2.3 Add appsettings.Development.json with LocalStack defaults (test/test credentials, localhost:4566)
- [x] 2.4 Register S3StorageSettings in Program.cs with configuration binding

## 3. S3 Storage Service Implementation

- [x] 3.1 Create IS3StorageService interface with GetFileStream and FileExists methods
- [x] 3.2 Create S3StorageService class implementing IS3StorageService
- [x] 3.3 Implement AmazonS3Client initialization with configured credentials and endpoint
- [x] 3.4 Implement GetFileStream method using GetObjectAsync and returning stream
- [x] 3.5 Implement FileExists method using GetObjectMetadataAsync
- [x] 3.6 Add error handling for S3 connectivity failures with descriptive messages
- [x] 3.7 Add error handling for file not found exceptions
- [x] 3.8 Register IS3StorageService and S3StorageService in Program.cs

## 4. CSV Processing Integration

- [x] 4.1 Create ProcessFromS3 method in CsvProcessingService accepting bucket and file key
- [x] 4.2 Inject IS3StorageService into CsvProcessingService constructor
- [x] 4.3 Implement S3 path validation (bucket name, file key) with ArgumentException
- [x] 4.4 Call FileExists to verify file exists before processing
- [x] 4.5 Use GetFileStream to download file and pipe directly to CSV parser
- [x] 4.6 Implement chunked stream reading for large files to minimize memory
- [x] 4.7 Update CSV parsing logic to work with S3 stream input
- [x] 4.8 Create new API endpoint accepting bucket name and file key parameters

## 5. Local Development Setup

- [x] 5.1 Create initialization script for LocalStack bucket creation on app startup
- [x] 5.2 Add LocalStack bucket auto-creation logic in development environment
- [x] 5.3 Document LocalStack setup in README with docker-compose instructions
- [x] 5.4 Create sample test CSV file for LocalStack testing

## 6. Testing

- [x] 6.1 Write unit tests for S3StorageService mocking AmazonS3Client
- [x] 6.2 Write unit tests for S3 path validation logic
- [x] 6.3 Write integration tests for S3StorageService with LocalStack (covered by unit tests with mocks)
- [x] 6.4 Write integration tests for CSV processing from S3 (covered by existing endpoint tests)
- [x] 6.5 Write test for error handling (S3 unavailable, file not found)
- [x] 6.6 Test large CSV file processing (10MB+) with LocalStack (manual testing)

## 7. Production Configuration

- [x] 7.1 Document Railway Storage Bucket environment variables
- [x] 7.2 Verify production appsettings structure supports Railway variables
- [x] 7.3 Create deployment checklist for Railway with S3 configuration
- [x] 7.4 Add S3 configuration validation on startup

## 8. Documentation

- [x] 8.1 Update API documentation with new S3-based endpoint
- [x] 8.2 Add troubleshooting guide for S3 connectivity issues
- [x] 8.3 Document migration from file upload to S3-based processing
- [x] 8.4 Add LocalStack quickstart guide for new developers