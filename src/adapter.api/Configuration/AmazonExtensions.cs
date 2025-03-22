using Amazon.Runtime;
using Amazon.S3;
using Amazon.SQS;
using core.domain.options;
using Microsoft.Extensions.Options;

namespace adapter.api.Configuration
{
    public static class AmazonExtensions
    {
        //TODO: Aplicar configuração para definir LocalStack ou AWS antes de instanciar os clients
        public static IServiceCollection AddAmazonSqs(this IServiceCollection services){
            services.AddTransient<IAmazonSQS>(sp => {
                var settingsAmazon = sp.GetRequiredService<IOptions<AmazonOptions>>().Value;
                var settingsSqs = sp.GetRequiredService<IOptions<SqsOptions>>().Value;
                var config = new AmazonSQSConfig{
                    ServiceURL = settingsSqs.ServiceUrl
                };
                return new AmazonSQSClient(settingsAmazon.AwsAccessKeyId, settingsAmazon.AwsSecretAccessKey, settingsAmazon.AwsSecretAccessToken, config);
            });
            return services;
        }

        public static IServiceCollection AddAmazonS3(this IServiceCollection services){
           
           services.AddSingleton<IAmazonS3>(sp => {
                var settingsAmazon = sp.GetRequiredService<IOptions<AmazonOptions>>().Value;
                var settingsS3 = sp.GetRequiredService<IOptions<S3Options>>().Value;
                var s3Config = new AmazonS3Config{
                    ServiceURL = settingsS3.ServiceUrl,
                    ForcePathStyle = true
                };
                var credentials = new BasicAWSCredentials(settingsAmazon.AwsAccessKeyId,settingsAmazon.AwsSecretAccessKey);

                return new AmazonS3Client(credentials,s3Config);
           });

           return services;
        }
    }
}