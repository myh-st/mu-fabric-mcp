// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Mcp.Core.Options;

namespace Fabric.Mcp.Tools.Analytics.Options;

public class BaseFabricAnalyticsOptions : GlobalOptions
{
    public string? WorkspaceId { get; set; }
}
