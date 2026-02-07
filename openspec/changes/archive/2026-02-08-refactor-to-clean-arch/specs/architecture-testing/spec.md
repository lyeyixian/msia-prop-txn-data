## Purpose

Update the architecture testing spec to reflect the complete Clean Architecture layer structure and ensure tests cover all layers (Domain, Application, Infrastructure, API).

## MODIFIED Requirements

### Requirement: Architecture tests enforce Clean Architecture rules

The system SHALL include automated architecture tests that validate Clean Architecture dependency rules using ArchUnitNET.

#### Scenario: Domain project has no project references
- **WHEN** running architecture tests
- **THEN** the test SHALL verify that `MsiaPropertyTransaction.Domain` has zero project references
- **AND** the test SHALL pass only if Domain has no outgoing dependencies

#### Scenario: Domain classes don't reference Infrastructure
- **WHEN** running architecture tests
- **THEN** the test SHALL verify that classes in `MsiaPropertyTransaction.Domain` namespace do not depend on classes in `MsiaPropertyTransaction.Data`, `MsiaPropertyTransaction.Infrastructure`, or `Microsoft.EntityFrameworkCore` namespaces
- **AND** the test SHALL fail if any domain class references EF Core or database concerns

#### Scenario: Application depends only on Domain
- **WHEN** running architecture tests
- **THEN** the test SHALL verify that `MsiaPropertyTransaction.Application` depends only on `MsiaPropertyTransaction.Domain`
- **AND** the test SHALL fail if Application references Infrastructure or API projects

#### Scenario: Infrastructure depends only on Application and Domain
- **WHEN** running architecture tests
- **THEN** the test SHALL verify that `MsiaPropertyTransaction.Infrastructure` depends only on `MsiaPropertyTransaction.Application` and `MsiaPropertyTransaction.Domain`
- **AND** the test SHALL fail if Infrastructure references the API project

#### Scenario: API depends only on Application
- **WHEN** running architecture tests
- **THEN** the test SHALL verify that `MsiaPropertyTransaction` (API) depends only on `MsiaPropertyTransaction.Application`
- **AND** the test SHALL verify that `MsiaPropertyTransaction.Domain` does not depend on `MsiaPropertyTransaction`
- **AND** the test SHALL verify that `MsiaPropertyTransaction.Application` does not depend on `MsiaPropertyTransaction`
- **AND** the dependency direction SHALL follow Clean Architecture principles

### Requirement: Architecture tests are integrated into test suite

The system SHALL run architecture tests alongside other tests in the CI/CD pipeline.

#### Scenario: Architecture tests run with dotnet test
- **WHEN** executing `dotnet test` command
- **THEN** architecture tests SHALL execute automatically
- **AND** test results SHALL include architecture rule validation

#### Scenario: Architecture tests have descriptive names
- **WHEN** inspecting architecture test files
- **THEN** test method names SHALL clearly describe the rule being tested
- **AND** test failure messages SHALL explain which architectural rule was violated
