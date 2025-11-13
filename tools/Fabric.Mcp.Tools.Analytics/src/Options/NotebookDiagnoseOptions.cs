// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Fabric.Mcp.Tools.Analytics.Options;

public class NotebookDiagnoseOptions : BaseFabricAnalyticsOptions
{
    public string? NotebookId { get; set; }
    public bool IncludeRecommendations { get; set; }
}
