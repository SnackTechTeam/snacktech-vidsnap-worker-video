using core.domain.common;
using core.domain.dtos.messages;
using core.domain.models;
using core.domain.ports.adapter.amazon.s3;
using core.domain.ports.core.application;
using Microsoft.Extensions.Logging;

namespace core.application.services
{
    public class VideoMessageHandler : IVideoMessageHandler
    {
        private readonly ILogger<VideoMessageHandler> logger;
        private readonly IS3BucketService s3BucketService;

        public VideoMessageHandler(ILogger<VideoMessageHandler> logger, 
                                    IS3BucketService s3BucketService){
            this.logger = logger;
            this.s3BucketService = s3BucketService;
        }

        public async Task<Result> ProcessVideoMessage(NewVideoDto newVideoDto){
            foreach(var newVideo in newVideoDto.Records){
                VideoParaBaixar videoParaBaixar = new VideoParaBaixar{
                    NomeVideo = newVideo.S3.Object.Key.Split("/").Last(),
                    Bucket = newVideo.S3.Bucket.Name,
                    Chave = newVideo.S3.Object.Key
                };

                var resultDownloadFile = await s3BucketService.BaixarArquivoAsync(videoParaBaixar);
            }

            //criar imagens
            //criar zip das imagens
            //publicar zip em S3
            //enviar mensagem de sucesso
            //remover video, imagens e zip do disco local

            await Task.FromResult(0);
            return new Result();
        }
    }
}