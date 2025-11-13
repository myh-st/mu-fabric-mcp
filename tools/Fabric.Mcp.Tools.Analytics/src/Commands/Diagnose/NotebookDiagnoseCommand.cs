// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Mcp.Core.Commands;
using Fabric.Mcp.Tools.Analytics.Options;
using Fabric.Mcp.Tools.Analytics.Services;
using Microsoft.Extensions.Logging;

namespace Fabric.Mcp.Tools.Analytics.Commands.Diagnose;

public sealed class NotebookDiagnoseCommand(ILogger<NotebookDiagnoseCommand> logger) : GlobalCommand<NotebookDiagnoseOptions>()
{
    private const string CommandTitle = "Diagnose Notebook Issues";

    private readonly ILogger<NotebookDiagnoseCommand> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public override string Id => "a1f80251-df7b-4054-953b-5f452c42dd03";

    public override string Name => "diagnose";

    public override string Description =>
        """
        Diagnose issues in a Microsoft Fabric notebook.
        This command analyzes notebook configuration, identifies potential issues,
        and provides recommendations for improvement.
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
        command.Options.Add(FabricAnalyticsOptionDefinitions.NotebookId);
        command.Options.Add(FabricAnalyticsOptionDefinitions.IncludeRecommendations);
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
            var result = await analyticsService.DiagnoseNotebookAsync(
                options.WorkspaceId!,
                options.NotebookId!,
                options.IncludeRecommendations,
                cancellationToken);

            context.Response.Results = ResponseResult.Create(result, FabricAnalyticsJsonContext.Default.NotebookDiagnosticResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error diagnosing notebook");
            HandleException(context, ex);
        }

        return context.Response;
    }

    protected override NotebookDiagnoseOptions BindOptions(ParseResult parseResult)
    {
        var options = base.BindOptions(parseResult);
        options.WorkspaceId = parseResult.GetValueOrDefault<string>(FabricAnalyticsOptionDefinitions.WorkspaceId.Name);
        options.NotebookId = parseResult.GetValueOrDefault<string>(FabricAnalyticsOptionDefinitions.NotebookId.Name);
        options.IncludeRecommendations = parseResult.GetValueOrDefault<bool>(FabricAnalyticsOptionDefinitions.IncludeRecommendations.Name);
        return options;
    }
}
