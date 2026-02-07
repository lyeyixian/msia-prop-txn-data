## 1. Create Domain Project Structure

- [x] 1.1 Create new `MsiaPropertyTransaction.Domain` project folder
- [x] 1.2 Create `MsiaPropertyTransaction.Domain.csproj` with zero package references
- [x] 1.3 Create `Entities/` folder within Domain project
- [x] 1.4 Add Domain project to solution file (`MsiaPropertyTransaction.slnx`)

## 2. Move Domain Entities

- [x] 2.1 Move `PropertyTransaction.cs` from `MsiaPropertyTransaction/Models/` to `MsiaPropertyTransaction.Domain/Entities/`
- [x] 2.2 Update namespace in `PropertyTransaction.cs` to `MsiaPropertyTransaction.Domain.Entities`
- [x] 2.3 Move `CsvPropertyTransaction.cs` from `MsiaPropertyTransaction/Models/` to `MsiaPropertyTransaction.Domain/Entities/`
- [x] 2.4 Update namespace in `CsvPropertyTransaction.cs` to `MsiaPropertyTransaction.Domain.Entities`
- [x] 2.5 Move `InsertResult` class from `PropertyTransactionService.cs` to new file `InsertResult.cs` in Domain project
- [x] 2.6 Update namespace in `InsertResult.cs` to `MsiaPropertyTransaction.Domain.Entities`

## 3. Update API Project References

- [x] 3.1 Add project reference to Domain project in `MsiaPropertyTransaction.csproj`
- [x] 3.2 Update `using MsiaPropertyTransaction.Models;` to `using MsiaPropertyTransaction.Domain.Entities;` in `Program.cs`
- [x] 3.3 Update `using MsiaPropertyTransaction.Models;` to `using MsiaPropertyTransaction.Domain.Entities;` in `AppDbContext.cs`
- [x] 3.4 Update `using MsiaPropertyTransaction.Models;` to `using MsiaPropertyTransaction.Domain.Entities;` in all Service files
- [x] 3.5 Remove old `Models/` folder if empty after move

## 4. Update Test Project References

- [x] 4.1 Add project reference to Domain project in `MsiaPropertyTransaction.Tests.csproj`
- [x] 4.2 Update `using MsiaPropertyTransaction.Models;` to `using MsiaPropertyTransaction.Domain.Entities;` in `PropertyTransactionModelTests.cs`
- [x] 4.3 Update `using MsiaPropertyTransaction.Models;` to `using MsiaPropertyTransaction.Domain.Entities;` in `PropertyTransactionServiceTests.cs`
- [x] 4.4 Update `using MsiaPropertyTransaction.Models;` to `using MsiaPropertyTransaction.Domain.Entities;` in `CsvParsingServiceTests.cs`
- [x] 4.5 Update `using MsiaPropertyTransaction.Models;` to `using MsiaPropertyTransaction.Domain.Entities;` in `CsvValidationServiceTests.cs`
- [x] 4.6 Update `using MsiaPropertyTransaction.Models;` to `using MsiaPropertyTransaction.Domain.Entities;` in `CsvUploadEndpointTests.cs`
- [x] 4.7 Update `using MsiaPropertyTransaction.Models;` to `using MsiaPropertyTransaction.Domain.Entities;` in `CsvUploadEndToEndTests.cs`

## 5. Add ArchUnitNET Architecture Tests

- [x] 5.1 Add ArchUnitNET and ArchUnitNET.xUnit NuGet packages to test project
- [x] 5.2 Create `ArchitectureTests.cs` file in test project
- [x] 5.3 Write test to verify Domain project has zero project references
- [x] 5.4 Write test to verify Domain classes don't reference EF Core or Infrastructure namespaces
- [x] 5.5 Write test to verify dependency direction (API → Domain, not reverse)
- [x] 5.6 Run architecture tests to ensure they pass with the new structure

## 6. Verify Implementation

- [x] 6.1 Build solution successfully with `dotnet build`
- [x] 6.2 Run all unit tests: `dotnet test` (including architecture tests)
- [x] 6.3 Verify Domain project has zero package references by inspecting `.csproj` file
- [x] 6.4 Verify all entities are accessible from API project
- [x] 6.5 Verify architecture tests enforce Clean Architecture rules
- [x] 6.6 Clean build artifacts and rebuild to ensure no cached issues

## 7. Final Review

- [x] 7.1 Review folder structure matches Clean Architecture pattern
- [x] 7.2 Ensure no leftover files in old `Models/` location
- [x] 7.3 Verify all imports are consistent across codebase
- [x] 7.4 Confirm solution builds without warnings
- [x] 7.5 Review architecture tests provide clear failure messages
- [x] 7.6 Document architecture testing approach for team
