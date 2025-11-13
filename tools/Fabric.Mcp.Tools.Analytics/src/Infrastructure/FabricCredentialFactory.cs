// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Fabric.Mcp.Tools.Analytics.Infrastructure;

public sealed class FabricCredentialFactory
{
    public static FabricCredentials LoadFromEnvironment()
    {
        var tenantId = Environment.GetEnvironmentVariable("FABRIC_TENANT_ID");
        var clientId = Environment.GetEnvironmentVariable("FABRIC_CLIENT_ID");
        var clientSecret = Environment.GetEnvironmentVariable("FABRIC_CLIENT_SECRET");

        return new FabricCredentials
        {
            TenantId = tenantId ?? throw new InvalidOperationException("FABRIC_TENANT_ID environment variable is required"),
            ClientId = clientId ?? throw new InvalidOperationException("FABRIC_CLIENT_ID environment variable is required"),
            ClientSecret = clientSecret ?? throw new InvalidOperationException("FABRIC_CLIENT_SECRET environment variable is required")
        };
    }
}

public sealed class FabricCredentials
{
    public string TenantId { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}
