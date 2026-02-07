## Why

The codebase currently lacks clear separation of concerns with business logic mixed across the API project. Following Clean Architecture principles will make the system easier to maintain, test, and extend by establishing clear dependency boundaries between Domain, Application, Infrastructure, and API layers. This refactoring builds on the already-extracted Domain layer to complete the architectural foundation.

## What Changes

- **Create Application layer project** (`MsiaPropertyTransaction.Application`) containing use cases, services, and interfaces
- **Create Infrastructure layer project** (`MsiaPropertyTransaction.Infrastructure`) containing data access, EF Core, and external service implementations
- **Refactor API project** (`MsiaPropertyTransaction`) to become a thin controller layer that depends only on Application layer
- **Move existing services** from API project to Application layer
- **Move data access** from API project to Infrastructure layer
- **Implement architecture tests** using ArchUnitNET to enforce Clean Architecture dependency rules
- **Update project references** to follow dependency inversion: API → Application → Domain, Infrastructure → Application

## Capabilities

### New Capabilities
- `application-layer`: Define and implement the Application layer with use cases, service interfaces, and DTOs
- `infrastructure-layer`: Define and implement the Infrastructure layer with EF Core, repositories, and external services
- `clean-architecture-tests`: Implement architecture tests using ArchUnitNET to enforce dependency rules

### Modified Capabilities
- `architecture-testing`: Update spec to reflect implementation details and ensure tests cover all Clean Architecture layers (Domain, Application, Infrastructure, API)

## Impact

- **Projects**: New Application and Infrastructure projects added; API project slimmed down
- **Dependencies**: Project reference structure changes to enforce Clean Architecture boundaries
- **Code organization**: Services and data access move to appropriate layers
- **Testing**: New architecture test suite added to enforce dependency rules
- **Build**: Solution file updated with new projects
- **CI/CD**: Architecture tests run alongside existing tests
