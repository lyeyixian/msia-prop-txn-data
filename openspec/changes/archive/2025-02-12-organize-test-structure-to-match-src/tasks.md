## 1. Create Domain Test Project

- [x] 1.1 Create directory `tests/MsiaPropertyTransaction.Domain.Tests/`
- [x] 1.2 Create `MsiaPropertyTransaction.Domain.Tests.csproj` with xUnit, Moq packages
- [x] 1.3 Add project reference to `MsiaPropertyTransaction.Domain`
- [x] 1.4 Create `Entities/` directory for domain entity tests

## 2. Create Application Test Project

- [x] 2.1 Create directory `tests/MsiaPropertyTransaction.Application.Tests/`
- [x] 2.2 Create `MsiaPropertyTransaction.Application.Tests.csproj` with xUnit, Moq packages
- [x] 2.3 Add project reference to `MsiaPropertyTransaction.Application`
- [x] 2.4 Create `Services/` directory for application service tests

## 3. Create Infrastructure Test Project

- [x] 3.1 Create directory `tests/MsiaPropertyTransaction.Infrastructure.Tests/`
- [x] 3.2 Create `MsiaPropertyTransaction.Infrastructure.Tests.csproj` with xUnit, Moq, EF Core In-Memory packages
- [x] 3.3 Add project reference to `MsiaPropertyTransaction.Infrastructure`
- [x] 3.4 Create `Services/` directory for infrastructure service tests
- [x] 3.5 Create `Repositories/` directory for repository tests
- [x] 3.6 Create `Validation/` directory for validator tests

## 4. Update Solution File

- [x] 4.1 Add `MsiaPropertyTransaction.Domain.Tests` project to solution
- [x] 4.2 Add `MsiaPropertyTransaction.Application.Tests` project to solution
- [x] 4.3 Add `MsiaPropertyTransaction.Infrastructure.Tests` project to solution
- [x] 4.4 Verify solution builds successfully with `dotnet build`

## 5. Move Domain Layer Tests

- [x] 5.1 Record pre-migration test count by running `dotnet test` (78 tests)
- [x] 5.2 Move `PropertyTransactionModelTests.cs` from `tests/MsiaPropertyTransaction.Tests/` to `tests/MsiaPropertyTransaction.Domain.Tests/Entities/` (moved to Infrastructure instead due to DbContext dependency)
- [x] 5.3 Update namespace from `MsiaPropertyTransaction.Tests` to `MsiaPropertyTransaction.Domain.Tests` (updated to Infrastructure.Tests)
- [x] 5.4 Verify domain tests pass in new location

## 6. Move Application Layer Tests

- [x] 6.1 Move `CsvValidationServiceTests.cs` from `tests/MsiaPropertyTransaction.Tests/` to `tests/MsiaPropertyTransaction.Application.Tests/Services/`
- [x] 6.2 Move `CsvParsingServiceTests.cs` from `tests/MsiaPropertyTransaction.Tests/` to `tests/MsiaPropertyTransaction.Application.Tests/Services/`
- [x] 6.3 Move `PropertyTransactionServiceTests.cs` from `tests/MsiaPropertyTransaction.Tests/` to `tests/MsiaPropertyTransaction.Application.Tests/Services/` (moved to Infrastructure.Tests instead due to DbContext dependency)
- [x] 6.4 Update namespaces from `MsiaPropertyTransaction.Tests` to `MsiaPropertyTransaction.Application.Tests`
- [x] 6.5 Add using statements for Moq if needed
- [x] 6.6 Verify application tests pass in new location

## 7. Move Infrastructure Layer Tests

- [x] 7.1 Move `S3StorageServiceTests.cs` from `tests/MsiaPropertyTransaction.Tests/` to `tests/MsiaPropertyTransaction.Infrastructure.Tests/Services/`
- [x] 7.2 Move `S3ConfigurationValidatorTests.cs` from `tests/MsiaPropertyTransaction.Tests/` to `tests/MsiaPropertyTransaction.Infrastructure.Tests/Validation/`
- [x] 7.3 Move `S3PathValidationTests.cs` from `tests/MsiaPropertyTransaction.Tests/` to `tests/MsiaPropertyTransaction.Infrastructure.Tests/Validation/`
- [x] 7.4 Update namespaces from `MsiaPropertyTransaction.Tests` to `MsiaPropertyTransaction.Infrastructure.Tests`
- [x] 7.5 Verify infrastructure tests pass in new location

## 8. Verify Integration Tests Remain

- [x] 8.1 Confirm `CsvUploadEndToEndTests.cs` still exists in `tests/MsiaPropertyTransaction.Tests/`
- [x] 8.2 Confirm `CsvUploadEndpointTests.cs` still exists in `tests/MsiaPropertyTransaction.Tests/`
- [x] 8.3 Confirm `CustomWebApplicationFactory.cs` still exists in `tests/MsiaPropertyTransaction.Tests/`
- [x] 8.4 Confirm `ArchitectureTests.cs` still exists in `tests/MsiaPropertyTransaction.Tests/`
- [x] 8.5 Verify integration tests still pass

## 9. Final Verification

- [x] 9.1 Run `dotnet test` at solution level and record post-migration test count
- [x] 9.2 Verify post-migration test count equals pre-migration count (78 = 78) ✓
- [x] 9.3 Verify all tests pass (100% success rate)
- [x] 9.4 Run `dotnet build` to ensure clean build
- [x] 9.5 Check that no test files remain in old locations using `git status`
