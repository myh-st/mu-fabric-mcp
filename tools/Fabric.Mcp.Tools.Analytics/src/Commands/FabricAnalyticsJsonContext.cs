// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text.Json.Serialization;
using Fabric.Mcp.Tools.Analytics.Models;

namespace Fabric.Mcp.Tools.Analytics.Commands;

[JsonSerializable(typeof(WorkspaceHealthResult))]
[JsonSerializable(typeof(PipelineOptimizationResult))]
[JsonSerializable(typeof(NotebookDiagnosticResult))]
[JsonSerializable(typeof(FabricWorkspace))]
[JsonSerializable(typeof(FabricPipeline))]
[JsonSerializable(typeof(FabricNotebook))]
[JsonSerializable(typeof(string))]
public partial class FabricAnalyticsJsonContext : JsonSerializerContext
{
}
