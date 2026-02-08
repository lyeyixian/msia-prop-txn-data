## ADDED Requirements

### Requirement: S3 Storage Client Interface
The system SHALL provide an interface for S3-compatible storage operations.

#### Scenario: Get file stream from S3 bucket
- **WHEN** the system calls GetFileStream with bucket name and file key
- **THEN** the system SHALL return a Stream containing the file content

#### Scenario: Check file existence
- **WHEN** the system calls FileExists with bucket name and file key
- **THEN** the system SHALL return true if the file exists, false otherwise

### Requirement: S3 Configuration
The system SHALL support environment-specific S3 configuration via appsettings.json.

#### Scenario: Load S3 configuration from environment variables
- **WHEN** the application starts
- **THEN** the system SHALL read S3 configuration from environment variables (S3_SERVICE_URL, AWS_REGION, AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY, S3_BUCKET_NAME)

#### Scenario: S3 client initialization
- **WHEN** the S3StorageService is instantiated
- **THEN** the system SHALL create an AmazonS3Client with the configured credentials and endpoint

### Requirement: S3 Error Handling
The system SHALL handle S3 connectivity errors gracefully.

#### Scenario: S3 endpoint unavailable
- **WHEN** the S3 endpoint is unreachable
- **THEN** the system SHALL throw an exception with a clear error message indicating the S3 connection failed

#### Scenario: File not found in bucket
- **WHEN** GetFileStream is called with a non-existent file key
- **THEN** the system SHALL throw a FileNotFoundException with the file key