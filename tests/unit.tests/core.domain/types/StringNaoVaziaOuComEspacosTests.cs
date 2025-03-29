using core.domain.types;

namespace unit.tests.core.domain.types
{
    public class StringNaoVaziaOuComEspacosTests
    {
        [Fact]
        public void ConstrutorStringValida()
        {
            string validString = "ValidString";
            StringNaoVaziaOuComEspacos stringNaoVazia = new StringNaoVaziaOuComEspacos(validString);

            Assert.Equal(validString, stringNaoVazia.Valor);
        }

        [Fact]
        public void ConstrutorStringInvalida()
        {
            string invalidString = "   ";

            Assert.Throws<ArgumentException>(() => new StringNaoVaziaOuComEspacos(invalidString));
        }

        [Fact]
        public void ConstrutorStringVazia()
        {
            string invalidString = "";

            Assert.Throws<ArgumentException>(() => new StringNaoVaziaOuComEspacos(invalidString));
        }

        [Fact]
        public void ConstrutorStringNula()
        {
            string? invalidString = null;

            Assert.Throws<ArgumentException>(() => new StringNaoVaziaOuComEspacos(invalidString!));
        }

        [Fact]
        public void ConversaoDeStringParaStringNaoVaziaOuComEspacos()
        {
            string validString = "ValidString";
            StringNaoVaziaOuComEspacos stringNaoVazia = validString;

            Assert.Equal(validString, stringNaoVazia.Valor);
        }

        [Fact]
        public void ConversaoDeStringNaoVaziaOuComEspacosParaString()
        {
            string validString = "ValidString";
            StringNaoVaziaOuComEspacos stringNaoVazia = new StringNaoVaziaOuComEspacos(validString);
            string resultString = stringNaoVazia;

            Assert.Equal(validString, resultString);
        }

        [Fact]
        public void EqualsRetornaTrueComOsMesmosValores()
        {
            string validString = "ValidString";
            StringNaoVaziaOuComEspacos stringNaoVazia1 = new StringNaoVaziaOuComEspacos(validString);
            StringNaoVaziaOuComEspacos stringNaoVazia2 = new StringNaoVaziaOuComEspacos(validString);

            Assert.True(stringNaoVazia1.Equals(stringNaoVazia2));
        }

        [Fact]
        public void EqualsRetornaFalseComValoresDiferentes()
        {
            StringNaoVaziaOuComEspacos stringNaoVazia1 = new StringNaoVaziaOuComEspacos("String1");
            StringNaoVaziaOuComEspacos stringNaoVazia2 = new StringNaoVaziaOuComEspacos("String2");

            Assert.False(stringNaoVazia1.Equals(stringNaoVazia2));
        }
    }
}