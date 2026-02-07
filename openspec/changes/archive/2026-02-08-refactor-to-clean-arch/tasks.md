## 1. Create Application Layer Project

- [x] 1.1 Create `MsiaPropertyTransaction.Application` class library project
- [x] 1.2 Add project reference to `MsiaPropertyTransaction.Domain`
- [x] 1.3 Create folder structure: `Interfaces/`, `Services/`, `DTOs/`
- [x] 1.4 Define `IPropertyTransactionService` interface
- [x] 1.5 Define `ICsvValidationService` interface
- [x] 1.6 Define `IPropertyTransactionRepository` interface
- [x] 1.7 Define `ICsvParsingService` interface

## 2. Create Infrastructure Layer Project

- [x] 2.1 Create `MsiaPropertyTransaction.Infrastructure` class library project
- [x] 2.2 Add project reference to `MsiaPropertyTransaction.Application`
- [x] 2.3 Add project reference to `MsiaPropertyTransaction.Domain`
- [x] 2.4 Add EF Core package references to Infrastructure
- [x] 2.5 Create folder structure: `Data/`, `Repositories/`

## 3. Move Data Access to Infrastructure

- [x] 3.1 Move `AppDbContext` from API to Infrastructure/Data
- [x] 3.2 Update `AppDbContext` namespace to `MsiaPropertyTransaction.Infrastructure.Data`
- [x] 3.3 Create `PropertyTransactionRepository` implementing `IPropertyTransactionRepository`
- [x] 3.4 Remove data access code from API project

## 4. Move Services to Application Layer

- [x] 4.1 Move `PropertyTransactionService` to Application/Services
- [x] 4.2 Move `CsvValidationService` to Application/Services
- [x] 4.3 Update namespaces to `MsiaPropertyTransaction.Application.Services`
- [x] 4.4 Update services to use repository interfaces instead of direct DbContext
- [x] 4.5 Remove service implementations from API project
- [x] 4.6 Make `CsvParsingService` implement `ICsvParsingService` interface
- [x] 4.7 Make `CsvValidationService` implement `ICsvValidationService` interface
- [x] 4.8 Update Program.cs to register and use interfaces instead of concrete types

## 5. Update API Project

- [x] 5.1 Add project reference from API to Application
- [x] 5.2 Remove direct reference to Infrastructure (keep only via DI)
- [x] 5.3 Update controllers to depend on service interfaces
- [x] 5.4 Register Infrastructure services in Program.cs
- [x] 5.5 Update using statements in API project
- [x] 5.6 Verify API endpoints still work correctly

## 6. Update Test Project

- [x] 6.1 Add project reference to Application in test project
- [x] 6.2 Update using statements in test files
- [x] 6.3 Verify all existing tests pass
- [x] 6.4 Fix any broken test references

## 7. Add Architecture Tests

- [x] 7.1 Add ArchUnitNET NuGet package to test project
- [x] 7.2 Create `ArchitectureTests.cs` test class
- [x] 7.3 Write test: Domain has no project references
- [x] 7.4 Write test: Application depends only on Domain
- [x] 7.5 Write test: Infrastructure depends only on Application and Domain
- [x] 7.6 Write test: API depends only on Application
- [x] 7.7 Write test: Domain classes don't reference Infrastructure
- [x] 7.8 Write test: Application classes don't reference EF Core
- [x] 7.9 Run all architecture tests and verify they pass

## 8. Final Verification

- [x] 8.1 Run all unit tests and verify they pass
- [x] 8.2 Run all integration tests and verify they pass
- [x] 8.3 Verify application builds successfully
- [x] 8.4 Test API endpoints manually
- [x] 8.5 Update solution file with new projects
- [x] 8.6 Verify Clean Architecture dependency rules are enforced
