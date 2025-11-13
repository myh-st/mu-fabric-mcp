# Fabric.Mcp.Tools.Analytics

Analytics tools for Microsoft Fabric - analyze workspace health, optimize pipelines, and diagnose notebooks.

## Features

This toolset provides three main analysis capabilities:

### 1. Workspace Health Analysis
Analyze the health status of your Fabric workspaces:
```bash
fabmcp analytics workspace health --workspace-id <workspace-id> --include-metrics
```

Features:
- Checks workspace state (Active/Inactive)
- Validates capacity assignment
- Identifies configuration issues
- Optional detailed metrics

### 2. Pipeline Optimization
Analyze and optimize your data pipelines:
```bash
fabmcp analytics pipeline optimize --workspace-id <workspace-id> --pipeline-id <pipeline-id> --include-recommendations
```

Features:
- Reviews pipeline configuration
- Identifies performance bottlenecks
- Provides optimization recommendations
- Suggests best practices

### 3. Notebook Diagnostics
Diagnose issues in your Fabric notebooks:
```bash
fabmcp analytics notebook diagnose --workspace-id <workspace-id> --notebook-id <notebook-id> --include-recommendations
```

Features:
- Analyzes notebook configuration
- Identifies potential issues
- Provides improvement recommendations
- Suggests optimization strategies

## Configuration

### Service Principal Authentication

The Analytics tools use Service Principal authentication to access the Fabric REST API. Configure the following environment variables:

```bash
FABRIC_TENANT_ID=your-tenant-id
FABRIC_CLIENT_ID=your-client-id
FABRIC_CLIENT_SECRET=your-client-secret
```

### Setting Up Service Principal

1. **Register an Application in Azure AD**:
   - Go to Azure Portal > Azure Active Directory > App registrations
   - Click "New registration"
   - Provide a name and register the application
   - Note the Application (Client) ID and Tenant ID

2. **Create a Client Secret**:
   - In your app registration, go to Certificates & secrets
   - Click "New client secret"
   - Set an expiration period and create
   - Copy the secret value (not the secret ID)

3. **Grant Permissions**:
   - In Fabric, grant the Service Principal access to the workspaces you want to analyze
   - Minimum required permission: Viewer (read-only access)

## Usage Examples

### Check Workspace Health
```bash
# Basic health check
fabmcp analytics workspace health --workspace-id abc123

# With detailed metrics
fabmcp analytics workspace health --workspace-id abc123 --include-metrics
```

### Optimize Pipeline
```bash
# Basic optimization analysis
fabmcp analytics pipeline optimize --workspace-id abc123 --pipeline-id def456

# With recommendations
fabmcp analytics pipeline optimize --workspace-id abc123 --pipeline-id def456 --include-recommendations
```

### Diagnose Notebook
```bash
# Basic diagnostic
fabmcp analytics notebook diagnose --workspace-id abc123 --notebook-id ghi789

# With recommendations
fabmcp analytics notebook diagnose --workspace-id abc123 --notebook-id ghi789 --include-recommendations
```

## Features

- **Read-Only Access**: All operations are read-only and do not modify your Fabric resources
- **Service Principal Support**: Uses Azure AD Service Principal for secure authentication
- **Automatic Retry**: Built-in retry logic with exponential backoff for transient failures
- **Comprehensive Logging**: Detailed logging for troubleshooting
- **AOT Compatible**: Fully compatible with Native AOT compilation

## Development

### Building
```bash
dotnet build Fabric.Mcp.Tools.Analytics.csproj
```

### Testing
```bash
# Unit tests
dotnet test tests/Fabric.Mcp.Tools.Analytics.UnitTests

# Live tests (requires configured credentials)
dotnet test tests/Fabric.Mcp.Tools.Analytics.LiveTests
```

## License

Copyright (c) Microsoft Corporation. Licensed under the MIT License.
