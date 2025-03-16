using adapter.video.services;
using core.domain.ports.adapter.video;
using Microsoft.Extensions.DependencyInjection;

namespace adapter.video
{
    public static class AdapterVideoInjectionDependency
    {
        public static IServiceCollection AddAdapterVideoServices(this IServiceCollection services){
            services.AddTransient<IExtrairImagensService,ExtrairImagensService>();
            return services;
        }   
    }
}