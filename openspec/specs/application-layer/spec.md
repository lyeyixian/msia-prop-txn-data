## Purpose

Define requirements for the Application layer project containing use cases, service interfaces, and DTOs following Clean Architecture principles.

## ADDED Requirements

### Requirement: Application layer project exists

The system SHALL have a dedicated Application layer project that contains use cases, service interfaces, and DTOs with no external dependencies except Domain.

#### Scenario: Application project references only Domain
- **WHEN** inspecting the Application project file
- **THEN** it SHALL contain only a ProjectReference to `MsiaPropertyTransaction.Domain`
- **AND** it SHALL contain no PackageReference elements for infrastructure concerns (EF Core, external services)

#### Scenario: Application layer is a class library
- **WHEN** inspecting the Application project file
- **THEN** it SHALL use `<ProjectType>Library</ProjectType>` or default to class library
- **AND** it SHALL not be an executable project

### Requirement: Service interfaces are defined in Application layer

The system SHALL define all service interfaces in the Application layer that the API layer depends upon.

#### Scenario: Property transaction service interface exists
- **WHEN** inspecting the Application layer
- **THEN** there SHALL be an interface `IPropertyTransactionService`
- **AND** it SHALL define the contract for property transaction operations
- **AND** it SHALL use domain entities as parameters and return types

#### Scenario: CSV validation service interface exists
- **WHEN** inspecting the Application layer
- **THEN** there SHALL be an interface `ICsvValidationService`
- **AND** it SHALL define the contract for CSV validation operations
- **AND** it SHALL use domain entities as parameters and return types

#### Scenario: CSV parsing service interface exists
- **WHEN** inspecting the Application layer
- **THEN** there SHALL be an interface `ICsvParsingService`
- **AND** it SHALL define the contract for CSV parsing operations
- **AND** it SHALL use domain entities as parameters and return types

### Requirement: DTOs are defined in Application layer

The system SHALL define Data Transfer Objects in the Application layer for cross-layer communication.

#### Scenario: DTOs use correct namespace
- **WHEN** inspecting DTO files
- **THEN** they SHALL be in namespace `MsiaPropertyTransaction.Application.DTOs`
- **AND** the folder structure SHALL match the namespace hierarchy

### Requirement: Application layer has no infrastructure dependencies

The system SHALL ensure the Application layer has no direct dependencies on infrastructure concerns.

#### Scenario: No EF Core references in Application
- **WHEN** inspecting Application layer code
- **THEN** there SHALL be no using statements for `Microsoft.EntityFrameworkCore`
- **AND** there SHALL be no using statements for `MsiaPropertyTransaction.Data`
- **AND** there SHALL be no database-specific types

#### Scenario: No HTTP context dependencies in Application
- **WHEN** inspecting Application layer code
- **THEN** there SHALL be no using statements for `Microsoft.AspNetCore.Http`
- **AND** there SHALL be no dependencies on web framework types
