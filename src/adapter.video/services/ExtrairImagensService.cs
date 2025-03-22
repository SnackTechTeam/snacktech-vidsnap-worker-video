
using System.Globalization;
using core.domain.common;
using core.domain.models;
using core.domain.ports.adapter.video;
using Microsoft.Extensions.Logging;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace adapter.video.services
{
    public class ExtrairImagensService: IExtrairImagensService
    {
        private readonly ILogger<ExtrairImagensService> logger;
        public ExtrairImagensService(ILogger<ExtrairImagensService> logger){
            this.logger = logger;
        }

        public async Task<Result<string>> ExtrairImagensPorIntervaloAsync(VideoParaBaixar videoParaBaixar, int intervaloEmSegundos){
            try{
                var caminhoVideo = videoParaBaixar.CaminhoVideoCompleto();
                var caminhoImagens = videoParaBaixar.DestinoImagens();
                if(!File.Exists(caminhoVideo))
                    throw new FileNotFoundException("O arquivo não foi encontrado",caminhoVideo);

                Directory.CreateDirectory(caminhoImagens);

                var mediaInfo = await BuscarMediaInfoAsync(caminhoVideo);
                
                await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official);
                
                TimeSpan duracao = mediaInfo.Duration;
                var timestamps = PegarTimeStamps(duracao,intervaloEmSegundos);

                var frames = new List<string>();
                foreach(var timestamp in timestamps)
                {
                    var frameName = await ExtrairFramesNoTimeStamp(caminhoVideo,caminhoImagens,timestamp);
                    frames.Add(frameName);
                }

                logger.LogInformation($"Frames extraídos de {videoParaBaixar.NomeVideo} com sucesso");
                return new Result<string>(frames.First());
            }
            catch(Exception exception){
                logger.LogError(exception,$"Erro - ExtrairImagensPorIntervaloAsync - {exception.Message}");
                return new Result<string>(exception);
            }
        }

        private async Task<IMediaInfo> BuscarMediaInfoAsync(string caminhoLocal){
            if(!File.Exists(caminhoLocal))
                throw new FileNotFoundException("O arquivo não foi encontrado",caminhoLocal);

            var mediaInfo = await FFmpeg.GetMediaInfo(caminhoLocal);
            return mediaInfo;
        }

        private IEnumerable<string> PegarTimeStamps(TimeSpan duracao, int intervaloEmSegundos){
            var timeStamps = new List<string>();

            for(int i = 0; i < duracao.TotalSeconds; i += intervaloEmSegundos)
                timeStamps.Add(TimeSpan.FromSeconds(i).ToString(@"hh\:mm\:ss",CultureInfo.InvariantCulture));

            return timeStamps;
        }

        private async Task<string> ExtrairFramesNoTimeStamp(string caminhoLocal, string destinoLocal, string timeStamp){
            string nomeImagemSaida = $"frame_{timeStamp.Replace(":","_")}.png";
            string caminhoDestinoFinal = Path.Combine(destinoLocal,nomeImagemSaida);

            var conversao = FFmpeg.Conversions.New()
                            .AddParameter($"-ss {timeStamp} -i \"{caminhoLocal}\" -frames:v 1 \"{caminhoDestinoFinal}\"")
                            .SetOverwriteOutput(true);
            
            await conversao.Start();

            logger.LogInformation($"Frame extraído: {caminhoDestinoFinal}");
            return nomeImagemSaida;
        }
    }
}