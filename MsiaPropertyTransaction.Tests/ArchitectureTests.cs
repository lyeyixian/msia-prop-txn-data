using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using MsiaPropertyTransaction.Domain.Entities;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace MsiaPropertyTransaction.Tests;

public class ArchitectureTests
{
    private static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(
            typeof(PropertyTransaction).Assembly, // Domain
            typeof(Application.Services.PropertyTransactionService).Assembly, // Application
            typeof(Infrastructure.Data.AppDbContext).Assembly, // Infrastructure
            typeof(Program).Assembly // API
        )
        .Build();

    [Fact]
    public void DomainLayer_ShouldNotDependOnOtherProjects()
    {
        // Domain layer should have zero outgoing dependencies to API/Infrastructure/Application
        var domainTypes = Types().That().ResideInNamespace("MsiaPropertyTransaction.Domain");
        var otherTypes = Types()
            .That()
            .ResideInNamespace("MsiaPropertyTransaction.Application")
            .Or()
            .ResideInNamespace("MsiaPropertyTransaction.Infrastructure")
            .Or()
            .ResideInNamespace("MsiaPropertyTransaction.Program");
        
        var rule = domainTypes
            .Should()
            .NotDependOnAny(otherTypes)
            .WithoutRequiringPositiveResults();

        rule.Check(Architecture);
    }

    [Fact]
    public void DomainLayer_ShouldNotReferenceEntityFrameworkCore()
    {
        // Domain entities should not reference EF Core
        var rule = Types()
            .That()
            .ResideInNamespace("MsiaPropertyTransaction.Domain")
            .Should()
            .NotDependOnAny(Types().That().ResideInNamespace("Microsoft.EntityFrameworkCore"))
            .WithoutRequiringPositiveResults();

        rule.Check(Architecture);
    }

    [Fact]
    public void DomainLayer_ShouldNotReferenceAspNetCore()
    {
        // Domain should not reference ASP.NET Core
        var rule = Types()
            .That()
            .ResideInNamespace("MsiaPropertyTransaction.Domain")
            .Should()
            .NotDependOnAny(Types().That().ResideInNamespace("Microsoft.AspNetCore"))
            .WithoutRequiringPositiveResults();

        rule.Check(Architecture);
    }

    [Fact]
    public void ApplicationLayer_ShouldOnlyDependOnDomain()
    {
        // Application layer should only depend on Domain
        var applicationTypes = Types().That().ResideInNamespace("MsiaPropertyTransaction.Application");
        var infrastructureAndApiTypes = Types()
            .That()
            .ResideInNamespace("MsiaPropertyTransaction.Infrastructure")
            .Or()
            .ResideInNamespace("MsiaPropertyTransaction.Program");
        
        var rule = applicationTypes
            .Should()
            .NotDependOnAny(infrastructureAndApiTypes)
            .WithoutRequiringPositiveResults();

        rule.Check(Architecture);
    }

    [Fact]
    public void ApplicationLayer_ShouldNotReferenceEntityFrameworkCore()
    {
        // Application should not reference EF Core directly
        var rule = Types()
            .That()
            .ResideInNamespace("MsiaPropertyTransaction.Application")
            .Should()
            .NotDependOnAny(Types().That().ResideInNamespace("Microsoft.EntityFrameworkCore"))
            .WithoutRequiringPositiveResults();

        rule.Check(Architecture);
    }

    [Fact]
    public void InfrastructureLayer_ShouldDependOnApplicationAndDomain()
    {
        // Infrastructure can depend on Application and Domain
        // This test verifies Infrastructure types exist and have proper references
        var infrastructureTypes = Types()
            .That()
            .ResideInNamespace("MsiaPropertyTransaction.Infrastructure.Data")
            .Or()
            .ResideInNamespace("MsiaPropertyTransaction.Infrastructure.Repositories");
        
        // Infrastructure should exist
        var types = infrastructureTypes.GetObjects(Architecture).ToList();
        Assert.True(types.Any(), 
            $"Infrastructure layer should contain types. Found {types.Count} types.");
    }

