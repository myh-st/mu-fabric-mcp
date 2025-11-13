// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Fabric.Mcp.Tools.Analytics.Commands;
using Fabric.Mcp.Tools.Analytics.Infrastructure;
using Fabric.Mcp.Tools.Analytics.Models;
using Microsoft.Extensions.Logging;

namespace Fabric.Mcp.Tools.Analytics.Services;

public sealed class FabricAnalyticsService(
    FabricApiClient apiClient,
    ILogger<FabricAnalyticsService> logger) : IFabricAnalyticsService
{
    private readonly FabricApiClient _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    private readonly ILogger<FabricAnalyticsService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<FabricWorkspace> GetWorkspaceAsync(string workspaceId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting workspace {WorkspaceId}", workspaceId);
        
        var endpoint = $"workspaces/{workspaceId}";
        var response = await _apiClient.GetAsync(endpoint, cancellationToken);
        
        var workspace = JsonSerializer.Deserialize(response, FabricAnalyticsJsonContext.Default.FabricWorkspace) 
            ?? throw new InvalidOperationException("Failed to deserialize workspace response");
        
        return workspace;
    }

    public async Task<WorkspaceHealthResult> AnalyzeWorkspaceHealthAsync(string workspaceId, bool includeMetrics, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Analyzing health for workspace {WorkspaceId}", workspaceId);
        
        var workspace = await GetWorkspaceAsync(workspaceId, cancellationToken);
        
        var result = new WorkspaceHealthResult
        {
            WorkspaceId = workspace.Id,
            WorkspaceName = workspace.Name,
            HealthStatus = workspace.State == "Active" ? "Healthy" : "Warning",
            Issues = []
        };

        if (workspace.State != "Active")
        {
            result.Issues.Add($"Workspace state is {workspace.State}, expected Active");
        }

        if (string.IsNullOrEmpty(workspace.CapacityId))
        {
            result.Issues.Add("No capacity assigned to workspace");
        }

        if (includeMetrics)
        {
            result.Metrics = new Dictionary<string, object>
            {
                ["State"] = workspace.State,
                ["HasCapacity"] = !string.IsNullOrEmpty(workspace.CapacityId),
                ["IssueCount"] = result.Issues.Count
            };
        }

        return result;
    }

    public async Task<FabricPipeline> GetPipelineAsync(string workspaceId, string pipelineId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting pipeline {PipelineId} in workspace {WorkspaceId}", pipelineId, workspaceId);
        
        var endpoint = $"workspaces/{workspaceId}/items/{pipelineId}";
        var response = await _apiClient.GetAsync(endpoint, cancellationToken);
        
        var pipeline = JsonSerializer.Deserialize(response, FabricAnalyticsJsonContext.Default.FabricPipeline) 
            ?? throw new InvalidOperationException("Failed to deserialize pipeline response");
        
        return pipeline;
    }

    public async Task<PipelineOptimizationResult> OptimizePipelineAsync(string workspaceId, string pipelineId, bool includeRecommendations, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Optimizing pipeline {PipelineId} in workspace {WorkspaceId}", pipelineId, workspaceId);
        
        var pipeline = await GetPipelineAsync(workspaceId, pipelineId, cancellationToken);
        
        var result = new PipelineOptimizationResult
        {
            PipelineId = pipeline.Id,
            PipelineName = pipeline.Name,
            OptimizationScore = "Good",
            Recommendations = [],
            PerformanceIssues = []
        };

        if (includeRecommendations)
        {
            result.Recommendations.Add("Consider enabling parallel execution for independent activities");
            result.Recommendations.Add("Review activity timeout settings");
            result.Recommendations.Add("Implement error handling and retry policies");
        }

        return result;
    }

    public async Task<FabricNotebook> GetNotebookAsync(string workspaceId, string notebookId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting notebook {NotebookId} in workspace {WorkspaceId}", notebookId, workspaceId);
        
        var endpoint = $"workspaces/{workspaceId}/items/{notebookId}";
        var response = await _apiClient.GetAsync(endpoint, cancellationToken);
        
        var notebook = JsonSerializer.Deserialize(response, FabricAnalyticsJsonContext.Default.FabricNotebook) 
            ?? throw new InvalidOperationException("Failed to deserialize notebook response");
        
        return notebook;
    }

    public async Task<NotebookDiagnosticResult> DiagnoseNotebookAsync(string workspaceId, string notebookId, bool includeRecommendations, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Diagnosing notebook {NotebookId} in workspace {WorkspaceId}", notebookId, workspaceId);
        
        var notebook = await GetNotebookAsync(workspaceId, notebookId, cancellationToken);
        
        var result = new NotebookDiagnosticResult
        {
            NotebookId = notebook.Id,
            NotebookName = notebook.Name,
            DiagnosticStatus = "Healthy",
            Issues = [],
            Recommendations = []
        };

        if (includeRecommendations)
        {
            result.Recommendations.Add("Use cached data sources when appropriate");
            result.Recommendations.Add("Optimize spark configuration for workload");
            result.Recommendations.Add("Consider breaking large notebooks into smaller ones");
        }

        return result;
    }
}
