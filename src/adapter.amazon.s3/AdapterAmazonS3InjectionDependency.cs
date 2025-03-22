using System.Diagnostics.CodeAnalysis;
using adapter.amazon.s3.services;
using core.domain.ports.adapter.amazon.s3;
using Microsoft.Extensions.DependencyInjection;

namespace adapter.amazon.s3
{
    [ExcludeFromCodeCoverage]
    public static class AdapterAmazonS3InjectionDependency
    {
        public static IServiceCollection AddAdapterAmazonS3Services(this IServiceCollection services){
            services.AddTransient<IS3BucketService,S3BucketService>();
            return services;
        }
    }
}