## Why

The current project structure has all projects at the repository root level, making it difficult to distinguish between source code projects and test projects. This lack of organization hinders code navigation, reduces clarity for new contributors, and deviates from standard .NET project organization practices. Establishing a clear `src/` and `tests/` folder structure will improve maintainability and follow community conventions.

## What Changes

- **Move** all source projects into a `src/` folder at repository root
  - `MsiaPropertyTransaction/`
  - `MsiaPropertyTransaction.Domain/`
  - `MsiaPropertyTransaction.Application/`
  - `MsiaPropertyTransaction.Infrastructure/`
- **Move** all test projects into a `tests/` folder at repository root
- **Update** solution file (`.sln`) to reflect new project paths
- **Update** all project references (`.csproj`) to use correct relative paths
- **Update** Docker files and docker-compose configuration if they reference project paths
- **Update** any CI/CD configuration files (GitHub Actions, etc.) with new paths
- **Update** `.gitignore` and other configuration files as needed
- **Verify** build still works after reorganization

## Capabilities

### New Capabilities
<!-- No new capabilities - this is a structural reorganization only -->

### Modified Capabilities
<!-- No spec-level requirement changes - implementation/structure only -->

## Impact

- **Solution file** (`.slnx`): Project paths will be updated to `src/` and `tests/` prefixes
- **Project files** (`.csproj`): All `<ProjectReference>` paths need adjustment
- **Docker**: `Dockerfile` and `docker-compose.yml` paths for COPY commands
- **CI/CD**: GitHub Actions workflow files with build/test steps
- **Documentation**: README and any docs referencing project structure
- **IDE**: Solution explorers will show new folder hierarchy
- **Breaking**: Local development environments may need to re-open solution after pull
