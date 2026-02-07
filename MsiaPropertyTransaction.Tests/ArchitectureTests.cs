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
            typeof(Program).Assembly // API/Infrastructure
        )
        .Build();

    [Fact]
    public void DomainLayer_ShouldNotDependOnOtherProjects()
    {
        // Domain layer should have zero outgoing dependencies to API/Infrastructure
        var domainTypes = Types().That().ResideInNamespace("MsiaPropertyTransaction.Domain");
        var otherTypes = Types().That().DoNotResideInNamespace("MsiaPropertyTransaction.Domain");
        
        var rule = domainTypes
            .Should()
            .NotDependOnAny(otherTypes)
            .WithoutRequiringPositiveResults(); // Pass when no dependencies found

        rule.Check(Architecture);
    }

    [Fact]
    public void DomainLayer_ShouldNotReferenceEntityFrameworkCore()
    {
        // Domain entities should not reference EF Core
        var rule = Types()
            .That()
            .ResideInNamespace("MsiaPropertyTransaction.Domain.Entities")
            .Should()
            .NotDependOnAny(Types().That().ResideInNamespace("Microsoft.EntityFrameworkCore"))
            .WithoutRequiringPositiveResults(); // Pass when no dependencies found

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
            .WithoutRequiringPositiveResults(); // Pass when no dependencies found

        rule.Check(Architecture);
    }

    [Fact]
    public void ApiLayer_ShouldDependOnDomainLayer()
    {
        // API should have references to Domain types - verify using reflection
        var apiAssembly = typeof(Program).Assembly;
        var domainAssembly = typeof(PropertyTransaction).Assembly;
        
        // Check that API assembly references Domain assembly
        var apiReferencesDomain = apiAssembly.GetReferencedAssemblies()
            .Any(r => r.Name == domainAssembly.GetName().Name);
        
        Assert.True(apiReferencesDomain, 
            "API project should reference Domain project. This is required for Clean Architecture.");
    }

    [Fact]
    public void DomainAssemblyReferences_VerifyNoCyclicDependencies()
    {
        // Load assemblies manually to check references
        var domainAssembly = typeof(PropertyTransaction).Assembly;
        var apiAssembly = typeof(Program).Assembly;
        
        // Get referenced assemblies for Domain
        var domainReferences = domainAssembly.GetReferencedAssemblies()
            .Select(r => r.Name)
            .ToList();
        
        // Get referenced assemblies for API
        var apiReferences = apiAssembly.GetReferencedAssemblies()
            .Select(r => r.Name)
            .ToList();
        
        // Domain should NOT reference API
        Assert.DoesNotContain("MsiaPropertyTransaction", domainReferences);
        
        // API SHOULD reference Domain
        Assert.Contains("MsiaPropertyTransaction.Domain", apiReferences);
    }
}
