namespace core.domain.types
{
    public struct UrlValida : IEquatable<UrlValida>
    {
        internal Uri Valor {readonly get; private set;}

        public UrlValida(string url){
            Valor = ValidarValor(url);
        }

        public UrlValida(Uri uri){
            Valor = uri;
        }

        public static implicit operator UrlValida(string url){
            return new UrlValida(url);
        }

        public static implicit operator Uri(UrlValida urlValida)
            => urlValida.Valor;

        public static implicit operator UrlValida(Uri uri)
            => new UrlValida(uri);

        public static implicit operator string(UrlValida urlValida)
            => urlValida.Valor.ToString();

        public override readonly string ToString()
            => Valor.ToString();

        private static Uri ValidarValor(string url){
            if(Uri.TryCreate(url,UriKind.Absolute, out Uri? uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)){
                return uriResult;
            }
            else
            {
                throw new ArgumentException($"O valor informado '{url}' não é uma Url válida.");
            }
        }

        public bool Equals(UrlValida other)
        {
            if(other == null) return false;

            return this.Valor == other.Valor;
        }
    }
}