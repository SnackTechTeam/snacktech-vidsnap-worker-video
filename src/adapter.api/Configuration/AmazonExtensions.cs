using System.Diagnostics.CodeAnalysis;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SQS;
using core.domain.options;
using Microsoft.Extensions.Options;

namespace adapter.api.Configuration
{
    [ExcludeFromCodeCoverage]
    public static class AmazonExtensions
    {
        public static IServiceCollection AddAmazonSqs(this IServiceCollection services){
            services.AddTransient<IAmazonSQS>(sp => {
                var settingsAmazon = sp.GetRequiredService<IOptions<AmazonOptions>>().Value;
                var settingsSqs = sp.GetRequiredService<IOptions<SqsOptions>>().Value;

                if(settingsAmazon.UseLocalStack)
                {
                    var config = new AmazonSQSConfig{
                    ServiceURL = settingsSqs.ServiceUrl
                    };
                    return new AmazonSQSClient(settingsAmazon.AwsAccessKeyId, settingsAmazon.AwsSecretAccessKey, settingsAmazon.AwsSecretAccessToken, config);
                }

                var credentials = new BasicAWSCredentials(settingsAmazon.AwsAccessKeyId,settingsAmazon.AwsSecretAccessKey);
                return new AmazonSQSClient(credentials,RegionEndpoint.GetBySystemName(settingsAmazon.Region));
            });
            return services;
        }

        public static IServiceCollection AddAmazonS3(this IServiceCollection services){
           
           services.AddSingleton<IAmazonS3>(sp => {
                var settingsAmazon = sp.GetRequiredService<IOptions<AmazonOptions>>().Value;
                var settingsS3 = sp.GetRequiredService<IOptions<S3Options>>().Value;
                var credentials = new BasicAWSCredentials(settingsAmazon.AwsAccessKeyId,settingsAmazon.AwsSecretAccessKey);
                if(settingsAmazon.UseLocalStack){
                    var s3Config = new AmazonS3Config{
                        ServiceURL = settingsS3.ServiceUrl,
                        ForcePathStyle = true
                    };
                    
                    return new AmazonS3Client(credentials,s3Config);
                }
                
                return new AmazonS3Client(credentials,RegionEndpoint.GetBySystemName(settingsAmazon.Region));
           });

           return services;
        }
    }
}