## ADDED Requirements

### Requirement: Domain unit tests are moved to Domain test project
Domain layer unit tests SHALL be moved from the integration test project to the new Domain test project.

#### Scenario: Property transaction model tests
- **WHEN** reviewing the test migration
- **THEN** `PropertyTransactionModelTests.cs` is located in `tests/MsiaPropertyTransaction.Domain.Tests/Entities/`
- **AND** the file is removed from `tests/MsiaPropertyTransaction.Tests/`

#### Scenario: Test namespaces updated
- **WHEN** domain tests are moved
- **THEN** namespaces are updated from `MsiaPropertyTransaction.Tests` to `MsiaPropertyTransaction.Domain.Tests`
- **AND` using statements are updated if needed

### Requirement: Application unit tests are moved to Application test project
Application layer unit tests SHALL be moved from the integration test project to the new Application test project.

#### Scenario: CSV validation service tests
- **WHEN** reviewing the test migration
- **THEN** `CsvValidationServiceTests.cs` is located in `tests/MsiaPropertyTransaction.Application.Tests/Services/`
- **AND** the file is removed from `tests/MsiaPropertyTransaction.Tests/`

#### Scenario: CSV parsing service tests
- **WHEN** reviewing the test migration
- **THEN** `CsvParsingServiceTests.cs` is located in `tests/MsiaPropertyTransaction.Application.Tests/Services/`
- **AND** the file is removed from `tests/MsiaPropertyTransaction.Tests/`

#### Scenario: Property transaction service tests
- **WHEN** reviewing the test migration
- **THEN** `PropertyTransactionServiceTests.cs` is located in `tests/MsiaPropertyTransaction.Application.Tests/Services/`
- **AND** the file is removed from `tests/MsiaPropertyTransaction.Tests/`

### Requirement: Infrastructure unit tests are moved to Infrastructure test project
Infrastructure layer unit tests SHALL be moved from the integration test project to the new Infrastructure test project.

#### Scenario: S3 storage service tests
- **WHEN** reviewing the test migration
- **THEN** `S3StorageServiceTests.cs` is located in `tests/MsiaPropertyTransaction.Infrastructure.Tests/Services/`
- **AND** the file is removed from `tests/MsiaPropertyTransaction.Tests/`

#### Scenario: S3 configuration validator tests
- **WHEN** reviewing the test migration
- **THEN** `S3ConfigurationValidatorTests.cs` is located in `tests/MsiaPropertyTransaction.Infrastructure.Tests/Validation/`
- **AND** the file is removed from `tests/MsiaPropertyTransaction.Tests/`

#### Scenario: S3 path validation tests
- **WHEN** reviewing the test migration
- **THEN** `S3PathValidationTests.cs` is located in `tests/MsiaPropertyTransaction.Infrastructure.Tests/Validation/`
- **AND** the file is removed from `tests/MsiaPropertyTransaction.Tests/`

### Requirement: Integration tests remain in existing project
Integration and end-to-end tests SHALL remain in the existing test project.

#### Scenario: Integration test preservation
- **WHEN** the test migration is complete
- **THEN** `tests/MsiaPropertyTransaction.Tests/` still contains:
  - `CsvUploadEndToEndTests.cs`
  - `CsvUploadEndpointTests.cs`
  - `CustomWebApplicationFactory.cs`
  - `ArchitectureTests.cs`

#### Scenario: Architecture tests remain
- **WHEN** the test migration is complete
- **THEN** `ArchitectureTests.cs` remains in `tests/MsiaPropertyTransaction.Tests/`
- **AND** it continues to enforce architecture rules across all layers

### Requirement: All tests pass after migration
All tests SHALL pass after the migration is complete.

#### Scenario: Pre-migration test count
- **WHEN** tests are counted before migration
- **THEN** the total number is recorded

#### Scenario: Post-migration test count
- **WHEN** tests are counted after migration
- **THEN** the total number equals the pre-migration count
- **AND** all tests pass successfully