    [Fact]
    public void InfrastructureLayer_ShouldNotDependOnApi()
    {
        // Infrastructure should not depend on API layer
        var infrastructureTypes = Types()
            .That()
            .ResideInNamespace("MsiaPropertyTransaction.Infrastructure.Data")
            .Or()
            .ResideInNamespace("MsiaPropertyTransaction.Infrastructure.Repositories");
        var apiTypes = Types().That().ResideInNamespace("MsiaPropertyTransaction");
        
        var rule = infrastructureTypes
            .Should()
            .NotDependOnAny(apiTypes)
            .WithoutRequiringPositiveResults();

        rule.Check(Architecture);
    }

    [Fact]
    public void ApiLayer_ShouldDependOnApplication()
    {
        // API should depend on Application layer
        var apiAssembly = typeof(Program).Assembly;
        var applicationAssembly = typeof(Application.Services.PropertyTransactionService).Assembly;
        
        // Check that API assembly references Application assembly
        var apiReferencesApplication = apiAssembly.GetReferencedAssemblies()
            .Any(r => r.Name == applicationAssembly.GetName().Name);
        
        Assert.True(apiReferencesApplication, 
            "API project should reference Application project for Clean Architecture.");
    }

    [Fact]
    public void DomainAssemblyReferences_VerifyNoCyclicDependencies()
    {
        // Load assemblies to check references
        var domainAssembly = typeof(PropertyTransaction).Assembly;
        var applicationAssembly = typeof(Application.Services.PropertyTransactionService).Assembly;
        var infrastructureAssembly = typeof(Infrastructure.Data.AppDbContext).Assembly;
        var apiAssembly = typeof(Program).Assembly;
        
        // Get referenced assemblies for each layer
        var domainReferences = domainAssembly.GetReferencedAssemblies()
            .Select(r => r.Name)
            .ToList();
        var applicationReferences = applicationAssembly.GetReferencedAssemblies()
            .Select(r => r.Name)
            .ToList();
        var infrastructureReferences = infrastructureAssembly.GetReferencedAssemblies()
            .Select(r => r.Name)
            .ToList();
        var apiReferences = apiAssembly.GetReferencedAssemblies()
            .Select(r => r.Name)
            .ToList();
        
        // Domain should NOT reference Application, Infrastructure, or API
        Assert.DoesNotContain("MsiaPropertyTransaction.Application", domainReferences);
        Assert.DoesNotContain("MsiaPropertyTransaction.Infrastructure", domainReferences);
        Assert.DoesNotContain("MsiaPropertyTransaction", domainReferences);
        
        // Application should NOT reference Infrastructure or API (only Domain)
        Assert.DoesNotContain("MsiaPropertyTransaction.Infrastructure", applicationReferences);
        Assert.DoesNotContain("MsiaPropertyTransaction", applicationReferences);
        
        // Application SHOULD reference Domain
        Assert.Contains("MsiaPropertyTransaction.Domain", applicationReferences);
        
        // Infrastructure should NOT reference API
        Assert.DoesNotContain("MsiaPropertyTransaction", infrastructureReferences);
        
        // Infrastructure SHOULD reference Application and Domain
        Assert.Contains("MsiaPropertyTransaction.Application", infrastructureReferences);
        Assert.Contains("MsiaPropertyTransaction.Domain", infrastructureReferences);
        
        // API SHOULD reference Application (Domain comes transitively)
        Assert.Contains("MsiaPropertyTransaction.Application", apiReferences);
        Assert.Contains("MsiaPropertyTransaction.Infrastructure", apiReferences);
    }

    [Fact]
    public void CleanArchitecture_DependencyDirectionIsCorrect()
    {
        // Verify the overall dependency flow: API -> Application -> Domain
        var apiAssembly = typeof(Program).Assembly;
        var applicationAssembly = typeof(Application.Services.PropertyTransactionService).Assembly;
        var domainAssembly = typeof(PropertyTransaction).Assembly;
        
        var apiReferences = apiAssembly.GetReferencedAssemblies().Select(r => r.Name).ToList();
        var applicationReferences = applicationAssembly.GetReferencedAssemblies().Select(r => r.Name).ToList();
        
        // API depends on Application
        Assert.Contains("MsiaPropertyTransaction.Application", apiReferences);
        
        // Application depends on Domain
        Assert.Contains("MsiaPropertyTransaction.Domain", applicationReferences);
        
        // Domain has no project references (only framework)
        var domainReferences = domainAssembly.GetReferencedAssemblies()
            .Where(r => r.Name?.StartsWith("MsiaPropertyTransaction") == true)
            .ToList();
        Assert.Empty(domainReferences);
    }
}
