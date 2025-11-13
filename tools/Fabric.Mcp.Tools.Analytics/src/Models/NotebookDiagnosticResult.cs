// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Fabric.Mcp.Tools.Analytics.Models;

public sealed class NotebookDiagnosticResult
{
    public string NotebookId { get; set; } = string.Empty;
    public string NotebookName { get; set; } = string.Empty;
    public string DiagnosticStatus { get; set; } = string.Empty;
    public List<string> Issues { get; set; } = [];
    public List<string> Recommendations { get; set; } = [];
}
