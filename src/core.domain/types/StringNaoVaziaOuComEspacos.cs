namespace core.domain.types
{
    public class StringNaoVaziaOuComEspacos : IEquatable<StringNaoVaziaOuComEspacos>
    {
        private string valor = default!;

        public string Valor
        {
            get { return valor; }
            set
            {
                ValidarValorString(value);
                valor = value;
            }
        }

        public StringNaoVaziaOuComEspacos(string value)
        {
            Valor = value;
        }

        public static implicit operator StringNaoVaziaOuComEspacos(string value)
        {
            return new StringNaoVaziaOuComEspacos(value);
        }

        public static implicit operator string(StringNaoVaziaOuComEspacos valor)
        {
            return valor.ToString();
        }

        public override string ToString()
            => Valor;

        private static void ValidarValorString(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("O valor atribuído não pode ser nulo, vazio ou somente com espaços");
            }
        }

        public bool Equals(StringNaoVaziaOuComEspacos? other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.Valor == other.Valor;
        }
    }
}