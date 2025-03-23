namespace core.domain.types
{
    public struct NumeroNaoNegativoOuZero: IEquatable<NumeroNaoNegativoOuZero>
    {
        private int valor = default!;
        public int Valor{ 
            get {return valor;}
            set{
                ValidarValor(value);
                valor = value;
            }
        }

        public NumeroNaoNegativoOuZero(int valor){
            ValidarValor(valor);
            Valor = valor;
        }

        public static implicit operator NumeroNaoNegativoOuZero(int valor)
            => new NumeroNaoNegativoOuZero(valor);

        public static implicit operator int(NumeroNaoNegativoOuZero valor)
            => valor.Valor;

        public override string ToString()
        {
            return Valor.ToString();
        }

        private static void ValidarValor(int valor){
            if(valor <= 0)
                throw new ArgumentException($"O número {valor} é menor ou igual a zero e não pode ser atribuído");
        }

        public bool Equals(NumeroNaoNegativoOuZero other){
            return this.Valor == other.Valor;
        }
    }
}