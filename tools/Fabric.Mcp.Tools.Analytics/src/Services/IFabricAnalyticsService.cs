// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Fabric.Mcp.Tools.Analytics.Models;

namespace Fabric.Mcp.Tools.Analytics.Services;

public interface IFabricAnalyticsService
{
    Task<FabricWorkspace> GetWorkspaceAsync(string workspaceId, CancellationToken cancellationToken = default);
    
    Task<WorkspaceHealthResult> AnalyzeWorkspaceHealthAsync(string workspaceId, bool includeMetrics, CancellationToken cancellationToken = default);
    
    Task<FabricPipeline> GetPipelineAsync(string workspaceId, string pipelineId, CancellationToken cancellationToken = default);
    
    Task<PipelineOptimizationResult> OptimizePipelineAsync(string workspaceId, string pipelineId, bool includeRecommendations, CancellationToken cancellationToken = default);
    
    Task<FabricNotebook> GetNotebookAsync(string workspaceId, string notebookId, CancellationToken cancellationToken = default);
    
    Task<NotebookDiagnosticResult> DiagnoseNotebookAsync(string workspaceId, string notebookId, bool includeRecommendations, CancellationToken cancellationToken = default);
}
