## 1. Create Domain Project Structure

- [ ] 1.1 Create new `MsiaPropertyTransaction.Domain` project folder
- [ ] 1.2 Create `MsiaPropertyTransaction.Domain.csproj` with zero package references
- [ ] 1.3 Create `Entities/` folder within Domain project
- [ ] 1.4 Add Domain project to solution file (`MsiaPropertyTransaction.slnx`)

## 2. Move Domain Entities

- [ ] 2.1 Move `PropertyTransaction.cs` from `MsiaPropertyTransaction/Models/` to `MsiaPropertyTransaction.Domain/Entities/`
- [ ] 2.2 Update namespace in `PropertyTransaction.cs` to `MsiaPropertyTransaction.Domain.Entities`
- [ ] 2.3 Move `CsvPropertyTransaction.cs` from `MsiaPropertyTransaction/Models/` to `MsiaPropertyTransaction.Domain/Entities/`
- [ ] 2.4 Update namespace in `CsvPropertyTransaction.cs` to `MsiaPropertyTransaction.Domain.Entities`
- [ ] 2.5 Move `InsertResult` class from `PropertyTransactionService.cs` to new file `InsertResult.cs` in Domain project
- [ ] 2.6 Update namespace in `InsertResult.cs` to `MsiaPropertyTransaction.Domain.Entities`

## 3. Update API Project References

- [ ] 3.1 Add project reference to Domain project in `MsiaPropertyTransaction.csproj`
- [ ] 3.2 Update `using MsiaPropertyTransaction.Models;` to `using MsiaPropertyTransaction.Domain.Entities;` in `Program.cs`
- [ ] 3.3 Update `using MsiaPropertyTransaction.Models;` to `using MsiaPropertyTransaction.Domain.Entities;` in `AppDbContext.cs`
- [ ] 3.4 Update `using MsiaPropertyTransaction.Models;` to `using MsiaPropertyTransaction.Domain.Entities;` in all Service files
- [ ] 3.5 Remove old `Models/` folder if empty after move

## 4. Update Test Project References

- [ ] 4.1 Add project reference to Domain project in `MsiaPropertyTransaction.Tests.csproj`
- [ ] 4.2 Update `using MsiaPropertyTransaction.Models;` to `using MsiaPropertyTransaction.Domain.Entities;` in `PropertyTransactionModelTests.cs`
- [ ] 4.3 Update `using MsiaPropertyTransaction.Models;` to `using MsiaPropertyTransaction.Domain.Entities;` in `PropertyTransactionServiceTests.cs`
- [ ] 4.4 Update `using MsiaPropertyTransaction.Models;` to `using MsiaPropertyTransaction.Domain.Entities;` in `CsvParsingServiceTests.cs`
- [ ] 4.5 Update `using MsiaPropertyTransaction.Models;` to `using MsiaPropertyTransaction.Domain.Entities;` in `CsvValidationServiceTests.cs`
- [ ] 4.6 Update `using MsiaPropertyTransaction.Models;` to `using MsiaPropertyTransaction.Domain.Entities;` in `CsvUploadEndpointTests.cs`
- [ ] 4.7 Update `using MsiaPropertyTransaction.Models;` to `using MsiaPropertyTransaction.Domain.Entities;` in `CsvUploadEndToEndTests.cs`

## 5. Add ArchUnitNET Architecture Tests

- [ ] 5.1 Add ArchUnitNET and ArchUnitNET.xUnit NuGet packages to test project
- [ ] 5.2 Create `ArchitectureTests.cs` file in test project
- [ ] 5.3 Write test to verify Domain project has zero project references
- [ ] 5.4 Write test to verify Domain classes don't reference EF Core or Infrastructure namespaces
- [ ] 5.5 Write test to verify dependency direction (API → Domain, not reverse)
- [ ] 5.6 Run architecture tests to ensure they pass with the new structure

## 6. Verify Implementation

- [ ] 6.1 Build solution successfully with `dotnet build`
- [ ] 6.2 Run all unit tests: `dotnet test` (including architecture tests)
- [ ] 6.3 Verify Domain project has zero package references by inspecting `.csproj` file
- [ ] 6.4 Verify all entities are accessible from API project
- [ ] 6.5 Verify architecture tests enforce Clean Architecture rules
- [ ] 6.6 Clean build artifacts and rebuild to ensure no cached issues

## 7. Final Review

- [ ] 7.1 Review folder structure matches Clean Architecture pattern
- [ ] 7.2 Ensure no leftover files in old `Models/` location
- [ ] 7.3 Verify all imports are consistent across codebase
- [ ] 7.4 Confirm solution builds without warnings
- [ ] 7.5 Review architecture tests provide clear failure messages
- [ ] 7.6 Document architecture testing approach for team
