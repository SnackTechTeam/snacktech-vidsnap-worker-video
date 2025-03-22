using core.domain.common;
using core.domain.dtos.messages;
using core.domain.models;
using core.domain.ports.adapter.amazon.s3;
using core.domain.ports.adapter.amazon.sqs;
using core.domain.ports.adapter.video;
using core.domain.ports.core.application;
using Microsoft.Extensions.Logging;

namespace core.application.services
{
    public class VideoMessageHandler : IVideoMessageHandler
    {
        private readonly ILogger<VideoMessageHandler> logger;
        private readonly IS3BucketService s3BucketService;
        private readonly IExtrairImagensService extrairImagensService;
        private readonly ICompactService compactService;
        private readonly ISqsMessagingService sqsMessagingService;

        public VideoMessageHandler(ILogger<VideoMessageHandler> logger, 
                                    IS3BucketService s3BucketService,
                                    IExtrairImagensService extrairImagensService,
                                    ICompactService compactService,
                                    ISqsMessagingService sqsMessagingService)
        {
            this.logger = logger;
            this.s3BucketService = s3BucketService;
            this.extrairImagensService = extrairImagensService;
            this.compactService = compactService;
            this.sqsMessagingService = sqsMessagingService;
        }

        public async Task<Result<VideoProcessingSuccessDto>> ProcessVideoMessage(NewVideoDto newVideoDto)
        {
            var record = newVideoDto.Records.First();
            
            var objectKeyArray = record.S3.Object.Key.Split("/");
                VideoParaBaixar videoParaBaixar = new VideoParaBaixar{
                    Cliente = objectKeyArray.First(),
                    NomeVideo = objectKeyArray.Last(),
                    Bucket = record.S3.Bucket.Name,
                    Chave = record.S3.Object.Key,
                    CaminhoChave = Path.Combine(objectKeyArray.Take(objectKeyArray.Length - 1).ToArray())
                };

                //TODO: Reduzir duplicação de código
                var resultDownloadFile = await s3BucketService.BaixarArquivoAsync(videoParaBaixar);
                
                if(!resultDownloadFile.IsSuccess())
                    return new Result<VideoProcessingSuccessDto>(resultDownloadFile.Exception);
                
                //aqui eu tenho o nome do primeiro frame
                var resultImageExtraction = await extrairImagensService.ExtrairImagensPorIntervaloAsync(videoParaBaixar,ConstantsValues.IntervaloPadraoEmSegundos);

                if(!resultImageExtraction.IsSuccess())
                    return new Result<VideoProcessingSuccessDto>(resultImageExtraction.Exception);

                var resultCompactingImages = compactService.CompactarImagensDeVideo(videoParaBaixar);

                if(!resultCompactingImages.IsSuccess())
                    return new Result<VideoProcessingSuccessDto>(resultCompactingImages.Exception);

                //TODO: Reduzir duplicação de código
                var caminhoS3Zip = Path.Combine(videoParaBaixar.CaminhoChave,videoParaBaixar.NomeZip());
                var caminhoS3Imagem = Path.Combine(videoParaBaixar.CaminhoChave,resultImageExtraction.Data);
                var caminhoLocalFrame = Path.Combine(videoParaBaixar.DestinoImagens(),resultImageExtraction.Data);
                await s3BucketService.SubirArquivoAsync(videoParaBaixar.Bucket,caminhoS3Zip,videoParaBaixar.CaminhoZipCompleto());
                await s3BucketService.SubirArquivoAsync(videoParaBaixar.Bucket,caminhoS3Imagem,caminhoLocalFrame);

                Directory.Delete(videoParaBaixar.DestinoLocal(),true);

                var successMessage = new VideoProcessingSuccessDto{
                    ObjectKey = videoParaBaixar.Chave,
                    ImagePath = caminhoS3Imagem,
                    ZipPath = caminhoS3Zip
                };

                return new Result<VideoProcessingSuccessDto>(successMessage);            
        }
    }
}