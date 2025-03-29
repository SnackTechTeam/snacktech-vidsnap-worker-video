
namespace core.domain.types
{
    public struct UrlS3Valida : IEquatable<UrlS3Valida>
    {
        internal Uri Valor {readonly get; private set;}

        public UrlS3Valida(string url){
            Valor = ValidarValor(url);
        }

        public UrlS3Valida(Uri uri){
            Valor = uri;
        }

        public static implicit operator UrlS3Valida(string url){
            return new UrlS3Valida(url);
        }

        public static implicit operator Uri(UrlS3Valida urlValida)
            => urlValida.Valor;

        public static implicit operator UrlS3Valida(Uri uri)
            => new UrlS3Valida(uri);

        public static implicit operator string(UrlS3Valida urlValida)
            => urlValida.Valor.ToString();

        public override readonly string ToString()
            => Valor.ToString();

        private static Uri ValidarValor(string url){
            if(Uri.TryCreate(url,UriKind.Absolute, out Uri? uriResult)
                    && uriResult.Scheme == "s3" 
                    && !string.IsNullOrEmpty(uriResult.Host)
                    && !string.IsNullOrEmpty(uriResult.AbsolutePath)){
                return uriResult;
            }
            else
            {
                throw new ArgumentException($"O valor informado '{url}' não é uma Url válida.");
            }
        }

        public bool Equals(UrlS3Valida other)
        {
            if(other == null) return false;

            return this.Valor == other.Valor;
        }
    }
}