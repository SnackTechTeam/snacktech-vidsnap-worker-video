using core.domain.types;

namespace unit.tests.core.domain.types
{
    public class NumeroNaoNegativoOuZeroTests
    {
        [Fact]
        public void ConstrutorNumeroValido()
        {
            int numeroValido = 10;
            NumeroNaoNegativoOuZero numero = new NumeroNaoNegativoOuZero(numeroValido);

            Assert.Equal(numeroValido,numero.Valor);
        }

        [Fact]
        public void ConstrutorNumeroNegativo(){
            int numeroInvalido = -1;

            Assert.Throws<ArgumentException>(() => new NumeroNaoNegativoOuZero(numeroInvalido));
        }

        [Fact]
        public void ConstrutorNumeroZero(){
            int numeroInvalido = 0;

            Assert.Throws<ArgumentException>(() => new NumeroNaoNegativoOuZero(numeroInvalido));
        }

        [Fact]
        public void ConversaoDeNumeroNaoNegativoParaInt(){
            int numeroValido = 1;
            NumeroNaoNegativoOuZero numero = new NumeroNaoNegativoOuZero(numeroValido);
            int resultado = numero;

            Assert.Equal(numeroValido,resultado);
        }

        [Fact]
        public void ConversaoDeIntParaNumeroNaoNegativo()
        {
            int numeroValido = 1;
            NumeroNaoNegativoOuZero numero = numeroValido;
            
            Assert.Equal(numeroValido, numero.Valor);
        }

        [Fact]
        public void ChamarToStringComoEsperado(){
            int numeroValido = 10;
            NumeroNaoNegativoOuZero numero = new NumeroNaoNegativoOuZero(numeroValido);

            Assert.Equal(numeroValido.ToString(),numero.ToString());
        }

        [Fact]
        public void EqualsRetornaTrueComOsMesmosValores(){
            NumeroNaoNegativoOuZero numero1 = new NumeroNaoNegativoOuZero(1);
            NumeroNaoNegativoOuZero numero2 = new NumeroNaoNegativoOuZero(2);

            Assert.False(numero1.Equals(numero2));
        }

    }
}