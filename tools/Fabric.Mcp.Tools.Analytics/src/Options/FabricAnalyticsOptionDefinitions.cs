// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Fabric.Mcp.Tools.Analytics.Options;

public static class FabricAnalyticsOptionDefinitions
{
    public const string WorkspaceIdName = "workspace-id";
    public static readonly Option<string> WorkspaceId = new($"--{WorkspaceIdName}", "-w")
    {
        Description = "The Fabric workspace ID",
        Required = true
    };

    public const string PipelineIdName = "pipeline-id";
    public static readonly Option<string> PipelineId = new($"--{PipelineIdName}", "-p")
    {
        Description = "The Fabric pipeline ID",
        Required = true
    };

    public const string NotebookIdName = "notebook-id";
    public static readonly Option<string> NotebookId = new($"--{NotebookIdName}", "-n")
    {
        Description = "The Fabric notebook ID",
        Required = true
    };

    public const string IncludeMetricsName = "include-metrics";
    public static readonly Option<bool> IncludeMetrics = new($"--{IncludeMetricsName}", "-m")
    {
        Description = "Include detailed metrics in the analysis"
    };

    public const string IncludeRecommendationsName = "include-recommendations";
    public static readonly Option<bool> IncludeRecommendations = new($"--{IncludeRecommendationsName}", "-r")
    {
        Description = "Include optimization recommendations"
    };
}
