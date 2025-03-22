using core.application.services;
using core.domain.ports.adapter.video;
using core.domain.ports.core.application;
using Microsoft.Extensions.DependencyInjection;

namespace core.application
{
    public static class AdapterCoreAplicationInjectionDependency
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services){
            services.AddTransient<IVideoMessageHandler, VideoMessageHandler>();
            services.AddTransient<ICompactService, CompactService>();
            return services;
        }
    }
}