using core.domain.types;

namespace unit.tests.core.domain.types
{
    public class VideoValidoTests
    {
        [Fact]
        public void ValorValidoParaVideo()
        {
            var validVideoFile = "video.mp4";

            var videoValido = new VideoValido(validVideoFile);

            Assert.Equal(validVideoFile, videoValido.Valor);
        }

        [Theory]
        [InlineData("video.txt")]
        [InlineData("image.jpeg")]
        [InlineData("document.pdf")]
        [InlineData("audio.mp3")]
        public void LancarErroParaValoresComFormatoForaDoEsperado(string invalidFile)
        {
            var exception = Assert.Throws<ArgumentException>(() => new VideoValido(invalidFile));
            Assert.Equal("O valor atribuído não representa um arquivo de vídeo com formato válido", exception.Message);
        }

        [Fact]
        public void ConversaoImplicitaDeStringParaVideoValido()
        {
            var validVideoFile = "movie.avi";

            VideoValido videoValido = validVideoFile;

            Assert.Equal(validVideoFile, videoValido.Valor);
        }

        [Fact]
        public void ConversaoImplicitaDeVideoValidoParaString()
        {
            var validVideoFile = "clip.mov";
            var videoValido = new VideoValido(validVideoFile);

            string result = videoValido;

            Assert.Equal(validVideoFile, result);
        }

        [Fact]
        public void EqualsRetornaTrueParaValoresIguais()
        {
            var video1 = new VideoValido("video.mkv");
            var video2 = new VideoValido("video.mkv");

            var areEqual = video1.Equals(video2);

            Assert.True(areEqual);
        }

        [Fact]
        public void EqualsRetornaFalseParaValoresDiferentes()
        {
            var video1 = new VideoValido("video1.flv");
            var video2 = new VideoValido("video2.flv");

            var areEqual = video1.Equals(video2);
            
            Assert.False(areEqual);
        }
    }
}