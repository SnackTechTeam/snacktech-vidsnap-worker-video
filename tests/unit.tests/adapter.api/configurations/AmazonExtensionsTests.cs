using adapter.api.Configuration;
using Amazon.S3;
using Amazon.SQS;
using core.domain.options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;

namespace adapter.api.configuration.tests
{
    public class AmazonExtensionsTest
    {
        [Fact]
        public void AddAmazonSqsDeveRegistrarIAmazonSQS()
        {
            var amazonOptions = new AmazonOptions
            {
                AwsAccessKeyId = "test-key",
                AwsSecretAccessKey = "test-secret",
                AwsSecretAccessToken = "test-token",
                Region = "us-east-1",
                UseLocalStack = false
            };
            var sqsOptions = new SqsOptions
            {
                ServiceUrl = "http://localhost:4566"
            };

            var amazonOptionsMock = new Mock<IOptions<AmazonOptions>>();
            var sqsOptionsMock = new Mock<IOptions<SqsOptions>>();

            amazonOptionsMock.Setup(o => o.Value).Returns(amazonOptions);
            sqsOptionsMock.Setup(o => o.Value).Returns(sqsOptions);

            var services = new ServiceCollection();
            services.AddSingleton(amazonOptionsMock.Object);
            services.AddSingleton(sqsOptionsMock.Object);

            services.AddAmazonSqs();
            var serviceProvider = services.BuildServiceProvider();
            var sqsClient = serviceProvider.GetService<IAmazonSQS>();

            Assert.NotNull(sqsClient);
            Assert.IsType<AmazonSQSClient>(sqsClient);
        }

        [Fact]
        public void AddAmazonS3DeveRegistrarIAmazonS3()
        {
            var amazonOptions = new AmazonOptions
            {
                AwsAccessKeyId = "test-key",
                AwsSecretAccessKey = "test-secret",
                AwsSecretAccessToken = "test-token",
                Region = "us-east-1",
                UseLocalStack = true
            };
            var s3Options = new S3Options
            {
                ServiceUrl = "http://localhost:4566"
            };

            var amazonOptionsMock = new Mock<IOptions<AmazonOptions>>();
            var s3OptionsMock = new Mock<IOptions<S3Options>>();

            amazonOptionsMock.Setup(o => o.Value).Returns(amazonOptions);
            s3OptionsMock.Setup(o => o.Value).Returns(s3Options);

            var services = new ServiceCollection();
            services.AddSingleton(amazonOptionsMock.Object);
            services.AddSingleton(s3OptionsMock.Object);

            services.AddAmazonS3();
            var serviceProvider = services.BuildServiceProvider();
            var s3Client = serviceProvider.GetService<IAmazonS3>();

            Assert.NotNull(s3Client);
            Assert.IsType<AmazonS3Client>(s3Client);
        }
    }
}