using core.domain.common;
using core.domain.dtos.messages;
using core.domain.enums;
using core.domain.models;
using core.domain.ports.adapter.amazon.s3;
using core.domain.ports.adapter.video;
using core.domain.ports.core.application;
using Microsoft.Extensions.Logging;

namespace core.application.services
{
    public class VideoMessageHandler : IVideoMessageHandler
    {
        private readonly IS3BucketService s3BucketService;
        private readonly IExtrairImagensService extrairImagensService;
        private readonly ICompactService compactService;

        public VideoMessageHandler(IS3BucketService s3BucketService,
                                    IExtrairImagensService extrairImagensService,
                                    ICompactService compactService)
        {
            this.s3BucketService = s3BucketService;
            this.extrairImagensService = extrairImagensService;
            this.compactService = compactService;
        }

        public async Task<Result<VideoProcessingStatusDto>> ProcessVideoMessage(VideoParaBaixar videoParaBaixar)
        {
            var execucaoVideo = await ExecutarProcessoVideo(videoParaBaixar);

            if(!execucaoVideo.IsSuccess()){
                return new Result<VideoProcessingStatusDto>(execucaoVideo.Exception);
            }
                    
            var mensagemSucesso = await UploadArquivosProduzidos(videoParaBaixar, execucaoVideo.Data);

            var destinoLocal = videoParaBaixar.DestinoLocal();

            if(Directory.Exists(destinoLocal))
                Directory.Delete(destinoLocal,true);

            return new Result<VideoProcessingStatusDto>(mensagemSucesso);            
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

        public async Task<VideoProcessingStatusDto> UploadArquivosProduzidos(VideoParaBaixar videoParaBaixar, string nomeFrame){
            var caminhoS3Zip = string.Join("/",videoParaBaixar.CaminhoChave,videoParaBaixar.NomeZip());
            await s3BucketService.SubirArquivoAsync(videoParaBaixar.Bucket,caminhoS3Zip,videoParaBaixar.CaminhoZipCompleto());

            var frameExtensao = Path.GetExtension(nomeFrame);
            var caminhoS3Imagem = string.Join("/",videoParaBaixar.CaminhoChave,$"image{frameExtensao}");
            var caminhoLocalFrame = Path.Combine(videoParaBaixar.DestinoImagens(),nomeFrame);
            await s3BucketService.SubirArquivoAsync(videoParaBaixar.Bucket,caminhoS3Imagem,caminhoLocalFrame);

            var successMessage = new VideoProcessingStatusDto{
                    IdVideo = videoParaBaixar.IdVideo,
                    UrlImagem = string.Join("/",$"s3://{videoParaBaixar.Bucket}",caminhoS3Imagem),
                    UrlZip = string.Join("/",$"s3://{videoParaBaixar.Bucket}",caminhoS3Zip),
                    Status = VideoStatusEnum.FinalizadoComSucesso.ToString()
                };
            return successMessage;
        }
    }
}