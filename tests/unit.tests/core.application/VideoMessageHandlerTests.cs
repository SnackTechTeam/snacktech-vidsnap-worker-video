using core.application.services;
using core.domain.common;
using core.domain.dtos.messages;
using core.domain.models;
using core.domain.ports.adapter.amazon.s3;
using core.domain.ports.adapter.video;
using Microsoft.Extensions.Logging;
using Moq;
using unit.tests.helpers;
using DomainDtos = core.domain.dtos;

namespace unit.tests.core.application.services
{
    public class VideoMessageHandlerTest
    {
        private readonly Mock<IS3BucketService> s3BucketServiceMock;
        private readonly Mock<IExtrairImagensService> extrairImagensServiceMock;
        private readonly Mock<ICompactService> compactServiceMock;
        private readonly VideoMessageHandler handler;

        public VideoMessageHandlerTest()
        {
            s3BucketServiceMock = new Mock<IS3BucketService>();
            extrairImagensServiceMock = new Mock<IExtrairImagensService>();
            compactServiceMock = new Mock<ICompactService>();

            handler = new VideoMessageHandler(
                s3BucketServiceMock.Object,
                extrairImagensServiceMock.Object,
                compactServiceMock.Object
            );
        }

        [Fact]
        public async Task ProcessVideoMessageComSucesso()
        {
            var cliente = Guid.NewGuid();
            var idVideo = Guid.NewGuid();
            var newVideoDto = new NewVideoDto
            {
                
                Records = new List<DomainDtos.messages.Record>
                {
                    new DomainDtos.messages.Record
                    {
                        S3 = new S3
                        {
                            Bucket = new Bucket { Name = "test-bucket" },
                            Object = new DomainDtos.messages.Object { Key = $"{cliente}/video.mp4" }
                        }
                    }
                }
            };
            
            var videoParaBaixar = ObjectsBuilder.VideoParaBaixarBuilderWithValues(cliente.ToString(),idVideo.ToString(),"video.mp4");

            s3BucketServiceMock
                .Setup(s => s.BaixarArquivoAsync(It.IsAny<VideoParaBaixar>()))
                .ReturnsAsync(new Result<string>("download-path"));

            extrairImagensServiceMock
                .Setup(e => e.ExtrairImagensPorIntervaloAsync(It.IsAny<VideoParaBaixar>(), ConstantsValues.IntervaloPadraoEmSegundos))
                .ReturnsAsync(new Result<string>("frame1.jpg"));

            compactServiceMock
                .Setup(c => c.CompactarImagensDeVideo(It.IsAny<VideoParaBaixar>()))
                .Returns(new Result<string>("zip-path"));

            s3BucketServiceMock
                .Setup(s => s.SubirArquivoAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Result());

            var result = await handler.ProcessVideoMessage(videoParaBaixar);

            Assert.True(result.IsSuccess());
            Assert.NotNull(result.Data);
            Assert.Equal($"{idVideo}", result.Data.IdVideo);
        }

        [Fact]
        public async Task ProcessVideoMessageRetornaFalhaQuandoExecutarProcessoVideoFalha()
        {
            var videoParaBaixar = ObjectsBuilder.VideoParaBaixarBuilderWithValues(Guid.NewGuid().ToString(),Guid.NewGuid().ToString(),"video.mp4");

            s3BucketServiceMock
                .Setup(s => s.BaixarArquivoAsync(It.IsAny<VideoParaBaixar>()))
                .ReturnsAsync(new Result<string>(new Exception("Download failed")));

            var result = await handler.ProcessVideoMessage(videoParaBaixar);

            Assert.False(result.IsSuccess());
            Assert.Equal("Download failed", result.Exception.Message);
        }

        [Fact]
        public async Task ExecutarProcessoVideoComSucesso()
        {
            var videoParaBaixar = ObjectsBuilder.VideoParaBaixarBuilderWithValues(Guid.NewGuid().ToString(),Guid.NewGuid().ToString(),"video.mp4");

            s3BucketServiceMock
                .Setup(s => s.BaixarArquivoAsync(It.IsAny<VideoParaBaixar>()))
                .ReturnsAsync(new Result<string>("download-path"));

            extrairImagensServiceMock
                .Setup(e => e.ExtrairImagensPorIntervaloAsync(It.IsAny<VideoParaBaixar>(), ConstantsValues.IntervaloPadraoEmSegundos))
                .ReturnsAsync(new Result<string>("frame1.jpg"));

            compactServiceMock
                .Setup(c => c.CompactarImagensDeVideo(It.IsAny<VideoParaBaixar>()))
                .Returns(new Result<string>("zip-path"));

            var result = await handler.ExecutarProcessoVideo(videoParaBaixar);

            Assert.True(result.IsSuccess());
            Assert.Equal("frame1.jpg", result.Data);
        }

        [Fact]
        public async Task ExecutarProcessoVideoRetornaFalhaQuandoExtracaoDeImagemFalha()
        {
            var videoParaBaixar = ObjectsBuilder.VideoParaBaixarBuilderWithValues(Guid.NewGuid().ToString(),Guid.NewGuid().ToString(),"video.mp4");

            s3BucketServiceMock
                .Setup(s => s.BaixarArquivoAsync(It.IsAny<VideoParaBaixar>()))
                .ReturnsAsync(new Result<string>("download-path"));

            extrairImagensServiceMock
                .Setup(e => e.ExtrairImagensPorIntervaloAsync(It.IsAny<VideoParaBaixar>(), ConstantsValues.IntervaloPadraoEmSegundos))
                .ReturnsAsync(new Result<string>(new Exception("Image extraction failed")));

            var result = await handler.ExecutarProcessoVideo(videoParaBaixar);

            Assert.False(result.IsSuccess());
            Assert.Equal("Image extraction failed", result.Exception.Message);
        }

        [Fact]
        public async Task UploadArquivosProduzidosFuncionaCorretamente()
        {
            var cliente = Guid.NewGuid();
            var idVideo = Guid.NewGuid();
            var videoParaBaixar = ObjectsBuilder.VideoParaBaixarBuilderWithValues(cliente.ToString(),idVideo.ToString(),"video.mp4");

            var nomeFrame = "frame1.jpg";

            s3BucketServiceMock
                .Setup(s => s.SubirArquivoAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Result());

            var result = await handler.UploadArquivosProduzidos(videoParaBaixar, nomeFrame);

            Assert.NotNull(result);
            Assert.Equal($"{idVideo}", result.IdVideo);
            Assert.Equal($"s3://bucket-name/{cliente}/{idVideo}/image.jpg", result.UrlImagem);
            Assert.Equal($"s3://bucket-name/{cliente}/{idVideo}/video-images.zip", result.UrlZip);
        }
    }
}