// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Fabric.Mcp.Tools.Analytics.Services;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Fabric.Mcp.Tools.Analytics.UnitTests;

public class FabricAnalyticsSetupTests
{
    [Fact]
    public void ConfigureServices_RegistersAllRequiredServices()
    {
        // Arrange
        var setup = new FabricAnalyticsSetup();
        var services = new ServiceCollection();

        // Act
        setup.ConfigureServices(services);
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider.GetService<IFabricAnalyticsService>());
    }

    [Fact]
    public void RegisterCommands_ReturnsCommandGroupWithCorrectName()
    {
        // Arrange
        var setup = new FabricAnalyticsSetup();
        var services = new ServiceCollection();
        setup.ConfigureServices(services);
        
        // Add required dependencies that aren't auto-registered
        services.AddSingleton(Substitute.For<Microsoft.Extensions.Logging.ILogger<Commands.Health.WorkspaceHealthCommand>>());
        services.AddSingleton(Substitute.For<Microsoft.Extensions.Logging.ILogger<Commands.Optimize.PipelineOptimizeCommand>>());
        services.AddSingleton(Substitute.For<Microsoft.Extensions.Logging.ILogger<Commands.Diagnose.NotebookDiagnoseCommand>>());
        services.AddSingleton(Substitute.For<Microsoft.Extensions.Logging.ILogger<Infrastructure.FabricApiClient>>());
        services.AddSingleton(Substitute.For<Microsoft.Extensions.Logging.ILogger<Fabric.Mcp.Tools.Analytics.Services.FabricAnalyticsService>>());
        services.AddSingleton(Substitute.For<HttpClient>());
        
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var commandGroup = setup.RegisterCommands(serviceProvider);

        // Assert
        Assert.Equal("analytics", commandGroup.Name);
        Assert.Equal("Microsoft Fabric Analytics Tools", commandGroup.Title);
    }

    [Fact]
    public void Name_ReturnsCorrectValue()
    {
        // Arrange
        var setup = new FabricAnalyticsSetup();

        // Assert
        Assert.Equal("analytics", setup.Name);
    }

    [Fact]
    public void Title_ReturnsCorrectValue()
    {
        // Arrange
        var setup = new FabricAnalyticsSetup();

        // Assert
        Assert.Equal("Microsoft Fabric Analytics Tools", setup.Title);
    }
}
