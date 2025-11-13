// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Fabric.Mcp.Tools.Analytics.Commands;
using Fabric.Mcp.Tools.Analytics.Infrastructure;
using Fabric.Mcp.Tools.Analytics.Models;
using Fabric.Mcp.Tools.Analytics.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Fabric.Mcp.Tools.Analytics.UnitTests.Services;

public class FabricAnalyticsServiceTests
{
    [Fact]
    public async Task AnalyzeWorkspaceHealthAsync_WithActiveWorkspace_ReturnsHealthyStatus()
    {
        // Arrange
        var workspaceId = "test-workspace-id";
        var workspaceJson = System.Text.Json.JsonSerializer.Serialize(
            new FabricWorkspace { Id = workspaceId, Name = "Test Workspace", State = "Active", CapacityId = "capacity-1" },
            FabricAnalyticsJsonContext.Default.FabricWorkspace);
        
        var mockApiClient = Substitute.For<FabricApiClient>(
            Substitute.For<HttpClient>(),
            Substitute.For<ILogger<FabricApiClient>>());
        
        mockApiClient.GetAsync($"workspaces/{workspaceId}", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(workspaceJson));
        
        var mockLogger = Substitute.For<ILogger<FabricAnalyticsService>>();
        var service = new FabricAnalyticsService(mockApiClient, mockLogger);

        // Act
        var result = await service.AnalyzeWorkspaceHealthAsync(workspaceId, true, TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Healthy", result.HealthStatus);
        Assert.Empty(result.Issues);
        Assert.NotNull(result.Metrics);
    }

    [Fact]
    public async Task AnalyzeWorkspaceHealthAsync_WithInactiveWorkspace_ReturnsWarningStatus()
    {
        // Arrange
        var workspaceId = "test-workspace-id";
        var workspaceJson = System.Text.Json.JsonSerializer.Serialize(
            new FabricWorkspace { Id = workspaceId, Name = "Test Workspace", State = "Inactive" },
            FabricAnalyticsJsonContext.Default.FabricWorkspace);
        
        var mockApiClient = Substitute.For<FabricApiClient>(
            Substitute.For<HttpClient>(),
            Substitute.For<ILogger<FabricApiClient>>());
        
        mockApiClient.GetAsync($"workspaces/{workspaceId}", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(workspaceJson));
        
        var mockLogger = Substitute.For<ILogger<FabricAnalyticsService>>();
        var service = new FabricAnalyticsService(mockApiClient, mockLogger);

        // Act
        var result = await service.AnalyzeWorkspaceHealthAsync(workspaceId, false, TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Warning", result.HealthStatus);
        Assert.NotEmpty(result.Issues);
        Assert.Contains(result.Issues, i => i.Contains("Inactive"));
    }
}
