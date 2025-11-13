// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Fabric.Mcp.Tools.Analytics.Options;

public class PipelineOptimizeOptions : BaseFabricAnalyticsOptions
{
    public string? PipelineId { get; set; }
    public bool IncludeRecommendations { get; set; }
}
