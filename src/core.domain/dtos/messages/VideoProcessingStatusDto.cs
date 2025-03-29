using core.domain.enums;

namespace core.domain.dtos.messages
{
    public class VideoProcessingStatusDto
    {
        public string IdVideo {get; set;} = default!;
        public string UrlZip {get; set;} = default!;
        public string UrlImagem {get; set;} = default!;
        public string Status {get; set;} = default!;

        public static VideoProcessingStatusDto CriarParaFalha(string idVideo)
            => new VideoProcessingStatusDto{
                IdVideo = idVideo,
                UrlImagem = "",
                UrlZip = "",
                Status = VideoStatusEnum.FinalizadoComErro.ToString()
            };

        public static VideoProcessingStatusDto CriarParaInicioProcesso(string idVideo)
            => new VideoProcessingStatusDto{
                IdVideo = idVideo,
                UrlImagem = "",
                UrlZip = "",
                Status = VideoStatusEnum.Processando.ToString()
            };
    }
}