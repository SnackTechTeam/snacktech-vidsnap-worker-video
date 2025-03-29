using adapter.amazon.s3.services;
using Amazon.S3;
using Amazon.S3.Model;
using core.domain.models;
using Microsoft.Extensions.Logging;
using Moq;
using unit.tests.helpers;

namespace unit.tests.adapter.amazon.s3
{
    public class S3BucketServiceTests
    {
        private readonly Mock<ILogger<S3BucketService>> mockLogger;
        private readonly Mock<IAmazonS3> mockS3Client;
        private readonly S3BucketService s3BucketService;

        public S3BucketServiceTests()
        {
            mockLogger = new Mock<ILogger<S3BucketService>>();
            mockS3Client = new Mock<IAmazonS3>();
            s3BucketService = new S3BucketService(mockLogger.Object, mockS3Client.Object);
        }

        [Fact]
        public async Task BaixarArquivoAsync_ShouldDownloadFileSuccessfully()
        {
            // Arrange
            var videoParaBaixar = ObjectsBuilder.VideoParaBaixarBuilder();

            var mockResponse = new GetObjectResponse
            {
                ResponseStream = new MemoryStream(new byte[] { 1, 2, 3, 4 })
            };

            mockS3Client
                .Setup(client => client.GetObjectAsync(It.IsAny<GetObjectRequest>(), default))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await s3BucketService.BaixarArquivoAsync(videoParaBaixar);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess());
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Download de")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.AtLeastOnce
            );
        }

        [Fact]
        public async Task BaixarArquivoAsync_ShouldHandleException()
        {
            // Arrange
            var videoParaBaixar = ObjectsBuilder.VideoParaBaixarBuilder();

            mockS3Client
                .Setup(client => client.GetObjectAsync(It.IsAny<GetObjectRequest>(), default))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await s3BucketService.BaixarArquivoAsync(videoParaBaixar);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess());
            Assert.NotNull(result.Exception);
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Test exception")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task SubirArquivoAsync_ShouldUploadFileSuccessfully()
        {
            // Arrange
            var bucketName = "test-bucket";
            var chaveS3 = "test-key";
            var caminhoLocalArquivo = "/tmp/test-file.mp4";

            var mockResponse = new PutObjectResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };

            mockS3Client
                .Setup(client => client.PutObjectAsync(It.IsAny<PutObjectRequest>(), default))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await s3BucketService.SubirArquivoAsync(bucketName, chaveS3, caminhoLocalArquivo);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess());
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Upload concluído")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task SubirArquivoAsync_ShouldHandleException()
        {
            // Arrange
            var bucketName = "test-bucket";
            var chaveS3 = "test-key";
            var caminhoLocalArquivo = "/tmp/test-file.mp4";

            mockS3Client
                .Setup(client => client.PutObjectAsync(It.IsAny<PutObjectRequest>(), default))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await s3BucketService.SubirArquivoAsync(bucketName, chaveS3, caminhoLocalArquivo);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess());
            Assert.NotNull(result.Exception);
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Test exception")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
        }
    }
}