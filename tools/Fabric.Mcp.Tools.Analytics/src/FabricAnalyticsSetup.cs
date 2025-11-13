// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Mcp.Core.Areas;
using Azure.Mcp.Core.Commands;
using Fabric.Mcp.Tools.Analytics.Commands.Diagnose;
using Fabric.Mcp.Tools.Analytics.Commands.Health;
using Fabric.Mcp.Tools.Analytics.Commands.Optimize;
using Fabric.Mcp.Tools.Analytics.Infrastructure;
using Fabric.Mcp.Tools.Analytics.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fabric.Mcp.Tools.Analytics;

public class FabricAnalyticsSetup : IAreaSetup
{
    public string Name => "analytics";

    public string Title => "Microsoft Fabric Analytics Tools";

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IFabricAnalyticsService, FabricAnalyticsService>();
        
        services.AddHttpClient<FabricApiClient>();

        services.AddSingleton<WorkspaceHealthCommand>();
        services.AddSingleton<PipelineOptimizeCommand>();
        services.AddSingleton<NotebookDiagnoseCommand>();
    }

    public CommandGroup RegisterCommands(IServiceProvider serviceProvider)
    {
        var analytics = new CommandGroup(Name,
            """
            Microsoft Fabric Analytics - Tools for analyzing and optimizing Fabric resources.
            Use this toolset when you need to:
            - Check the health status of workspaces
            - Optimize data pipeline performance
            - Diagnose notebook issues and get recommendations
            
            This toolset provides read-only access to Fabric resources via the Fabric REST API
            using Service Principal authentication. Configure credentials in .env file with:
            - FABRIC_TENANT_ID
            - FABRIC_CLIENT_ID
            - FABRIC_CLIENT_SECRET
            """, Title);

        // Create subgroups for commands
        var workspace = new CommandGroup("workspace", "Workspace Operations - Commands for analyzing workspace health and configuration.");
        analytics.AddSubGroup(workspace);

        var pipeline = new CommandGroup("pipeline", "Pipeline Operations - Commands for optimizing data pipeline performance.");
        analytics.AddSubGroup(pipeline);

        var notebook = new CommandGroup("notebook", "Notebook Operations - Commands for diagnosing notebook issues.");
        analytics.AddSubGroup(notebook);

        // Register commands
        var workspaceHealth = serviceProvider.GetRequiredService<WorkspaceHealthCommand>();
        workspace.AddCommand(workspaceHealth.Name, workspaceHealth);

        var pipelineOptimize = serviceProvider.GetRequiredService<PipelineOptimizeCommand>();
        pipeline.AddCommand(pipelineOptimize.Name, pipelineOptimize);

        var notebookDiagnose = serviceProvider.GetRequiredService<NotebookDiagnoseCommand>();
        notebook.AddCommand(notebookDiagnose.Name, notebookDiagnose);

        return analytics;
    }
}
