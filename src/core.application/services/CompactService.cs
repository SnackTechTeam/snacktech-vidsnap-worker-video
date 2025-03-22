using System.IO.Compression;
using core.domain.common;
using core.domain.models;
using core.domain.ports.adapter.video;
using Microsoft.Extensions.Logging;

namespace core.application.services
{
    public class CompactService : ICompactService
    {
        private readonly ILogger<CompactService> logger;

        public CompactService(ILogger<CompactService> logger){
            this.logger = logger;
        }

        public Result CompactarImagensDeVideo(VideoParaBaixar videoParaBaixar){
            try{
                var caminhoImagens = videoParaBaixar.DestinoImagens();
                var caminhoZipCompleto = videoParaBaixar.CaminhoZipCompleto();

                Directory.CreateDirectory(videoParaBaixar.DestinoZip());
                
                logger.LogInformation($"Iniciando compactação de vídeo {videoParaBaixar.CaminhoVideoCompleto()}...");
                
                ZipFile.CreateFromDirectory(caminhoImagens,
                                            caminhoZipCompleto,
                                            CompressionLevel.Optimal,
                                            includeBaseDirectory: true);

                logger.LogInformation($"Vídeo {videoParaBaixar.CaminhoVideoCompleto()} compactado com sucesso em {caminhoZipCompleto}...");
                return new Result();
            }
            catch(Exception ex){
                logger.LogError(ex,$"Erro ao compactar vídeo {videoParaBaixar.CaminhoVideoCompleto()} - {ex.Message}");
                return new Result(ex);
            }
        }
    }
}