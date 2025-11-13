// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Mcp.Core.Commands;
using Fabric.Mcp.Tools.Analytics.Options;
using Fabric.Mcp.Tools.Analytics.Services;
using Microsoft.Extensions.Logging;

namespace Fabric.Mcp.Tools.Analytics.Commands.Health;

public sealed class WorkspaceHealthCommand(ILogger<WorkspaceHealthCommand> logger) : GlobalCommand<WorkspaceHealthOptions>()
{
    private const string CommandTitle = "Analyze Workspace Health";

    private readonly ILogger<WorkspaceHealthCommand> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public override string Id => "a1f80251-df7b-4054-953b-5f452c42dd01";

    public override string Name => "health";

    public override string Description =>
        """
        Analyze the health status of a Microsoft Fabric workspace.
        This command checks workspace state, capacity assignment, and other health indicators.
        Returns health status, identified issues, and optional metrics.
        """;

    public override string Title => CommandTitle;

    public override ToolMetadata Metadata => new()
    {
        Destructive = false,
        Idempotent = true,
        OpenWorld = false,
        ReadOnly = true,
        LocalRequired = false,
        Secret = false
    };

    protected override void RegisterOptions(Command command)
    {
        base.RegisterOptions(command);
        command.Options.Add(FabricAnalyticsOptionDefinitions.WorkspaceId);
        command.Options.Add(FabricAnalyticsOptionDefinitions.IncludeMetrics);
    }

    public override async Task<CommandResponse> ExecuteAsync(CommandContext context, ParseResult parseResult, CancellationToken cancellationToken)
    {
        try
        {
            var options = BindOptions(parseResult);
            
            if (!Validate(parseResult.CommandResult, context.Response).IsValid)
            {
                return context.Response;
            }

            var analyticsService = context.GetService<IFabricAnalyticsService>();
            var result = await analyticsService.AnalyzeWorkspaceHealthAsync(
                options.WorkspaceId!,
                options.IncludeMetrics,
                cancellationToken);

            context.Response.Results = ResponseResult.Create(result, FabricAnalyticsJsonContext.Default.WorkspaceHealthResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing workspace health");
            HandleException(context, ex);
        }

        return context.Response;
    }

    protected override WorkspaceHealthOptions BindOptions(ParseResult parseResult)
    {
        var options = base.BindOptions(parseResult);
        options.WorkspaceId = parseResult.GetValueOrDefault<string>(FabricAnalyticsOptionDefinitions.WorkspaceId.Name);
        options.IncludeMetrics = parseResult.GetValueOrDefault<bool>(FabricAnalyticsOptionDefinitions.IncludeMetrics.Name);
        return options;
    }
}
