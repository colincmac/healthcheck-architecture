using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Net.Http;

namespace Showcase.HealthChecks.Web;

public class BackendHealthCheck : IHealthCheck
{
    private readonly WeatherApiClient _httpClient;

    public const int MaxSuccessCode = 299;
    public const int MinSuccessCode = 200;

    public BackendHealthCheck(WeatherApiClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <inheritdoc />
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            if (cancellationToken.IsCancellationRequested) return new HealthCheckResult(context.Registration.FailureStatus, description: $"{nameof(BackendHealthCheck)} execution is cancelled.");

            var response = await _httpClient.CheckLivelinessAsync(cancellationToken);

            if (!((int)response.StatusCode >= MinSuccessCode && (int)response.StatusCode <= MaxSuccessCode)) return new HealthCheckResult(context.Registration.FailureStatus, description: $"Endpoint is not responding with code in {MinSuccessCode}...{MaxSuccessCode} range, the current status is {response.StatusCode}.");

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
        }
    }
}
