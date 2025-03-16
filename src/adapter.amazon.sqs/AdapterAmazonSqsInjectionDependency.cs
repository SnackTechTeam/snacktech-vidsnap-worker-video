using adapter.amazon.sqs.services;
using core.domain.ports.adapter.amazon.sqs;
using Microsoft.Extensions.DependencyInjection;

namespace adapter.amazon.sqs
{
    public static class AdapterAmazonSqsInjectionDependency
    {
        public static IServiceCollection AddAdapterAmazonSqsServices(this IServiceCollection services){
            services.AddTransient<ISqsMessagingService,SqsMessagingService>();
            return services;
        }
    }
}