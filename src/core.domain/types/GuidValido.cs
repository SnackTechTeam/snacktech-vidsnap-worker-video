namespace core.domain.types
{
    public struct GuidValido: IEquatable<GuidValido>
    {
        internal Guid Valor { readonly get; private set; }
        
        public GuidValido(string guid){
            Valor = ValidarValor(guid);
        }

        public GuidValido(Guid guid){
            Valor = guid;
        }

        public static implicit operator GuidValido(string guid)
        {
            return new GuidValido(guid);
        }

        public static implicit operator Guid(GuidValido guid)
        {
            return guid.Valor;
        }

        public static implicit operator GuidValido(Guid guid)
        {
            return new GuidValido(guid);
        }

        public static implicit operator string(GuidValido guid)
        {
            return guid.Valor.ToString();
        }

        public override readonly string ToString()
        {
            return Valor.ToString();
        }

        private static Guid ValidarValor(string guidValue)
        {
            if (Guid.TryParse(guidValue, out Guid guid))
            {
                return guid;
            }
            else
            {
                throw new ArgumentException($"O valor informado '{guidValue}' não é um Guid válido.");
            }
        }

        public bool Equals(GuidValido other)
        {
            if (other == null) return false;

            return this.Valor == other.Valor;
        }
    }
}