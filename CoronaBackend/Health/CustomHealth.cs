using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CoronaBackend.Health
{
    public class CustomHealth : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(HealthCheckResult.Healthy("It is work"));
        }
    }
}
