using System.Diagnostics.CodeAnalysis;
using adapter.video.services;
using core.domain.ports.adapter.video;
using Microsoft.Extensions.DependencyInjection;

namespace adapter.video
{
    [ExcludeFromCodeCoverage]
    public static class AdapterVideoInjectionDependency
    {
        public static IServiceCollection AddAdapterVideoServices(this IServiceCollection services){
            services.AddTransient<IExtrairImagensService,ExtrairImagensService>();
            return services;
        }   
    }
}