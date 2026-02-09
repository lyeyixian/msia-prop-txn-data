## Context

The Malaysian Property Transaction Data System is organized as a standard .NET solution using Clean Architecture. Currently, all project folders are located at the repository root, which doesn't follow the conventional `src/` and `tests/` folder structure commonly used in the .NET ecosystem. This reorganization aims to align the project structure with community standards without changing any functional behavior.

Current structure:
```
/
├── MsiaPropertyTransaction/
├── MsiaPropertyTransaction.Domain/
├── MsiaPropertyTransaction.Application/
├── MsiaPropertyTransaction.Infrastructure/
├── docker-compose.yml
├── Dockerfile
└── MsiaPropertyTransaction.slnx
```

Target structure:
```
/
├── src/
│   ├── MsiaPropertyTransaction/
│   ├── MsiaPropertyTransaction.Domain/
│   ├── MsiaPropertyTransaction.Application/
│   └── MsiaPropertyTransaction.Infrastructure/
├── docker-compose.yml
├── Dockerfile
└── MsiaPropertyTransaction.slnx
```

## Goals / Non-Goals

**Goals:**
- Move all source projects into a `src/` folder
- Update all project references to maintain build integrity
- Ensure Docker and CI/CD configurations continue to work
- Maintain full backward compatibility (no functional changes)

**Non-Goals:**
- No code changes (only file moves and path updates)
- No changes to project names or namespaces
- No changes to Docker images or deployment processes
- No changes to database schemas or data
- No changes to API contracts or behavior

## Decisions

### Decision 1: Use simple folder moves with git tracking
**Choice:** Move folders using `git mv` to preserve history
**Rationale:** Git will track the moves as renames, preserving file history and making code reviews easier

### Decision 2: Update solution file paths
**Choice:** Manually edit `.slnx` file to update project paths from `ProjectName\` to `src\ProjectName\`
**Rationale:** Solution files use relative paths; updating them ensures Visual Studio/VS Code can load projects correctly
**Alternative considered:** Regenerating solution file - rejected because it would lose project GUIDs and configurations

### Decision 3: Update project references iteratively
**Choice:** Update each `.csproj` file's `<ProjectReference>` paths to account for new relative locations
**Rationale:** References use relative paths; moving projects changes the relative path depth
**Example:** `..\Domain\Domain.csproj` becomes `..\..\src\Domain\Domain.csproj` for projects in `tests/`

### Decision 4: Docker context root remains unchanged
**Choice:** Keep Dockerfile at root; update COPY paths from `ProjectName/` to `src/ProjectName/`
**Rationale:** Docker build context remains the repository root; only the paths within the context change

## Risks / Trade-offs

**[Risk]** Build breaks after path changes → **Mitigation:** Run `dotnet build` after each step, commit working state frequently
**[Risk]** CI/CD pipelines fail → **Mitigation:** Update GitHub Actions workflow files before merging; test on feature branch
**[Risk]** IDE confusion for existing developers → **Mitigation:** Communicate change clearly; developers need to close and reopen solution
**[Risk]** Merge conflicts with in-flight work → **Mitigation:** Coordinate with team; choose low-activity period; this change should be merged quickly

## Migration Plan

1. **Preparation**
   - Ensure all work is committed
   - Create feature branch
   - Notify team of impending structural change

2. **Move Projects**
   - Create `src/` directory
   - Use `git mv` to move each project folder to `src/` location
   - Commit: "Move projects to src/ folder"

3. **Update Solution File**
   - Edit `.slnx` file to update all project paths
   - Verify solution loads in IDE
   - Commit: "Update solution file with new project paths"

4. **Update Project References**
   - For each `.csproj` file, update `<ProjectReference>` paths
   - Test build: `dotnet build`
   - Commit: "Update project reference paths"

5. **Update Docker Configuration**
   - Update `Dockerfile` COPY commands
   - Update `docker-compose.yml` if needed
   - Test: `docker-compose build`
   - Commit: "Update Docker configuration for new paths"

6. **Update CI/CD**
   - Update GitHub Actions workflow files
   - Push branch and verify CI passes
   - Commit: "Update CI/CD pipeline paths"

7. **Verification**
   - Full solution build: `dotnet build`
   - Run tests: `dotnet test`
   - Docker build test: `docker-compose up --build`
   - Verify all paths work

8. **Merge**
   - Create PR
   - Merge to main
   - Notify team to pull and reopen solutions

## Open Questions

- Are there any scripts or tools in the repo that reference project paths hardcoded?
- Are there documentation files beyond README that reference project locations?
