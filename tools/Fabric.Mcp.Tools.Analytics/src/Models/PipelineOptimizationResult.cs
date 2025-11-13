// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Fabric.Mcp.Tools.Analytics.Models;

public sealed class PipelineOptimizationResult
{
    public string PipelineId { get; set; } = string.Empty;
    public string PipelineName { get; set; } = string.Empty;
    public string OptimizationScore { get; set; } = string.Empty;
    public List<string> Recommendations { get; set; } = [];
    public List<string> PerformanceIssues { get; set; } = [];
}
