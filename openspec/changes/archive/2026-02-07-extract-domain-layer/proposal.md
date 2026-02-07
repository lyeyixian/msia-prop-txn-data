## Why

The current codebase has all code in a single project, mixing domain entities with infrastructure concerns (EF Core DbContext) and application logic (services). This violates separation of concerns and makes the codebase harder to maintain and test. By extracting a Domain layer, we establish Clean Architecture principles where the core business logic has zero dependencies and can be tested in isolation.

## What Changes

- Create a new `MsiaPropertyTransaction.Domain` project
- Move domain entities to the Domain project:
  - `PropertyTransaction` - Core domain entity
  - `CsvPropertyTransaction` - CSV import DTO
  - `InsertResult` - Result object from batch operations
- Update namespace references in all consuming projects (API, Tests)
- The Domain project will have **no external dependencies** (no EF Core, no ASP.NET)
- Add ArchUnitNET architecture tests to enforce Clean Architecture rules:
  - Domain layer has no dependencies on other projects
  - Proper dependency direction (API → Domain, not reverse)

## Capabilities

### New Capabilities
- `domain-layer-extraction`: Extract core domain entities into a separate, dependency-free project following Clean Architecture principles
- `architecture-testing`: Add ArchUnitNET tests to enforce Clean Architecture dependency rules and validate layer boundaries

### Modified Capabilities
<!-- No existing capabilities are changing at the spec level - only implementation refactoring -->

## Impact

- **New Project**: `MsiaPropertyTransaction.Domain/` with `Entities/` folder
- **Modified Files**:
  - `MsiaPropertyTransaction/Models/PropertyTransaction.cs` → move to Domain
  - `MsiaPropertyTransaction/Models/CsvPropertyTransaction.cs` → move to Domain
  - `MsiaPropertyTransaction/Services/PropertyTransactionService.cs` → update namespace refs
  - `MsiaPropertyTransaction/Data/AppDbContext.cs` → update namespace refs
  - `MsiaPropertyTransaction/Program.cs` → update namespace refs
  - `MsiaPropertyTransaction.Tests/` → update all test files with new namespace refs
- **Dependencies**: 
  - Domain project has zero package references
  - ArchUnitNET and ArchUnitNET.xUnit packages for architecture testing
  - Test project will reference ArchUnitNET to enforce architecture rules
- **Breaking Changes**: Namespace changes from `MsiaPropertyTransaction.Models` to `MsiaPropertyTransaction.Domain.Entities` (internal refactor)
