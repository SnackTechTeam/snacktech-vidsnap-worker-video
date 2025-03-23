using core.domain.types;

namespace unit.tests.core.domain.types
{
    public class UrlS3ValidaTests
    {
        [Fact]
        public void ConstrutorCriaInstanciaComStringS3Valida()
        {
            string validS3Url = "s3://bucket-name/object-key";

            var urlS3Valida = new UrlS3Valida(validS3Url);

            Assert.Equal(validS3Url, urlS3Valida.ToString());
        }

        [Fact]
        public void ConstrutorLancaErroComStringS3Invalida()
        {
            string invalidS3Url = "http://bucket-name/object-key";

            Assert.Throws<ArgumentException>(() => new UrlS3Valida(invalidS3Url));
        }

        [Fact]
        public void ConstrutorCriaInstanciaComUriValida()
        {
            Uri validS3Uri = new Uri("s3://bucket-name/object-key");

            var urlS3Valida = new UrlS3Valida(validS3Uri);

            Assert.Equal(validS3Uri, (Uri)urlS3Valida);
        }

        [Fact]
        public void ConversaoImplicaitaDeStringParaUrlS3()
        {
            string validS3Url = "s3://bucket-name/object-key";

            UrlS3Valida urlS3Valida = validS3Url;

            Assert.Equal(validS3Url, urlS3Valida.ToString());
        }

        [Fact]
        public void ConversaroImplicitaDeUrlS3ParaUri()
        {
            string validS3Url = "s3://bucket-name/object-key";
            UrlS3Valida urlS3Valida = new UrlS3Valida(validS3Url);

            Uri uri = urlS3Valida;

            Assert.Equal(validS3Url, uri.ToString());
        }

        [Fact]
        public void ConversaoImplicitaDeUrlS3ParaString()
        {
            string validS3Url = "s3://bucket-name/object-key";
            UrlS3Valida urlS3Valida = new UrlS3Valida(validS3Url);

            string urlString = urlS3Valida;

            Assert.Equal(validS3Url, urlString);
        }

        [Fact]
        public void EqualsRetornaTrueComValoresIguais()
        {
            string validS3Url = "s3://bucket-name/object-key";
            UrlS3Valida url1 = new UrlS3Valida(validS3Url);
            UrlS3Valida url2 = new UrlS3Valida(validS3Url);

            bool areEqual = url1.Equals(url2);

            Assert.True(areEqual);
        }

        [Fact]
        public void EqualsRetornaFalseComValoresDiferentes()
        {
            UrlS3Valida url1 = new UrlS3Valida("s3://bucket-name/object-key1");
            UrlS3Valida url2 = new UrlS3Valida("s3://bucket-name/object-key2");

            bool areEqual = url1.Equals(url2);

            Assert.False(areEqual);
        }
    }
}