
using System.Globalization;
using core.domain.common;
using core.domain.ports.adapter.video;
using Microsoft.Extensions.Logging;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace adapter.video.services
{
    public class ExtrairImagensService:IExtrairImagensService
    {
        private readonly ILogger<ExtrairImagensService> logger;
        public ExtrairImagensService(ILogger<ExtrairImagensService> logger){
            this.logger = logger;
        }

        public async Task<Result> ExtrairImagensPorIntervaloAsync(string nomeArquivo, string caminhoLocal, string caminhoDestino, int intervaloEmSegundos){
            try{
                if(!File.Exists(caminhoLocal))
                    throw new FileNotFoundException("O arquivo não foi encontrado",caminhoLocal);

                Directory.CreateDirectory(caminhoDestino);

                var mediaInfo = await BuscarMediaInfoAsync(caminhoLocal);
                
                await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official);
                
                TimeSpan duracao = mediaInfo.Duration;
                var timestamps = PegarTimeStamps(duracao,intervaloEmSegundos);

                foreach(var timestamp in timestamps)
                    await ExtrairFramesNoTimeStamp(caminhoLocal,caminhoDestino,timestamp);

                logger.LogInformation($"Frames extraídos de {nomeArquivo} com sucesso");
                return new Result();
            }
            catch(Exception exception){
                logger.LogError(exception,$"Erro - ExtrairImagensPorIntervaloAsync - {exception.Message}");
                return new Result(exception);
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

        private async Task ExtrairFramesNoTimeStamp(string caminhoLocal, string destinoLocal, string timeStamp){
            string nomeImagemSaida = $"frame_{timeStamp.Replace(":","_")}.png";
            string caminhoDestinoFinal = Path.Combine(destinoLocal,nomeImagemSaida);

            var conversao = FFmpeg.Conversions.New()
                            .AddParameter($"-ss {timeStamp} -i \"{caminhoLocal}\" -frames:v 1 \"{caminhoDestinoFinal}\"")
                            .SetOverwriteOutput(true);
            
            await conversao.Start();

            logger.LogInformation($"Frame extraído: {caminhoDestinoFinal}");
        }
    }
}