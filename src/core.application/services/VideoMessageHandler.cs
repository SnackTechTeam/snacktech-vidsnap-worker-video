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
                                    ICompactService compactService){
            this.logger = logger;
            this.s3BucketService = s3BucketService;
            this.extrairImagensService = extrairImagensService;
            this.compactService = compactService;
        }

        public async Task<Result> ProcessVideoMessage(NewVideoDto newVideoDto){
            foreach(var newVideo in newVideoDto.Records){
                var objectKeyArray = newVideo.S3.Object.Key.Split("/");
                VideoParaBaixar videoParaBaixar = new VideoParaBaixar{
                    Cliente = objectKeyArray.First(),
                    NomeVideo = objectKeyArray.Last(),
                    Bucket = newVideo.S3.Bucket.Name,
                    Chave = newVideo.S3.Object.Key,
                    CaminhoChave = Path.Combine(objectKeyArray.Take(objectKeyArray.Length - 1).ToArray())
                };

                var resultDownloadFile = await s3BucketService.BaixarArquivoAsync(videoParaBaixar);
                
                if(!resultDownloadFile.IsSuccess())
                    return resultDownloadFile;
                
                //aqui eu tenho o nome do primeiro frame
                var resultImageExtraction = await extrairImagensService.ExtrairImagensPorIntervaloAsync(videoParaBaixar,ConstantsValues.IntervaloPadraoEmSegundos);

                if(!resultImageExtraction.IsSuccess())
                    return resultImageExtraction;

                var resultCompactingImages = compactService.CompactarImagensDeVideo(videoParaBaixar);

                if(!resultCompactingImages.IsSuccess())
                    return resultCompactingImages;

                var caminhoS3Zip = Path.Combine(videoParaBaixar.CaminhoChave,videoParaBaixar.NomeZip());
                var caminhoS3Imagem = Path.Combine(videoParaBaixar.CaminhoChave,resultImageExtraction.Data);
                var caminhoLocalFrame = Path.Combine(videoParaBaixar.DestinoImagens(),resultImageExtraction.Data);
                await s3BucketService.SubirArquivoAsync(videoParaBaixar.Bucket,caminhoS3Zip,videoParaBaixar.CaminhoZipCompleto());
                await s3BucketService.SubirArquivoAsync(videoParaBaixar.Bucket,caminhoS3Imagem,caminhoLocalFrame);

                Directory.Delete(videoParaBaixar.DestinoLocal(),true);
            }

            //enviar mensagem de sucesso            

            await Task.FromResult(0);
            return new Result();
        }
    }
}