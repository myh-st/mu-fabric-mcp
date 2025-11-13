// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Fabric.Mcp.Tools.Analytics.Models;

public sealed class WorkspaceHealthResult
{
    public string WorkspaceId { get; set; } = string.Empty;
    public string WorkspaceName { get; set; } = string.Empty;
    public string HealthStatus { get; set; } = string.Empty;
    public List<string> Issues { get; set; } = [];
    public Dictionary<string, object>? Metrics { get; set; }
}
