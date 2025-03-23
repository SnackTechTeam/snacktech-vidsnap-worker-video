using core.domain.types;

namespace unit.tests.core.domain.types
{
    public class UrlValidaTests
    {
        [Fact]
        public void ConstrutorCriaInstanciaComUrlValida()
        {
            string validUrl = "https://example.com/";

            var urlValida = new UrlValida(validUrl);

            Assert.Equal(validUrl, urlValida.ToString());
        }

        [Fact]
        public void ConstrutorLancaExceptionComUrlInvalida()
        {
            string invalidUrl = "invalid-url";

            Assert.Throws<ArgumentException>(() => new UrlValida(invalidUrl));
        }

        [Fact]
        public void ConstrutorCriaInstanciaComUriValida()
        {
            var validUri = new Uri("https://example.com");

            var urlValida = new UrlValida(validUri);

            Assert.Equal(validUri, (Uri)urlValida);
        }

        [Fact]
        public void ConversaoImplicitaDeStringParaUrlValida()
        {
            string validUrl = "https://example.com/";

            UrlValida urlValida = validUrl;

            Assert.Equal(validUrl, urlValida.ToString());
        }

        [Fact]
        public void ConversaoImplicaitaDeUrlValidaParaString()
        {
            string validUrl = "https://example.com/";
            var urlValida = new UrlValida(validUrl);

            string result = urlValida;

            Assert.Equal(validUrl, result);
        }

        [Fact]
        public void ConversaoImplicaitaDeUrlValidaParaUri()
        {
            string validUrl = "https://example.com";
            var urlValida = new UrlValida(validUrl);

            Uri result = urlValida;

            Assert.Equal(new Uri(validUrl), result);
        }

        [Fact]
        public void ConversaoImplicitaDeUriParaUrlValida()
        {
            var validUri = new Uri("https://example.com");

            UrlValida urlValida = validUri;

            Assert.Equal(validUri, (Uri)urlValida);
        }

        [Fact]
        public void ChamarToStringCorretamente()
        {
            string validUrl = "https://example.com/";
            var urlValida = new UrlValida(validUrl);

            string result = urlValida.ToString();

            Assert.Equal(validUrl, result);
        }

        [Fact]
        public void EqualsRetornaTrueComOsMesmosValores()
        {
            var url1 = new UrlValida("https://example.com");
            var url2 = new UrlValida("https://example.com");

            bool areEqual = url1.Equals(url2);

            Assert.True(areEqual);
        }

        [Fact]
        public void EqualsRetornaFalseComValoresDiferentes()
        {
            var url1 = new UrlValida("https://example.com");
            var url2 = new UrlValida("https://different.com");

            bool areEqual = url1.Equals(url2);

            Assert.False(areEqual);
        }
    }
}