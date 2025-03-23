using core.domain.common;
using core.domain.dtos.messages;
using core.domain.models;
using core.domain.ports.adapter.amazon.s3;
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

        public VideoMessageHandler(ILogger<VideoMessageHandler> logger, 
                                    IS3BucketService s3BucketService,
                                    IExtrairImagensService extrairImagensService,
                                    ICompactService compactService)
        {
            this.logger = logger;
            this.s3BucketService = s3BucketService;
            this.extrairImagensService = extrairImagensService;
            this.compactService = compactService;
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

                var execucaoVideo = await ExecutarProcessoVideo(videoParaBaixar);

                if(!execucaoVideo.IsSuccess())
                    return new Result<VideoProcessingSuccessDto>(execucaoVideo.Exception);

                var mensagemSucesso = await UploadArquivosProduzidos(videoParaBaixar, execucaoVideo.Data);

                var destinoLocal = videoParaBaixar.DestinoLocal();

                if(Directory.Exists(destinoLocal))
                    Directory.Delete(destinoLocal,true);

                return new Result<VideoProcessingSuccessDto>(mensagemSucesso);            
        }

        public async Task<Result<string>> ExecutarProcessoVideo(VideoParaBaixar videoParaBaixar){
            var resultDownloadFile = await s3BucketService.BaixarArquivoAsync(videoParaBaixar);
                
            if(!resultDownloadFile.IsSuccess())
                return new Result<string>(resultDownloadFile.Exception);
                
            //aqui eu tenho o nome do primeiro frame
            var resultImageExtraction = await extrairImagensService.ExtrairImagensPorIntervaloAsync(videoParaBaixar,ConstantsValues.IntervaloPadraoEmSegundos);

            if(!resultImageExtraction.IsSuccess())
                return new Result<string>(resultImageExtraction.Exception);

            var resultCompactingImages = compactService.CompactarImagensDeVideo(videoParaBaixar);

            if(!resultCompactingImages.IsSuccess())
                return new Result<string>(resultCompactingImages.Exception);

            return new Result<string>(resultImageExtraction.Data);    
        }

        public async Task<VideoProcessingSuccessDto> UploadArquivosProduzidos(VideoParaBaixar videoParaBaixar, string nomeFrame){
            var caminhoS3Zip = Path.Combine(videoParaBaixar.CaminhoChave,videoParaBaixar.NomeZip());
            await s3BucketService.SubirArquivoAsync(videoParaBaixar.Bucket,caminhoS3Zip,videoParaBaixar.CaminhoZipCompleto());

            var caminhoS3Imagem = Path.Combine(videoParaBaixar.CaminhoChave,nomeFrame);
            var caminhoLocalFrame = Path.Combine(videoParaBaixar.DestinoImagens(),nomeFrame);
            await s3BucketService.SubirArquivoAsync(videoParaBaixar.Bucket,caminhoS3Imagem,caminhoLocalFrame);

            var successMessage = new VideoProcessingSuccessDto{
                    ObjectKey = videoParaBaixar.Chave,
                    ImagePath = caminhoS3Imagem,
                    ZipPath = caminhoS3Zip
                };
            return successMessage;
        }
    }
}