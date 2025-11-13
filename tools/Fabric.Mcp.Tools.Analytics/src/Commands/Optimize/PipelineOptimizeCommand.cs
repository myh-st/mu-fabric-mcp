// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Mcp.Core.Commands;
using Fabric.Mcp.Tools.Analytics.Options;
using Fabric.Mcp.Tools.Analytics.Services;
using Microsoft.Extensions.Logging;

namespace Fabric.Mcp.Tools.Analytics.Commands.Optimize;

public sealed class PipelineOptimizeCommand(ILogger<PipelineOptimizeCommand> logger) : GlobalCommand<PipelineOptimizeOptions>()
{
    private const string CommandTitle = "Optimize Pipeline Configuration";

    private readonly ILogger<PipelineOptimizeCommand> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public override string Id => "a1f80251-df7b-4054-953b-5f452c42dd02";

    public override string Name => "optimize";

    public override string Description =>
        """
        Analyze and optimize a Microsoft Fabric data pipeline.
        This command reviews pipeline configuration, identifies performance issues,
        and provides optimization recommendations.
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
        command.Options.Add(FabricAnalyticsOptionDefinitions.PipelineId);
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
            var result = await analyticsService.OptimizePipelineAsync(
                options.WorkspaceId!,
                options.PipelineId!,
                options.IncludeRecommendations,
                cancellationToken);

            context.Response.Results = ResponseResult.Create(result, FabricAnalyticsJsonContext.Default.PipelineOptimizationResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error optimizing pipeline");
            HandleException(context, ex);
        }

        return context.Response;
    }

    protected override PipelineOptimizeOptions BindOptions(ParseResult parseResult)
    {
        var options = base.BindOptions(parseResult);
        options.WorkspaceId = parseResult.GetValueOrDefault<string>(FabricAnalyticsOptionDefinitions.WorkspaceId.Name);
        options.PipelineId = parseResult.GetValueOrDefault<string>(FabricAnalyticsOptionDefinitions.PipelineId.Name);
        options.IncludeRecommendations = parseResult.GetValueOrDefault<bool>(FabricAnalyticsOptionDefinitions.IncludeRecommendations.Name);
        return options;
    }
}
