## Context

The project follows Clean Architecture with 4 layers in src/:
- **MsiaPropertyTransaction** (API/Web layer)
- **MsiaPropertyTransaction.Application** (Application services)
- **MsiaPropertyTransaction.Domain** (Domain entities and logic)
- **MsiaPropertyTransaction.Infrastructure** (Data access, external services)

Currently, all tests reside in a single project `MsiaPropertyTransaction.Tests` which mixes:
- Unit tests (Domain, Application services)
- Integration tests (Database, S3)
- Architecture tests (using ArchUnitNET)
- End-to-end tests

This violates the principle that test structure should mirror source structure for maintainability.

## Goals / Non-Goals

**Goals:**
- Create dedicated test projects for each non-API layer (Domain, Application, Infrastructure)
- Establish clear separation between unit tests and integration tests
- Maintain 100% test coverage during refactoring
- Enable running tests by layer (faster feedback loops)
- Follow .NET testing best practices (xUnit, project naming conventions)

**Non-Goals:**
- No changes to production code logic
- No new test cases (only reorganization)
- No changes to existing integration test behavior
- No addition of new testing frameworks

## Decisions

### Decision 1: Keep Integration Tests in Existing Project
**Rationale**: The existing `MsiaPropertyTransaction.Tests` already uses `Microsoft.AspNetCore.Mvc.Testing` and tests the full stack. Renaming it would require updating CI/CD and documentation. Instead, we'll repurpose it as the integration/E2E test project.

**Alternative considered**: Rename to `MsiaPropertyTransaction.Integration.Tests` - rejected due to breaking changes in build scripts.

### Decision 2: Standard xUnit Project Template
**Rationale**: All new test projects will follow the same pattern as the existing test project:
- Same package versions (xUnit 2.9.2, Microsoft.NET.Test.Sdk 17.12.0, etc.)
- Same target framework (net10.0)
- Same nullable/implicit usings settings
- Moq for mocking

### Decision 3: Project Reference Strategy
Each test project references ONLY its corresponding source project:
- `Domain.Tests` → `Domain`
- `Application.Tests` → `Application` (+ Moq for interface mocking)
- `Infrastructure.Tests` → `Infrastructure` (+ in-memory DB for isolation)

The existing test project already references all 4 source projects, which is correct for integration tests.

### Decision 4: File Organization
Tests will be organized in folders matching the source structure:
```
tests/
├── MsiaPropertyTransaction.Domain.Tests/
│   └── Entities/
│       └── PropertyTransactionTests.cs
├── MsiaPropertyTransaction.Application.Tests/
│   ├── Services/
│   │   └── CsvValidationServiceTests.cs
│   └── Interfaces/
├── MsiaPropertyTransaction.Infrastructure.Tests/
│   ├── Repositories/
│   │   └── PropertyTransactionRepositoryTests.cs
│   └── Services/
│       └── S3StorageServiceTests.cs
└── MsiaPropertyTransaction.Tests/          # Integration tests stay here
    ├── CsvUploadEndToEndTests.cs
    ├── CsvUploadEndpointTests.cs
    └── CustomWebApplicationFactory.cs
```

## Risks / Trade-offs

**Risk**: Moving test files may break git history blame
→ **Mitigation**: Use `git mv` to preserve history during file moves

**Risk**: Test coverage gaps during migration
→ **Mitigation**: Run full test suite before and after; ensure same number of tests pass

**Risk**: CI/CD pipelines reference specific test project paths
→ **Mitigation**: Check for hardcoded paths in `.github/workflows/`, Azure DevOps configs, or scripts

**Risk**: IDE solution explorer becomes cluttered with many test projects
→ **Mitigation**: Use solution folders to group test projects together

**Trade-off**: More projects = slightly slower initial build
→ **Acceptance**: Build time increase is minimal; benefit of clear separation outweighs cost
