// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Fabric.Mcp.Tools.Analytics.Models;

public sealed class FabricWorkspace
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string State { get; set; } = string.Empty;
    public string? CapacityId { get; set; }
}
