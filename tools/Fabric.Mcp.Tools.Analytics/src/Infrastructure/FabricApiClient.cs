// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;

namespace Fabric.Mcp.Tools.Analytics.Infrastructure;

public sealed class FabricApiClient(HttpClient httpClient, ILogger<FabricApiClient> logger)
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    private readonly ILogger<FabricApiClient> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private const string BaseUrl = "https://api.fabric.microsoft.com/v1";
    private const int MaxRetries = 3;
    private const int RetryDelayMilliseconds = 1000;

    public async Task<string> GetAsync(string endpoint, CancellationToken cancellationToken = default)
    {
        var url = $"{BaseUrl}/{endpoint.TrimStart('/')}";
        
        _logger.LogInformation("Making GET request to {Url}", url);

        var retryCount = 0;
        Exception? lastException = null;

        while (retryCount < MaxRetries)
        {
            try
            {
                var response = await _httpClient.GetAsync(url, cancellationToken);
                response.EnsureSuccessStatusCode();
                
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                
                _logger.LogInformation("Successfully retrieved data from {Url}", url);
                
                return content;
            }
            catch (HttpRequestException ex) when (retryCount < MaxRetries - 1)
            {
                lastException = ex;
                retryCount++;
                var delay = RetryDelayMilliseconds * (int)Math.Pow(2, retryCount - 1);
                
                _logger.LogWarning("Request failed, retrying in {Delay}ms (attempt {RetryCount}/{MaxRetries})", delay, retryCount, MaxRetries);
                
                await Task.Delay(delay, cancellationToken);
            }
        }

        throw lastException ?? new InvalidOperationException("Request failed after retries");
    }
}

