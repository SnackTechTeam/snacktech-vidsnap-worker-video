using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace adapter.api.Configuration.HealthChecks
{
    [ExcludeFromCodeCoverage]
    public static class HealthChecksExtensions
    {
        public static void UseCustomHealthChecks(this WebApplication app){
            app.MapHealthChecks("api/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    var result = JsonSerializer.Serialize(
                        new
                        {
                            status = report.Status.ToString(),
                            check = report.Entries.Select(entry => new {
                                name = entry.Key,
                                status = entry.Value.Status.ToString(),
                                exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                                duration = entry.Value.Duration.ToString()
                            })
                        }
                    );

                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(result);
                }
            });

            app.MapHealthChecks("api/health/live", new HealthCheckOptions
            {
                Predicate = (_) => false
            });
        }
    }
}