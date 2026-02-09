## 1. Preparation

- [x] 1.1 Ensure all work is committed and working directory is clean
- [x] 1.2 Create feature branch: `git checkout -b restructure/src-tests-folders`
- [x] 1.3 Verify current build works: `dotnet build`
- [x] 1.4 Identify all project folders at root level

## 2. Create Folder Structure

- [x] 2.1 Create `src/` directory at repository root
- [x] 2.2 Create `tests/` directory at repository root
- [x] 2.3 Verify directories exist with `ls -la`

## 3. Move Source Projects to src/

- [ ] 3.1 Move MsiaPropertyTransaction to src/: `git mv MsiaPropertyTransaction src/`
- [ ] 3.2 Move MsiaPropertyTransaction.Domain to src/: `git mv MsiaPropertyTransaction.Domain src/`
- [ ] 3.3 Move MsiaPropertyTransaction.Application to src/: `git mv MsiaPropertyTransaction.Application src/`
- [ ] 3.4 Move MsiaPropertyTransaction.Infrastructure to src/: `git mv MsiaPropertyTransaction.Infrastructure src/`
- [ ] 3.5 Commit: "Move source projects to src/ folder"

## 4. Update Solution File

- [ ] 4.1 Open `.slnx` file and update all project paths from `ProjectName\` to `src\ProjectName\`
- [ ] 4.2 Verify solution loads correctly in IDE
- [ ] 4.3 Commit: "Update solution file with new project paths"

## 5. Update Project References

- [ ] 5.1 Update src/MsiaPropertyTransaction/MsiaPropertyTransaction.csproj references
- [ ] 5.2 Update src/MsiaPropertyTransaction.Application/MsiaPropertyTransaction.Application.csproj references
- [ ] 5.3 Update src/MsiaPropertyTransaction.Infrastructure/MsiaPropertyTransaction.Infrastructure.csproj references
- [ ] 5.4 Test build: `dotnet build` - must pass
- [ ] 5.5 Commit: "Update project reference paths"

## 6. Update Docker Configuration

- [ ] 6.1 Update Dockerfile COPY commands from `ProjectName/` to `src/ProjectName/`
- [ ] 6.2 Update docker-compose.yml if it has path references
- [ ] 6.3 Test Docker build: `docker-compose build`
- [ ] 6.4 Commit: "Update Docker configuration for new paths"

## 7. Update CI/CD Pipeline

- [ ] 7.1 Check `.github/workflows/` for workflow files
- [ ] 7.2 Update any hardcoded paths in GitHub Actions workflows
- [ ] 7.3 Update build steps if they reference specific project paths
- [ ] 7.4 Push branch to remote and verify CI passes
- [ ] 7.5 Commit: "Update CI/CD pipeline paths"

## 8. Update Documentation

- [ ] 8.1 Update README.md if it references project structure or paths
- [ ] 8.2 Check for any other documentation files with path references
- [ ] 8.3 Update .gitignore if it has project-specific entries
- [ ] 8.4 Commit: "Update documentation for new project structure"

## 9. Final Verification

- [ ] 9.1 Full solution build: `dotnet build` (clean build)
- [ ] 9.2 Docker compose build: `docker-compose up --build` (must succeed)
- [ ] 9.3 Verify IDE solution explorer shows correct folder hierarchy
- [ ] 9.4 Check git status - ensure all changes are tracked

## 10. Merge and Cleanup

- [ ] 10.1 Create pull request with detailed description
- [ ] 10.2 Request code review
- [ ] 10.3 Merge to main branch
- [ ] 10.4 Notify team members to pull changes and reopen solutions
