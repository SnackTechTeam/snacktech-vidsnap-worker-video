using core.domain.types;

namespace unit.tests.core.domain.types
{
    public class GuidValidoTests
    {
        [Fact]
        public void ConstrutorGuidComoStringValido()
        {
            string validGuidString = "d2719b1e-1c5c-4d3b-9c8e-2e5b2c6d7f3a";
            GuidValido guidValido = new GuidValido(validGuidString);

            Assert.Equal(Guid.Parse(validGuidString), guidValido.Valor);
        }

        [Fact]
        public void ConstrutorGuidInvalido()
        {
            string invalidGuidString = "invalid-guid";

            Assert.Throws<ArgumentException>(() => new GuidValido(invalidGuidString));
        }

        [Fact]
        public void ConstrutorGuidValido()
        {
            Guid validGuid = Guid.NewGuid();
            GuidValido guidValido = new GuidValido(validGuid);

            Assert.Equal(validGuid, guidValido.Valor);
        }

        [Fact]
        public void ConversaoDeStringParaGuidValido()
        {
            string validGuidString = "d2719b1e-1c5c-4d3b-9c8e-2e5b2c6d7f3a";
            GuidValido guidValido = validGuidString;

            Assert.Equal(Guid.Parse(validGuidString), guidValido.Valor);
        }

        [Fact]
        public void ConversaoDeGuidParaGuidValido()
        {
            Guid validGuid = Guid.NewGuid();
            GuidValido guidValido = validGuid;

            Assert.Equal(validGuid, guidValido.Valor);
        }

        [Fact]
        public void ConversaoDeGuidValidoParaGuid()
        {
            Guid validGuid = Guid.NewGuid();
            GuidValido guidValido = new GuidValido(validGuid);
            Guid guid = guidValido;

            Assert.Equal(validGuid, guid);
        }

        [Fact]
        public void ConversaoDeGuidValidoParaString()
        {
            Guid validGuid = Guid.NewGuid();
            GuidValido guidValido = new GuidValido(validGuid);
            string guidString = guidValido;

            Assert.Equal(validGuid.ToString(), guidString);
        }

        [Fact]
        public void EqualsRetornaTrueComOsMesmosValores()
        {
            Guid validGuid = Guid.NewGuid();
            GuidValido guidValido1 = new GuidValido(validGuid);
            GuidValido guidValido2 = new GuidValido(validGuid);

            Assert.True(guidValido1.Equals(guidValido2));
        }

        [Fact]
        public void EqualsRetornaFalseComValoresDiferentes()
        {
            GuidValido guidValido1 = new GuidValido(Guid.NewGuid());
            GuidValido guidValido2 = new GuidValido(Guid.NewGuid());

            Assert.False(guidValido1.Equals(guidValido2));
        }
    }
}