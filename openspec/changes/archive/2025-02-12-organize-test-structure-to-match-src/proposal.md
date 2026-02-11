## Why

The current test project structure doesn't mirror the source code organization. We have 4 projects in src/ (MsiaPropertyTransaction, MsiaPropertyTransaction.Application, MsiaPropertyTransaction.Domain, MsiaPropertyTransaction.Infrastructure) but only 1 test project that mixes integration tests, unit tests, and architecture tests together. This makes it hard to maintain clear separation of concerns and run targeted test suites.

## What Changes

- **Create 3 new test projects** to mirror each non-API project in src/:
  - `MsiaPropertyTransaction.Domain.Tests` - unit tests for domain entities and logic
  - `MsiaPropertyTransaction.Application.Tests` - unit tests for services and application logic
  - `MsiaPropertyTransaction.Infrastructure.Tests` - unit tests for repositories, S3 storage, database operations
- **Refactor existing test project** `MsiaPropertyTransaction.Tests` to focus on integration and end-to-end tests
- **BREAKING**: Move existing unit tests from MsiaPropertyTransaction.Tests to appropriate new test projects
- **Update solution file** to include new test projects
- **Update project references** to follow proper dependency flow

## Capabilities

### New Capabilities
- `test-project-domain`: Unit tests for Domain layer entities and value objects
- `test-project-application`: Unit tests for Application layer services, interfaces, and business logic
- `test-project-infrastructure`: Unit tests for Infrastructure layer repositories, storage services, and external integrations
- `test-structure-refactoring`: Reorganize existing tests into appropriate projects while maintaining test coverage

### Modified Capabilities
<!-- No existing capability requirements are changing, only implementation structure -->

## Impact

- **Solution file**: Add 3 new test project references
- **Project references**: New test projects will reference their corresponding src/ projects
- **Existing tests**: Some test files will move from `tests/MsiaPropertyTransaction.Tests/` to new project directories
- **Build configuration**: No changes needed, follows existing .NET test project patterns
- **CI/CD**: May need updates if test execution commands are project-specific
- **Developer workflow**: Tests can now be run per-layer (e.g., `dotnet test tests/MsiaPropertyTransaction.Domain.Tests`)
