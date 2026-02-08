## Why

Passing large CSV files directly to API endpoints is not feasible due to HTTP request size limitations, memory consumption, and timeout issues. Uploading files to S3 and having the API read from the storage bucket provides a more scalable and reliable approach for handling large data imports.

## What Changes

- Replace file upload endpoint with S3-compatible bucket reference input
- Add S3 client service for downloading files from S3-compatible storage
- Integrate LocalStack for local development with local S3 emulation
- Configure Railway Storage Bucket for production deployment
- Update CSV processing logic to read from S3 stream instead of uploaded file

## Capabilities

### New Capabilities
- `s3-storage-integration`: S3-compatible storage client for file retrieval
- `localstack-environment`: LocalStack configuration for local S3 development
- `railway-storage-config`: Railway Storage Bucket configuration for production

### Modified Capabilities
- `csv-processing`: Change input source from file upload to S3 file reference

## Impact

- `CsvProcessingService.cs` - Modify to accept S3 file path instead of IFormFile
- `appsettings.json` - Add S3 configuration section with LocalStack and Railway endpoints
- `Program.cs` - Register S3 client service
- New project dependency on AWS S3 SDK or compatible S3 client library
- Docker Compose - Add LocalStack service for local development
