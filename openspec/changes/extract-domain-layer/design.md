## Context

The codebase currently has a monolithic structure with domain entities (`PropertyTransaction`, `CsvPropertyTransaction`) mixed with infrastructure concerns (`AppDbContext`) and application logic (services). This makes it difficult to:

1. Unit test domain logic without database dependencies
2. Maintain clear boundaries between business rules and technical implementation
3. Evolve the architecture incrementally toward Clean Architecture

Clean Architecture establishes that the Domain layer is at the center with no dependencies on other layers. This enables independent testing and clear separation of concerns.

## Goals / Non-Goals

**Goals:**
- Extract domain entities into a separate, dependency-free Domain project
- Maintain all existing functionality (no behavioral changes)
- Set foundation for future Clean Architecture refactoring (Application layer next)
- Ensure all tests pass after namespace updates
- Add ArchUnitNET architecture tests to enforce Clean Architecture rules
- Prevent accidental dependency violations through automated testing

**Non-Goals:**
- Changing entity behavior or properties
- Adding new features
- Refactoring services or infrastructure
- Creating repository pattern (that comes later)

## Decisions

### Decision 1: Domain Project Structure

**Approach:** Create `MsiaPropertyTransaction.Domain` project with `Entities/` folder

**Rationale:** 
- Keeps it simple and clear—one project for the Domain layer
- `Entities` subfolder follows Clean Architecture conventions
- Future value objects and domain events can go in separate folders

**Alternatives considered:**
- Multiple projects (Domain.Core, Domain.Entities) - too granular for current scope
- Keep in same project but separate folder - doesn't achieve zero dependencies

### Decision 2: Namespace Convention

**Approach:** `MsiaPropertyTransaction.Domain.Entities`

**Rationale:**
- Follows .NET naming conventions
- Clear hierarchical structure
- Easy to understand where classes belong

### Decision 3: Entity Placement

**Approach:** Move only `PropertyTransaction`, `CsvPropertyTransaction`, and `InsertResult`

**Rationale:**
- `PropertyTransaction` is the core domain entity with business meaning
- `CsvPropertyTransaction` is a domain DTO for the CSV import use case
- `InsertResult` is a domain result object representing batch operation outcomes
- `AppDbContext` stays in Infrastructure (has EF Core dependency)
- Services stay in Application layer (for now)

### Decision 4: Architecture Testing Strategy

**Approach:** Use ArchUnitNET to enforce Clean Architecture dependency rules

**Rationale:**
- ArchUnitNET is the standard .NET port of ArchUnit for Java
- Provides fluent API for writing architecture rules as tests
- Integrates with xUnit test framework
- Catches architectural violations at build time before they become problems

**Rules to enforce:**
- Domain project has no project references (zero dependencies)
- Domain layer classes don't reference Infrastructure or Application layers
- Dependency direction: API → Application → Domain (Domain has no outgoing dependencies)

**Alternatives considered:**
- Custom Roslyn analyzers - more complex, requires additional tooling
- Manual code review - not scalable, easy to miss violations
- No enforcement - architecture will drift over time

## Risks / Trade-offs

**[Risk] Namespace changes may break existing code references**
→ **Mitigation:** Comprehensive search and replace across all files; verify tests pass

**[Risk] EF Core configuration in OnModelCreating uses entity properties**
→ **Mitigation:** EF Core config will reference the new namespace—no code changes needed beyond using statements

**[Risk] Circular dependencies if Domain references other projects**
→ **Mitigation:** Enforce zero package references in Domain project; build will fail if violated

**[Trade-off] Slightly more complex build (multiple projects)**
→ **Acceptance:** Standard for Clean Architecture; solution file handles it automatically

## Migration Plan

1. Create new Domain project
2. Move entities with updated namespaces
3. Update project references in main API project
4. Update all using statements across codebase
5. Run tests to verify nothing broke
6. Commit the change

Rollback: Revert the commit or manually restore files from git
