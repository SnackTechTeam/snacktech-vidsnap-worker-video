using core.domain.models;
using core.domain.types;

namespace unit.tests.core.domain.models
{
    public class VideoParaBaixarTests
    {
         [Fact]
        public void DestinoLocalRetornaCaminhoCorreto()
        {
            var cliente = new GuidValido(Guid.NewGuid());
            var video = new VideoValido("example.mp4");
            var videoParaBaixar = new VideoParaBaixar
            {
                Cliente = cliente,
                NomeVideo = video
            };

            var expectedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "videos", cliente.ToString());

            var result = videoParaBaixar.DestinoLocal();

            Assert.Equal(expectedPath, result);
        }

        [Fact]
        public void CaminhoVideoCompletoRetornaCaminhoCorreto()
        {
            var cliente = new GuidValido(Guid.NewGuid());
            var video = new VideoValido("example.mp4");
            var videoParaBaixar = new VideoParaBaixar
            {
                Cliente = cliente,
                NomeVideo = video
            };

            var expectedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "videos", cliente.ToString(), video.ToString());

            var result = videoParaBaixar.CaminhoVideoCompleto();

            Assert.Equal(expectedPath, result);
        }

        [Fact]
        public void DestinoImagensRetornaCaminhoCorreto()
        {
            var cliente = new GuidValido(Guid.NewGuid());
            var video = new VideoValido("example.mp4");
            var videoParaBaixar = new VideoParaBaixar
            {
                Cliente = cliente,
                NomeVideo = video
            };

            var expectedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "videos", cliente.ToString(), "imagens");

            var result = videoParaBaixar.DestinoImagens();

            Assert.Equal(expectedPath, result);
        }

        [Fact]
        public void DestinoZipRetornaCaminhoCorreto()
        {
            var cliente = new GuidValido(Guid.NewGuid());
            var video = new VideoValido("example.mp4");
            var videoParaBaixar = new VideoParaBaixar
            {
                Cliente = cliente,
                NomeVideo = video
            };

            var expectedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "videos", cliente.ToString(), "zip");

            var result = videoParaBaixar.DestinoZip();

            Assert.Equal(expectedPath, result);
        }

        [Fact]
        public void NomeZipRetornaNomeCorreto()
        {
            var cliente = new GuidValido(Guid.NewGuid());
            var video = new VideoValido("example.mp4");
            var videoParaBaixar = new VideoParaBaixar
            {
                Cliente = cliente,
                NomeVideo = video
            };

            var expectedName = $"{cliente}-{video}.zip";

            var result = videoParaBaixar.NomeZip();

            Assert.Equal(expectedName, result);
        }

        [Fact]
        public void CaminhoZipCompletoRetornaCaminhoCorreto()
        {
            var cliente = new GuidValido(Guid.NewGuid());
            var video = new VideoValido("example.mp4");
            var videoParaBaixar = new VideoParaBaixar
            {
                Cliente = cliente,
                NomeVideo = video
            };

            var expectedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "videos", cliente.ToString(), "zip", $"{cliente}-{video}.zip");

            var result = videoParaBaixar.CaminhoZipCompleto();

            Assert.Equal(expectedPath, result);
        }
    }
}